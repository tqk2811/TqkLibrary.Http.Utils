using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using TqkLibrary.Http.HttpClientHandles;
using TqkLibrary.Http.Json;

namespace TqkLibrary.Http.Api
{
    /// <summary>
    /// Base of the TqkLibrary.Http.Api.* clients: <see cref="BaseApi"/> plus an optional <see cref="ApiKey"/> and the
    /// JSON conventions these clients were written for (camelCase, null and empty strings skipped).
    /// Constructors without a handler use <see cref="SharedHttpClientHandler"/>; pass a
    /// <see cref="HttpMessageHandler"/> or <see cref="HttpClient"/> to use your own.
    /// </summary>
    public abstract class BaseApiKeyApi : BaseApi
    {
        static BaseApiKeyApi()
        {
            // UseCookies must be OFF: this ONE handler is shared by every client built without a handler, so a
            // cookie jar here would be shared by all users of the process. Wrap CookieHandler for per-client cookies.
            SharedHttpClientHandler = IsBrowserRuntime()
                ? new WrapperHttpClientHandler() { UseCookies = false }
                : new WrapperHttpClientHandler()
                {
                    UseCookies = false,
                    AllowAutoRedirect = true,
                    AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
                };
        }

        static bool IsBrowserRuntime()
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsBrowser();
#else
            return RuntimeInformation.ProcessArchitecture.ToString().Equals("Wasm", StringComparison.OrdinalIgnoreCase);
#endif
        }

        /// <summary>
        /// Handler shared by the whole process, the default of the constructors that take no handler.
        /// </summary>
        /// <remarks>
        /// Do not put anything per user or per session on it (<c>CookieContainer</c>, <c>Credentials</c>,
        /// <c>ClientCertificates</c>). It cannot be disposed (<see cref="WrapperHttpClientHandler"/> ignores Dispose).
        /// </remarks>
        public static WrapperHttpClientHandler SharedHttpClientHandler { get; }

        /// <summary>
        ///
        /// </summary>
        public string? ApiKey { get; }

        /// <summary>
        /// Uses <see cref="SharedHttpClientHandler"/>.
        /// </summary>
        protected BaseApiKeyApi() : this(SharedHttpClientHandler, false)
        {
        }
        /// <summary>
        /// Uses <see cref="SharedHttpClientHandler"/>.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected BaseApiKeyApi(string apiKey) : this(apiKey, SharedHttpClientHandler, false)
        {
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="httpMessageHandler"></param>
        /// <param name="disposeHandler"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected BaseApiKeyApi(string apiKey, HttpMessageHandler httpMessageHandler, bool disposeHandler = false)
            : base(httpMessageHandler, disposeHandler)
        {
            if (string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentNullException(nameof(apiKey));
            this.ApiKey = apiKey;
            InitJsonSettings();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="httpMessageHandler"></param>
        /// <param name="disposeHandler"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected BaseApiKeyApi(HttpMessageHandler httpMessageHandler, bool disposeHandler = false)
            : base(httpMessageHandler, disposeHandler)
        {
            InitJsonSettings();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="httpClient"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected BaseApiKeyApi(string apiKey, HttpClient httpClient) : base(httpClient)
        {
            this.ApiKey = apiKey;
            InitJsonSettings();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="httpClient"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected BaseApiKeyApi(HttpClient httpClient) : base(httpClient)
        {
            InitJsonSettings();
        }

        void InitJsonSettings()
        {
            DefaultJsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCaseIgnoreEmptyStringContractResolver(),
#if DEBUG
                MissingMemberHandling = MissingMemberHandling.Error,
#endif
            };
        }
    }
}
