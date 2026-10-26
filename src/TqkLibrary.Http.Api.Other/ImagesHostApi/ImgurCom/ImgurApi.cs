using TqkLibrary.Http.Api;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Http.Api.Other.ImagesHostApi.ImgurCom.DataClass;

namespace TqkLibrary.Http.Api.Other.ImagesHostApi.ImgurCom
{
    /// <summary>
    /// 
    /// </summary>
    public class ImgurApi : BaseApiKeyApi
    {
        private const string EndPoint = "https://api.imgur.com/3";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApiKey"></param>
        public ImgurApi(string ApiKey) : base(ApiKey)
        {
        }

        public ImgurApi(string ApiKey, HttpMessageHandler httpMessageHandler, bool disposeHandler = false) : base(ApiKey, httpMessageHandler, disposeHandler)
        {
        }

        public ImgurApi(string ApiKey, HttpClient httpClient) : base(ApiKey, httpClient)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<ImgurResponse<ImgurImage>> UploadImage(byte[] bitmap, CancellationToken cancellationToken = default)
        {
            MultipartFormDataContent requestContent = new MultipartFormDataContent();
            ByteArrayContent imageContent_instructions = new ByteArrayContent(bitmap);
            imageContent_instructions.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            requestContent.Add(imageContent_instructions, "image");
            requestContent.Add(new StringContent("file"), "type");
            return Build()
                .WithUrlPost(new UrlBuilder(EndPoint, "upload"), requestContent)
                .WithHeader("Authorization", $"Client-ID {ApiKey}")
                .ExecuteAsync<ImgurResponse<ImgurImage>>(cancellationToken);
        }
    }
}