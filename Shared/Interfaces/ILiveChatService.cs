using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ben.Module.LiveChat.Services
{
    public interface ILiveChatService 
    {
        Task<List<Models.LiveChat>> GetLiveChatsAsync(int ModuleId);

        Task<Models.LiveChat> GetLiveChatAsync(int LiveChatId, int ModuleId);

        Task<Models.LiveChat> AddLiveChatAsync(Models.LiveChat LiveChat);

        Task<Models.LiveChat> UpdateLiveChatAsync(Models.LiveChat LiveChat);

        Task DeleteLiveChatAsync(int LiveChatId, int ModuleId);
    }
}
