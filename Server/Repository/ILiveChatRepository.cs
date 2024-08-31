using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ben.Module.LiveChat.Repository
{
    public interface ILiveChatRepository
    {
        IEnumerable<Models.LiveChat> GetLiveChats(int ModuleId);
        Models.LiveChat GetLiveChat(int LiveChatId);
        Models.LiveChat GetLiveChat(int LiveChatId, bool tracking);
        Models.LiveChat AddLiveChat(Models.LiveChat LiveChat);
        Models.LiveChat UpdateLiveChat(Models.LiveChat LiveChat);
        void DeleteLiveChat(int LiveChatId);
    }
}
