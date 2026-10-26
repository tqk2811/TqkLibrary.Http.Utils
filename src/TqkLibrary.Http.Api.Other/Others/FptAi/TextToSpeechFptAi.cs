using TqkLibrary.Http.Api;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Http.Api.Other.Others.FptAi.DataClass;

namespace TqkLibrary.Http.Api.Other.Others.FptAi
{
    /// <summary>
    /// 
    /// </summary>
    public class TextToSpeechFptAi : BaseApiKeyApi
    {
        const string EndPoint = "https://api.fpt.ai/hmi/tts";
        readonly string version = "/v5";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApiKey"></param>
        public TextToSpeechFptAi(string ApiKey) : base(ApiKey)
        {

        }

        public TextToSpeechFptAi(string ApiKey, HttpMessageHandler httpMessageHandler, bool disposeHandler = false) : base(ApiKey, httpMessageHandler, disposeHandler)
        {

        }

        public TextToSpeechFptAi(string ApiKey, HttpClient httpClient) : base(ApiKey, httpClient)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<TTSResponse> TTS(string text, Voice voice = Voice.BanMai, Speed speed = Speed.Normal, Format format = Format.mp3, CancellationToken cancellationToken = default)
            => Build()
                .WithUrlPost(new UrlBuilder(EndPoint, version), new StringContent(text, Encoding.UTF8, "text/html"), true)
                .WithHeader("api-key", ApiKey)
                .WithHeader("speed", ((int)speed).ToString())
                .WithHeader("voice", voice.ToString().ToLower())
                .WithHeader("format", format.ToString().ToLower())
                .ExecuteAsync<TTSResponse>(cancellationToken);
    }
}
