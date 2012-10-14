using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using Corbis.WebAPI.Models;
using System.Net;
using Corbis.WebAPI.Code;
using Corbis.Public.Repository.Interface;
using Microsoft.Practices.Unity;
using System.Globalization;
using Corbis.Common;
using Corbis.Public.Entity.Search;

namespace Corbis.WebAPI.Controllers.API
{
    /// <summary>
    /// Search controller
    /// </summary>
    public class SearchController : ApiControllerBase
    {
        /// <summary>
        /// Search repository
        /// </summary>
        [Dependency]
        public ISearchRepository SearchRepository { get; set; }

        #region Autocomplete

        /// <summary>
        /// Retrieves the current suggested terms to be presented to the user for use as search term
        /// </summary>
        /// <param name="model">User data for processing</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Search([FromBody]ImageSearchModel model)
        {
            var context = new ImageSearchContext();
            context.SearchMode = model.SearchMode;
            context.SearchText = model.SearchText;

            OperationResult<OperationResults, ImageSearchResult> rslt = null;

            try
            {
                rslt = this.SearchRepository.Search(context, this.AccessToken.Value);
            }
            catch(Exception ex)
            {
                this.Logger.WriteError(ex);
#if DEBUG
                throw;
#else
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
#endif
            }

            switch(rslt.Result)
            {
                case OperationResults.Success:
                    return this.Request.CreateResponse<ApiResponse<ImageSearchResult>>(HttpStatusCode.OK, new ApiResponse<ImageSearchResult>() { Result = SearchApiActionCodes.Success, Output = rslt.Output });
                case OperationResults.Failure:
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError, rslt);
                case OperationResults.Unathorized:
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized);
                default:
                    throw new NotImplementedException();

            }
        }

        /// <summary>
        /// Retrieves the current suggested terms to be presented to the user for use as search term
        /// </summary>
        /// <param name="model">User data for processing</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Autocomplete([FromBody]SearchAutocompleteModel model)
        {
            var rdata = new ApiResponse<List<string>>();

            if (!this.ModelState.IsValid)
            {
                rdata.Result = SearchApiActionCodes.ValidationFailure;
                rdata.Errors = this.ModelState.GetErrorList();
                return this.Request.CreateResponse<ApiResponse<List<string>>>(HttpStatusCode.BadRequest, rdata);
            }

            //TODO: Custom parameter binder for CultureInfo will be implemented
            CultureInfo culture = null;

            try
            {
                culture = CultureInfo.GetCultureInfo(model.Culture);
            }
            catch(CultureNotFoundException)
            {
                rdata.Result = SearchApiActionCodes.ValidationFailure;
                rdata.Errors = new List<string>() { "Culture is not valid" };
                return this.Request.CreateResponse<ApiResponse<List<string>>>(HttpStatusCode.BadRequest, rdata);
            }
            
            OperationResult<OperationResults, List<string>> rslt = null;

            try
            {
                rslt = this.SearchRepository.Autocomplete(model.Text, this.AccessToken.Value, model.MaxResultCount.Value, culture);
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex);

                rdata.Result = SearchApiActionCodes.Failure;
                rdata.Errors = new List<string>() { "Error inside autocomplete method" };
                return this.Request.CreateResponse<ApiResponse<List<string>>>(HttpStatusCode.InternalServerError, rdata);
            }

            switch(rslt.Result)
            {
                case OperationResults.Unathorized:
                    return this.Request.CreateResponse<ApiResponse<List<string>>>(HttpStatusCode.Unauthorized, null);
                case OperationResults.Success:
                    { 
                        rdata.Result = SearchApiActionCodes.Success;
                        rdata.Output = rslt.Output;
                        return this.Request.CreateResponse<ApiResponse<List<string>>>(HttpStatusCode.NotFound, rdata);
                    }
                case OperationResults.Failure:
                    return this.Request.CreateResponse<ApiResponse<List<string>>>(HttpStatusCode.InternalServerError, null);
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion Autocomplete

    }
}