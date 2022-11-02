using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helper
{
    public class HttpLibraryDAL
    {
        public HttpLibraryDAL()
        {
        }
        public HttpResponseMessage SentRequest(String JsonData, String URL, String Method, Boolean isAuthorize, String Token, out string ResultJson)
        {
            ResultJson = "";

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod(Method), URL))
                {
                    try
                    {
                        if (isAuthorize)
                        {
                            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + Token);

                        }
                        request.Content = new StringContent(JsonData);
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                        var response = httpClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
                        string result = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        ResultJson = result;
                        return response;
                    }
                    catch (Exception ex)
                    {
                        return null;
                        //throw ex;
                    }
                }
            }
        }
    }
}
