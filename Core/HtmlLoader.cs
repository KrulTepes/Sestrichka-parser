using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Сестричка_парсер.Core
{
    class HtmlLoader
    {
        readonly HttpClient client;
        IParserSettings settings;
        public HtmlLoader(IParserSettings settings)
        {
            client = new HttpClient();
            this.settings = settings;
            
        }
        public async Task<string> GetSourceByPageId(int id)
        {
            string url = $"{settings.BaseUrl}/{settings.Prefix}";
            var currentUrl = url.Replace("{CurrentId}", id.ToString());
            var response = await client.GetAsync(currentUrl);
            string source = null;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                source = await response.Content.ReadAsStringAsync();
            }

            return source;
        }
    }
}
