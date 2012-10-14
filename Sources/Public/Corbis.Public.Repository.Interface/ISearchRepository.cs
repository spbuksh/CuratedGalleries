using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Public.Entity;
using Corbis.Public.Entity.Search;
using System.Globalization;
using Corbis.Common;

namespace Corbis.Public.Repository.Interface
{
    /// <summary>
    /// Repository to perform search actions
    /// </summary>
    public interface ISearchRepository
    {
        /// <summary>
        /// Searching images by user setted search filters
        /// </summary>
        /// <param name="context">Search context. Contains search filters</param>
        /// <param name="token">user active access token</param>
        /// <param name="culture">Culture</param>
        /// <returns></returns>
        OperationResult<OperationResults, ImageSearchResult> Search(ImageSearchContext context, Guid token, CultureInfo culture = null);

        /// <summary>
        /// Image text search autocomplete
        /// </summary>
        /// <param name="text">Text for autocomple processing. It is required field</param>
        /// <param name="token">User access token. It is required field</param>
        /// <param name="resultCount">This is the number of results that are returned. By default it uses 10 items</param>
        /// <param name="culture">This is the culture identity of the text. By default current thread culture is used</param>
        /// <returns></returns>
        OperationResult<OperationResults, List<string>> Autocomplete(string text, Guid token, int resultCount = 10, CultureInfo culture = null);
    }
}
