using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SwaggerExample.FalconRequests
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFalconPort
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspPage"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        Task<string> SubmitFalconRequest<T>(string aspPage, T requestData) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<string> SubmitCosecRequest<T>(T request) where T : class;
    }

    /// <summary>
    /// 
    /// </summary>
    public class SwaggerExamplePort : IFalconPort
    {
        private readonly HttpClient _httpClient;
        private readonly IEnvironmentConfiguration _environmentalConfiguration;
        SwaggerExampleCertificateValidationHandler _adxeCertificateValidationHandler;

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="environmentalConfiguration"></param>
        /// <param name="adxeCertificateValidationHandler"></param>
        public SwaggerExamplePort(IEnvironmentConfiguration environmentalConfiguration, SwaggerExampleCertificateValidationHandler adxeCertificateValidationHandler)
        {
            _environmentalConfiguration = environmentalConfiguration;
            _adxeCertificateValidationHandler = adxeCertificateValidationHandler;
            _httpClient = new HttpClient(_adxeCertificateValidationHandler);
            _httpClient.Timeout = TimeSpan.FromSeconds(300);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aspPage"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public async Task<string> SubmitFalconRequest<T>(string aspPage, T requestData) where T : class
        {
            try
            {
                var request = requestData.Serialize("Request");
                var requestContent = new StringContent(request);

                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                //requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                //requestContent.Headers.ContentType = new MediaTypeHeaderValue("text/xml");

                Uri uri = new Uri(_environmentalConfiguration.FalconServiceUrl + "/" + aspPage);

                // Make a request and get a response
                return await SubmitFalconRequestNGetResponse(uri, requestContent);
            }
            catch (Exception ex)
            {
                ex.ToString(); // get rid of warning
                throw;
            }
        }
        #endregion Public Methods

        #region Private Methods
        private async Task<string> SubmitFalconRequestNGetResponse(Uri uri, HttpContent requestContent)
        {
            var requestResponse = await _httpClient.PostAsync(uri, requestContent);

            if (requestResponse.IsSuccessStatusCode)
            {
                var responseXml = await requestResponse.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseXml))
                {
                    var xDoc = XDocument.Parse(responseXml);
                    CheckFalconResponse(xDoc);
                }
                else
                {
                    Console.WriteLine("Response from Falcon app is null or empty!");
                }

                return responseXml;
            }
            else
            {
                throw new HttpStatusException(requestResponse.StatusCode, requestResponse.ReasonPhrase, requestResponse.RequestMessage.ToString());
            }
        }

        private void CheckFalconResponse(XContainer xDoc)
        {
            XElement eleResponse = xDoc.Element("Response");
            if (eleResponse != null)
            {
                XAttribute attrError = eleResponse.Attribute("Error");
                XAttribute attrErrorDesc = eleResponse.Attribute("Desc");
                if (attrError != null && attrErrorDesc != null)
                {
                    var errorNum = attrError.Value;
                    var errorDesc = attrErrorDesc.Value;
                    if (Convert.ToInt32(errorNum) != 0)
                    {
                        throw new HttpStatusException($"Falcon app response error code:{ errorNum }, description: { errorDesc }");
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(errorNum) || !string.IsNullOrWhiteSpace(errorDesc))
                        {
                            throw new HttpStatusException($"Falcon app response error code:{ errorNum }, description: { errorDesc }");
                        }
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public async Task<string> SubmitCosecRequest<T>(T requestData) where T : class
        {
            try
            {
                var content = new StringContent(string.Join("&", requestData.SerializeAsKeyValuePairs().Select(kvp => kvp.Key + "=" + kvp.Value)));

                content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                content.Headers.Add("Cookie", "Credential=new;OracleID=;AIPid=;MRPid=;DALogout=;DAState=;ADPCSGCoSec=;");
                Uri uri = new Uri(_environmentalConfiguration.CosecUrl);

                var response = await _httpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    response.Headers.TryGetValues("Set-Cookie", out var values);
                    string ticket = values?.FirstOrDefault() ?? "";
                    if (ticket?.ToLowerInvariant().Contains("credential=") == true || ticket?.ToLowerInvariant().Contains("falconticket=") == true)
                    {

                        return $"{{ 'status' : 'success', '{ ticket.Replace("ADPCSGCoSec=", "").Replace("&", "', '").Replace("=", "' : '").Replace(";", "'") }'}}";
                    }

                    return JsonConvert.SerializeObject(new { status = "failure", error = GetErrorMsg(response) });
                }
                else
                {
                    throw new HttpStatusException(response.StatusCode, response.ReasonPhrase, response.RequestMessage.ToString()); // "Incorrect UserId or Password");
                }
            }
            catch (Exception ex)
            {
                ex.ToString(); // get rid of warning
                throw;
            }
        }

        string GetErrorMsg(HttpResponseMessage response)
        {
            Dictionary<string, StringValues> nvc = QueryHelpers.ParseQuery(response.RequestMessage.RequestUri.Query);

            string code = nvc?["failure-code"] ?? "<unknown failure>";
            switch (code)
            {
                case "0":
                //return "Error code: " + code + ";\r\nUser ID and/or Password is incorrect!"; User does not exist. ???
                case "1":
                    return "Login_Bad_Login";
                case "2":
                    return "Login_Too_Many_Attempts";
                default:
                    return $"Error_Code_Not_Implemented='{ code }'";
            }
        }

        #endregion Private Methods
    }
}