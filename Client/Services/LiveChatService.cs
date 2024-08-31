using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;

namespace Ben.Module.LiveChat.Services
{
    public class LiveChatService : ServiceBase, ILiveChatService, IService
    {
        public LiveChatService(IHttpClientFactory http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("LiveChat");

        public async Task<List<Models.LiveChat>> GetLiveChatsAsync(int ModuleId)
        {
            List<Models.LiveChat> LiveChats = await GetJsonAsync<List<Models.LiveChat>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<Models.LiveChat>().ToList());
            return LiveChats.OrderBy(item => item.Name).ToList();
        }

        public async Task<Models.LiveChat> GetLiveChatAsync(int LiveChatId, int ModuleId)
        {
            return await GetJsonAsync<Models.LiveChat>(CreateAuthorizationPolicyUrl($"{Apiurl}/{LiveChatId}", EntityNames.Module, ModuleId));
        }

        public async Task<Models.LiveChat> AddLiveChatAsync(Models.LiveChat LiveChat)
        {
            return await PostJsonAsync<Models.LiveChat>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, LiveChat.ModuleId), LiveChat);
        }

        public async Task<Models.LiveChat> UpdateLiveChatAsync(Models.LiveChat LiveChat)
        {
            return await PutJsonAsync<Models.LiveChat>(CreateAuthorizationPolicyUrl($"{Apiurl}/{LiveChat.LiveChatId}", EntityNames.Module, LiveChat.ModuleId), LiveChat);
        }

        public async Task DeleteLiveChatAsync(int LiveChatId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{LiveChatId}", EntityNames.Module, ModuleId));
        }
    }
}
