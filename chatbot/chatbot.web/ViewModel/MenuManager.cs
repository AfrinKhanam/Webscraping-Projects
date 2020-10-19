using System;
using System.Threading.Tasks;

using IndianBank_ChatBOT.Utils;

using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json.Linq;

namespace IndianBank_ChatBOT.ViewModel
{
    public class MenuManager
    {
        private readonly IMemoryCache memoryCache;
        private readonly string menuItemsUrl;

        public MenuManager(string _menuItemsUrl, IMemoryCache memoryCache)
        {
            menuItemsUrl = _menuItemsUrl;
            this.memoryCache = memoryCache;
        }

        public async Task<MenuViewModel[]> GetMenuItems()
        {
            return await memoryCache.GetOrCreateAsync<MenuViewModel[]>("MenuItems", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                return await GetMenuItemsFromES();
            });
        }

        private async Task<MenuViewModel[]> GetMenuItemsFromES()
        {
            var requestParameters = new
            {
                _source = true,
                size = 1,
                query = new
                {
                    ids = new
                    {
                        values = new[] { "menu_items" }
                    }
                }
            };

            var jsonResponse = await HttpRequestUtils.PostJsonBody(menuItemsUrl, requestParameters);

            var jobject = JObject.Parse(jsonResponse);

            return jobject["hits"]["hits"][0]["_source"]["ib_menu"].ToObject<MenuViewModel[]>();
        }
    }
}
