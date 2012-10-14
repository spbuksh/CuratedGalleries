using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net;
using Corbis.Common;

namespace Corbis.Public.Repository
{
    /// <summary>
    /// Http request verbs
    /// </summary>
    public enum HttpVerbs
    {
        Get,
        Post,
        Put,
        Delete,
        Head,
        Patch,
        Options
    }

    public class CorbisApiError
    {
        public string ErrorType { get; set; }
        public string ErrorDescription { get; set; }
    }
    public class CorbisApiResponse<T>
    {
        public virtual HttpStatusCode StatusCode { get; set; }

        public virtual CorbisApiError Error { get; set; }

        public virtual T Content { get; set; }

        public virtual bool IsSuccess
        {
            get { return object.ReferenceEquals(this.Error, null) && this.StatusCode != HttpStatusCode.Unauthorized; }
        }
    }


    /// <summary>
    /// Helper to work with corbis images web api
    /// </summary>
    public class CorbisWebApiHelper
    {
        /// <summary>
        /// Executes web http request to the Corbis images api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="verb"></param>
        /// <param name="converter">Body content converter. Converts json string into object of the specific type. If it is null then standard default json deserialization is used</param>
        /// <param name="content">
        /// System.Net.Http.ByteArrayContent
        /// System.Net.Http.HttpMessageContent -- media type "application/http"
        /// System.Net.Http.MultipartContent
        /// System.Net.Http.FormUrlEncodedContent -- media type "application/x-www-form-urlencoded"
        /// System.Net.Http.HttpMessageContent
        /// System.Net.Http.MultipartFormDataContent
        /// System.Net.Http.StreamContent
        /// System.Net.Http.StringContent
        /// System.Net.Http.ObjectContent
        /// </param>
        /// <param name="handler">Creates output based on response message</param>
        /// <returns></returns>
        public static CorbisApiResponse<T> ExecHttpRequest<T>(string uri, 
            HttpVerbs verb = HttpVerbs.Get, 
            ActionHandler<string, T> converter = null,
            HttpContent content = null,
            ActionHandler<HttpResponseMessage, ActionHandler<string, T>, CorbisApiResponse<T>> handler = null)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage rmsg = null;

            switch (verb)
            {
                case HttpVerbs.Get:
                    rmsg = client.GetAsync(uri).Result;
                    break;
                case HttpVerbs.Post:
                    rmsg = client.PostAsync(uri, content).Result;
                    break;
                case HttpVerbs.Delete:
                    rmsg = client.DeleteAsync(uri).Result;
                    break;
                case HttpVerbs.Put:
                    rmsg = client.PutAsync(uri, content).Result;
                    break;
                //other cases I did not consider due to in most cases only pointed above verbs will be used
                default:
                    throw new NotImplementedException();
            }

            if (handler == null)
                handler = DefaultHandler<T>;

            return handler(rmsg, converter);
        }

        /// <summary>
        /// Default http response handler logic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static CorbisApiResponse<T> DefaultHandler<T>(HttpResponseMessage msg, ActionHandler<string, T> converter = null)
        {
            switch (msg.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        if (converter == null)
                            converter = x => System.Web.Helpers.Json.Decode<T>(x);

                        return new CorbisApiResponse<T>() { Content = converter(msg.Content.ReadAsStringAsync().Result), StatusCode = msg.StatusCode };
                    }
                case HttpStatusCode.BadRequest:
                    dynamic e = System.Web.Helpers.Json.Decode(msg.Content.ReadAsStringAsync().Result);
                    return new CorbisApiResponse<T>() { Error = new CorbisApiError() { ErrorType = e.error.ToString().ToLower(), ErrorDescription = e.error_description.ToString() }, StatusCode = msg.StatusCode };
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    return new CorbisApiResponse<T>() { StatusCode = msg.StatusCode };
                case HttpStatusCode.NotFound:
                    dynamic err = System.Web.Helpers.Json.Decode(msg.Content.ReadAsStringAsync().Result);
                    return new CorbisApiResponse<T>() { Error = new CorbisApiError() { ErrorType = err.error.ToString().ToLower(), ErrorDescription = err.error_description.ToString() }, StatusCode = msg.StatusCode };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
