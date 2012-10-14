using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

using Corbis.Common;
using Corbis.Public.Repository.Interface;
using Corbis.Public.Entity;
using Corbis.Common.WebApi;
using Corbis.Public.Entity.Search;
using System.Threading;
using System.Globalization;

namespace Corbis.Public.Repository
{
    public class SearchRepository : RepositoryBase, ISearchRepository
    {
        /// <summary>
        /// Separator for multiple CorbisImages WebAPI url parameter values
        /// </summary>
        protected string PmtrValueSeparator
        {
            get { return ","; }
        }

        ///// <summary>
        ///// Searching images by user setted search value
        ///// </summary>
        ///// <param name="searchvalue">search value</param>
        ///// <param name="token">user active access token</param>
        ///// <returns></returns>
        //public CorbisSearchResult Search(string searchvalue, Guid token)
        //{
        //    WebClient client = new WebClient();
        //    string searchresult = client.DownloadString(string.Format("{0}/v2/search/?q={1}&access_token={2}",
        //        WebApiClient.Instance.ApiUrl.TrimEnd(Path.AltDirectorySeparatorChar),
        //        searchvalue,
        //        token));

        //    var outputs = System.Web.Helpers.Json.Decode(searchresult);
        //    CorbisSearchResult result = new CorbisSearchResult();

        //    foreach (var item in outputs.Results)
        //    {
        //        result.Result.Add(new CorbisSearchResultItem()
        //        {
        //            CorbisId = item.CorbisId,
        //            CutdownUrl128 = item.CutdownUrl128,
        //            CutdownUrl170 = item.CutdownUrl170,
        //            CutdownUrl350 = item.CutdownUrl350,
        //            SearchResultValues = new SearchResultValue()
        //            {
        //                AssetId = item.SearchResultValues.AssetId,
        //                score = item.SearchResultValues.score
        //            },
        //            MetadataJson = new MetadataJson()
        //            {
        //                AssetType = item.MetadataJson.AssetType,
        //                Title = item.MetadataJson.Title,
        //                DatePhotographed = item.MetadataJson.DatePhotographed,
        //                WidthPx = item.MetadataJson.WidthPx,
        //                HeightPx = item.MetadataJson.HeightPx,
        //                HasRelatedImages = item.MetadataJson.HasRelatedImages,
        //                CreditLine = item.MetadataJson.CreditLine,
        //                Category = item.MetadataJson.Category,
        //                CategoryId = item.MetadataJson.CategoryId,
        //                CollectionName = item.MetadataJson.CollectionName,
        //                CorbisId = item.MetadataJson.CorbisId,
        //                ImageMediaType = item.MetadataJson.ImageMediaType,
        //                Location = item.MetadataJson.Location,
        //                MarketingCollectionId = item.MetadataJson.MarketingCollectionId,
        //                MarketingCollectionName = item.MetadataJson.MarketingCollectionName,
        //                Photographer = item.MetadataJson.Photographer,
        //                Placement = item.MetadataJson.Placement,
        //                LicenseType = item.MetadataJson.LicenseType
        //            }
        //        });

        //    }

        //    return result;
        //}


        #region Autocomplete

        public OperationResult<OperationResults, List<string>> Autocomplete(string text, Guid token, int resultCount = 10, System.Globalization.CultureInfo culture = null)
        {
            if (culture == null)
                culture = CultureInfo.InvariantCulture;

            string uri = string.Format("{0}/search/autocomplete?term={1}&lcid={2}&maxresults={3}&access_token={4}",
                WebApiClient.Instance.ApiUrl.TrimEnd(Path.AltDirectorySeparatorChar),
                text.GetUrlEncoded(),
                (culture == null ? Thread.CurrentThread.CurrentCulture : culture).Name,
                resultCount,
                token);

            CorbisApiResponse<List<string>> response = null;

            try
            {
                response = CorbisWebApiHelper.ExecHttpRequest<List<string>>(uri);
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex, "Image autocomplete text search request to Corbis Images WebAPI is failed");
#if DEBUG
                throw;                
#else
                return new OperationResult<OperationResults, List<string>>() { Result = OperationResults.Failure };
#endif
            }

            var rslt = new OperationResult<OperationResults, List<string>>();

            if (response.IsSuccess)
            {
                rslt.Result = OperationResults.Success;
                rslt.Output = response.Content;
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        rslt.Result = OperationResults.Success;
                        rslt.Output = response.Content;
                        break;
                    case HttpStatusCode.InternalServerError:
                        break;
                    case HttpStatusCode.Unauthorized:
                        rslt.Result = OperationResults.Unathorized;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                rslt.Result = OperationResults.Failure;
            }

            return rslt;
        }

        #endregion Autocomplete

        #region Regular Search

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public OperationResult<OperationResults, ImageSearchResult> Search(ImageSearchContext context, Guid token, CultureInfo culture = null)
        {
            if (culture == null)
                culture = CultureInfo.InvariantCulture;

            string url = this.BuildSearchUrl(context, token, culture);

            var rslt = CorbisWebApiHelper.ExecHttpRequest<ImageSearchResult>(url, HttpVerbs.Get, x => this.ParseResults(x, culture));

            switch(rslt.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new OperationResult<OperationResults, ImageSearchResult>() { Result = OperationResults.Success, Output = rslt.Content };
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    return new OperationResult<OperationResults, ImageSearchResult>() { Result = OperationResults.Unathorized };
                case HttpStatusCode.InternalServerError:
                    return new OperationResult<OperationResults, ImageSearchResult>() { Result = OperationResults.Failure };
                default:
                    throw new NotImplementedException();
            }

            throw new NotImplementedException();
        }

        protected string BuildSearchUrl(ImageSearchContext context, Guid token, CultureInfo culture)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.AppendFormat("{0}/search/?access_token={1}", WebApiClient.Instance.ApiUrl.TrimEnd(Path.AltDirectorySeparatorChar), token);

            var mode = context.SearchMode.HasValue ? context.SearchMode.Value : SearchModes.Regular;

            switch (mode)
            {
                case SearchModes.Identifier:
                    {
                        urlBuilder.AppendFormat("&in={0}", context.SearchText.GetUrlEncoded());

                        //other parameters are ignored
                        return urlBuilder.ToString();
                    }
                case SearchModes.MoreOrLikeThis:
                    {
                        urlBuilder.AppendFormat("&mlt={0}", context.SearchText.GetUrlEncoded());

                        //other parameters are ignored
                        return urlBuilder.ToString();
                    }
                case SearchModes.Regular:
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (!string.IsNullOrEmpty(context.SearchText))
                urlBuilder.AppendFormat("&q={0}", context.SearchText.GetUrlEncoded());

            #region Image Attributes

            if (context.Categories != null && context.Categories.Count != 0)
                urlBuilder.AppendFormat("&cat={0}", context.Categories.ToString(this.PmtrValueSeparator).GetUrlEncoded());

            if (context.Type.HasValue && context.Type != ImageTypes.All)
            {
                string pmtrValue = context.Type.Value.ToString(this.PmtrValueSeparator, delegate(Enum eitem) { return this.Convert((ImageTypes)eitem).ToString(); });
                urlBuilder.AppendFormat("&mt={0}", pmtrValue.GetUrlEncoded());
            }

            if (context.ColorFormat.HasValue && context.ColorFormat.Value != ColorFormats.All)
            {
                string pmtrValue = context.ColorFormat.Value.ToString(this.PmtrValueSeparator, delegate(Enum eitem) { return this.Convert((ColorFormats)eitem).ToString(); });
                urlBuilder.AppendFormat("&cf={0}", pmtrValue.GetUrlEncoded());
            }

            if (context.Size.HasValue && context.Size.Value != ImageSizes.All)
            {
                string pmtrValue = context.Size.Value.ToString(this.PmtrValueSeparator, delegate(Enum eitem) { return this.Convert((ImageSizes)eitem).ToString(); });
                urlBuilder.AppendFormat("&ia={0}", pmtrValue.GetUrlEncoded());
            }

            if (context.Orientation.HasValue && context.Orientation.Value != ImageOrientations.All)
            {
                string pmtrValue = context.Orientation.Value.ToString(this.PmtrValueSeparator, delegate(Enum eitem) { return this.Convert((ImageOrientations)eitem).ToString(); });
                urlBuilder.AppendFormat("&or={0}", pmtrValue.GetUrlEncoded());
            }

            if (context.IsModelReleased.HasValue)
                urlBuilder.AppendFormat("&mr={0}", context.IsModelReleased.Value.ToString().ToLower());

            if(context.MarketingCollections != null && context.MarketingCollections.Count != 0)
                urlBuilder.AppendFormat("&mrc={0}", context.MarketingCollections.ToString(this.PmtrValueSeparator));

            #endregion Image Attributes

            #region Composition Attributes

            if (context.Viewpoint.HasValue && context.Viewpoint.Value != CompositionViewpoint.All)
            {
                string pmtrValue = context.Viewpoint.Value.ToString(this.PmtrValueSeparator, delegate(Enum eitem) { return this.Convert((CompositionViewpoint)eitem).ToString(); });
                urlBuilder.AppendFormat("&pv={0}", pmtrValue.GetUrlEncoded());
            }

            #endregion Composition Attributes

            #region People

            if(context.NumberOfPeople.HasValue && context.NumberOfPeople.Value != NumbersOfPeople.All)
            {
                string pmtrValue = context.NumberOfPeople.Value.ToString(this.PmtrValueSeparator, delegate(Enum eitem) { return this.Convert((NumbersOfPeople)eitem).ToString(); });
                urlBuilder.AppendFormat("&np={0}", pmtrValue.GetUrlEncoded());
            }

            #endregion People

            #region Misc

            if (!string.IsNullOrEmpty(context.DatePhotographed))
                urlBuilder.AppendFormat("&dr={0}", context.DatePhotographed.GetUrlEncoded());

            if (context.LastDaysAdded.HasValue)
                urlBuilder.AppendFormat("&ma={0}", context.LastDaysAdded.Value);

            if (context.DateAdded != null)
            {
                urlBuilder.AppendFormat("&bd={0}", context.LastDaysAdded.Value);
            }

            if (context.DateCreated != null)
            {
                //urlBuilder.AppendFormat("&bd={0}", context.LastDaysAdded.Value);
            }

            if (!string.IsNullOrEmpty(context.Location))
                urlBuilder.AppendFormat("&lc={0}", context.Location.GetUrlEncoded());

            if (!string.IsNullOrEmpty(context.Photographer))
                urlBuilder.AppendFormat("&pg={0}", context.Photographer.GetUrlEncoded());

            #endregion Misc

            #region Paging

            if(context.PageNumber.HasValue)
                urlBuilder.AppendFormat("&p={0}", context.PageNumber.Value);

            if (context.PageLength.HasValue)
                urlBuilder.AppendFormat("&s={0}", context.PageLength.Value);

            #endregion Paging

            return urlBuilder.ToString();
        }

        /// <summary>
        /// Parses json and builds search result
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        protected ImageSearchResult ParseResults(string json, CultureInfo culture)
        {
            var source = System.Web.Helpers.Json.Decode(json);

            var output = new ImageSearchResult() { HitCount = source.HitCount };

            foreach (var item in source.Results)
            {
                var si = new ImageSearchResultItem();
                si.CategoryID = item.MetadataJson.CategoryId;
                si.CategoryName = item.MetadataJson.Category;
                si.CollectionID = item.MetadataJson.MarketingCollectionId;
                si.CollectionName = item.MetadataJson.CollectionName;
                si.CorbisID = item.MetadataJson.CorbisId;
                si.CutdownUrl128 = item.CutdownUrl128;
                si.CutdownUrl170 = item.CutdownUrl170;
                si.CutdownUrl350 = item.CutdownUrl350;
                si.DatePhotographed = string.IsNullOrEmpty(item.MetadataJson.DatePhotographed) ? (DateTime?)null : DateTime.ParseExact(item.MetadataJson.DatePhotographed, "MMMM dd, yyyy", culture);
                si.FileSize = item.MetadataJson.FileSize;
                si.HasRelatedImages = item.MetadataJson.HasRelatedImages;
                si.ImageID = item.MetadataJson.ImageId;
                si.LicenseModel = (LicenseModels)Enum.Parse(typeof(LicenseModels), item.MetadataJson.LicenseType);
                si.Location = item.MetadataJson.Location;
                si.Photographer = item.MetadataJson.Photographer;
                si.Resolution = item.MetadataJson.Resolution;
                si.Score = item.Score;
                si.SizeInch = new Size<double>() { Height = item.MetadataJson.HeightInch, Width = item.MetadataJson.WidthInch };
                si.SizePx = new Size<int>() { Height = item.MetadataJson.HeightPx, Width = item.MetadataJson.WidthPx };
                si.Title = item.MetadataJson.Title;
                output.Items.Add(si);
            }

            return output;
        }

        protected int Convert(ImageTypes item)
        {
            return (int)item;
        }
        protected int Convert(ColorFormats item)
        {
            return (int)item;
        }
        protected int Convert(ImageSizes item)
        {
            switch (item)
            {
                case ImageSizes.Web:
                    return 67;
                case ImageSizes.Small:
                    return 64;
                case ImageSizes.Medium:
                    return 561;
                case ImageSizes.Lagest:
                    return 559;
                default:
                    throw new NotImplementedException();
            }
        }
        protected int Convert(ImageOrientations item)
        {
            switch (item)
            {
                case ImageOrientations.Vertical:
                    return 4;
                case ImageOrientations.Horizontal:
                    return 2;
                case ImageOrientations.Panorama:
                    return 1;
                case ImageOrientations.Square:
                    return 3;
                default:
                    throw new NotImplementedException();
            }
        }
        protected int Convert(CompositionViewpoint item)
        {
            //all values are received from html in CorbisImages.com search filter
            switch (item)
            {
                case CompositionViewpoint.Aerial:
                    return 553721;
                case CompositionViewpoint.CloseUp:
                    return 553727;
                case CompositionViewpoint.FromAbove:
                    return 553728;
                case CompositionViewpoint.FromBehind:
                    return 553733;
                case CompositionViewpoint.FromBelow:
                    return 553735;
                case CompositionViewpoint.LookingAtCamera:
                    return 553741;
                case CompositionViewpoint.LookingAwayFromCamera:
                    return 553743;
                default:
                    throw new NotImplementedException();
            }
        }
        protected int Convert(NumbersOfPeople item)
        {
            //all values are received from html in CorbisImages.com search filter
            switch (item)
            {
                case NumbersOfPeople.NoPeople:
                    return 6;
                case NumbersOfPeople.Person1:
                    return 1;
                case NumbersOfPeople.Person2:
                    return 2;
                case NumbersOfPeople.Person3:
                    return 9;
                case NumbersOfPeople.Person4:
                    return 7;
                case NumbersOfPeople.Person5:
                    return 8;
                case NumbersOfPeople.Group:
                    return 4;
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion Regular Search
    }
}