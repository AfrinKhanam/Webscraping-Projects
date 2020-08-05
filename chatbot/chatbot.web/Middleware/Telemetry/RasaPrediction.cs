using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Rest;

using Newtonsoft.Json;

namespace IndianBank_ChatBOT.Middleware.Telemetry
{
    public class RasaPrediction : IPrediction
    {
        #region constructor

        /// <summary>
        /// Initializes a new instance of the Prediction class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the service client.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when a required parameter is null
        /// </exception>
        public RasaPrediction(RasaRuntimeClient client, string baseUrl, string rasaProjectName)
        {
            Client = client ?? throw new System.ArgumentNullException("client");
            BaseUrl = baseUrl;
            RasaProjectName = rasaProjectName;
        }

        #endregion

        #region properties

        public RasaRuntimeClient Client { get; private set; }
        public string BaseUrl { get; set; }
        public string RasaProjectName { get; set; }

        #endregion 

        #region method
        /// <summary>
        /// Gets a reference to the LUISRuntimeClient
        /// </summary>
        public async Task<HttpOperationResponse<LuisResult>> ResolveWithHttpMessagesAsync(string appId, string query, double? timezoneOffset = default(double?), bool? verbose = default(bool?), bool? staging = default(bool?), bool? spellCheck = default(bool?), string bingSpellCheckSubscriptionKey = default(string), bool? log = default(bool?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Tracing
            bool shouldTrace = ServiceClientTracing.IsEnabled;
            string invocationId = null;
            // Construct URL
            var url = new Uri(new Uri(BaseUrl), "/parse");
            // Create HTTP transport objects
            var httpRequest = new HttpRequestMessage();
            HttpResponseMessage httpResponse = null;
            httpRequest.Method = new HttpMethod("POST");
            httpRequest.RequestUri = url;
            // Serialize Request
            string requestContent = null;
            string responseContent = null;

            if (Client.Endpoint == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "this.Client.Endpoint");
            }

            if (string.IsNullOrEmpty(query))
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "query");
            }

            if (!string.IsNullOrEmpty(query))
            {
                if (query.Length > 500)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "query", 500);
                }
            }

            // Set Headers
            if (customHeaders != null)
            {
                foreach (var header in customHeaders)
                {
                    if (httpRequest.Headers.Contains(header.Key))
                    {
                        httpRequest.Headers.Remove(header.Key);
                    }
                    httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            if (!string.IsNullOrEmpty(query))
            {
                requestContent = JsonConvert.SerializeObject(new { q = query, project = RasaProjectName });
                httpRequest.Content = new StringContent(requestContent, System.Text.Encoding.UTF8);
                httpRequest.Content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
            }

            // Set Credentials
            if (Client.Credentials != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Client.Credentials.ProcessHttpRequestAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            }

            // Send Request
            if (shouldTrace)
            {
                ServiceClientTracing.SendRequest(invocationId, httpRequest);
            }

            cancellationToken.ThrowIfCancellationRequested();
            httpResponse = await Client.HttpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);

            if (shouldTrace)
            {
                ServiceClientTracing.ReceiveResponse(invocationId, httpResponse);
            }

            HttpStatusCode statusCode = httpResponse.StatusCode;
            cancellationToken.ThrowIfCancellationRequested();

            if ((int)statusCode != 200)
            {
                var ex = new APIErrorException(string.Format("Operation returned an invalid status code '{0}'", statusCode));
                try
                {
                    responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    APIError errorBody = Microsoft.Rest.Serialization.SafeJsonConvert.DeserializeObject<APIError>(responseContent, Client.DeserializationSettings);
                    if (errorBody != null)
                    {
                        ex.Body = errorBody;
                    }
                }
                catch (JsonException)
                {
                    // Ignore the exception
                }

                ex.Request = new HttpRequestMessageWrapper(httpRequest, requestContent);
                ex.Response = new HttpResponseMessageWrapper(httpResponse, responseContent);

                if (shouldTrace)
                {
                    ServiceClientTracing.Error(invocationId, ex);
                }

                httpRequest.Dispose();

                if (httpResponse != null)
                {
                    httpResponse.Dispose();
                }
                throw ex;
            }

            // Create Result
            var result = new HttpOperationResponse<LuisResult>
            {
                Request = httpRequest,
                Response = httpResponse
            };

            // Deserialize Response
            if ((int)statusCode == 200)
            {
                responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    result.Body = Microsoft.Rest.Serialization.SafeJsonConvert.DeserializeObject<LuisResult>(responseContent, Client.DeserializationSettings);
                }
                catch (JsonException ex)
                {
                    httpRequest.Dispose();
                    if (httpResponse != null)
                    {
                        httpResponse.Dispose();
                    }
                    throw new SerializationException("Unable to deserialize the response.", responseContent, ex);
                }
            }

            if (shouldTrace)
            {
                ServiceClientTracing.Exit(invocationId, result);
            }

            return result;
        }

        #endregion

    }
}
