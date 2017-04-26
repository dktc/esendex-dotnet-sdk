using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using com.esendex.sdk.extensions;
using com.esendex.sdk.models.requests;
using com.esendex.sdk.surveys.models;
using Newtonsoft.Json;
using System.Net.Http;

namespace com.esendex.sdk.surveys
{
    /// <summary>
    /// A service to send surveys
    /// </summary>
    public class SurveySendService
    {
        private const string SURVEYS_BASE_URL = "https://surveys.api.esendex.com";

        private readonly Uri _baseUrl;
        private readonly EsendexCredentials _credentials;

        /// <summary>
        /// Initialises a new instance of the SurveySendService
        /// </summary>
        /// <param name="credentials">A com.esendex.sdk.EsendexCredentials instance containing your username and password.</param>
        public SurveySendService(EsendexCredentials credentials) : this(SURVEYS_BASE_URL, credentials) { }

        /// <summary>
        /// Initialises a new instance of the SurveySendService
        /// </summary>
        /// <param name="username">A string containing your username.</param>
        /// <param name="password">A string containing your password.</param>
        public SurveySendService(string username, string password) : this(SURVEYS_BASE_URL, new EsendexCredentials(username, password)) { }

        internal SurveySendService(string baseUrl, EsendexCredentials credentials)
        {
            _baseUrl = new Uri(baseUrl);
            _credentials = credentials;
        }

        /// <summary>
        /// Send survey to a recipient
        /// </summary>
        /// <param name="surveyId">A System.Guid that contains the Id of an active survey.</param>
        /// <param name="recipient">A string that contains the phone number of a recipient.</param>
        /// <param name="templateFields">An optional dictionary of mappings between template field names and recipient data.</param>
        /// <param name="metaData">An optional dictionary of additional data to be stored against the recipient.</param>
        /// <returns>A com.esendex.sdk.surveys.model.SurveysSendResult that contains any errors resulting from the send.</returns>
        /// <exception cref="System.Net.WebException"></exception>
        public SurveySendResult Send(Guid surveyId, string recipient, Dictionary<string, string> templateFields = null, Dictionary<string, string> metaData = null)
        {
            var requestData = new SurveysSendRequest
            {
                Recipients = new[]
                {
                    new SurveysRecipient
                    {
                        PhoneNumber = recipient,
                        TemplateFields = templateFields,
                        MetaData = metaData
                    }
                }
            };

            var requestUrl = new Uri(_baseUrl, string.Format("v1.0/surveys/{0}/send", surveyId));
            var request = Request.Create("POST", requestUrl)
                                 .WithHeader("Authorization", "Basic " + _credentials.EncodedValue())
                                 .WithAcceptHeader(Constants.JSON_MEDIA_TYPE)
                                 .If(_credentials.UseProxy, r => r.WithProxy(_credentials.WebProxy))
                                 .WriteBody(Constants.JSON_MEDIA_TYPE, streamWriter => JsonSerializer.Create().Serialize(streamWriter, requestData));

            HttpResponseMessage response;
            response = request.GetResponse();

            if (response.IsSuccessStatusCode)
            {
                return response.DeserialiseJson<SurveySendResult>();
            }
            else
            {
                throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}