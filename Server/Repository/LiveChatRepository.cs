using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;

namespace Ben.Module.LiveChat.Repository
{
    public class LiveChatRepository : ILiveChatRepository, ITransientService
    {
        private readonly IDbContextFactory<LiveChatContext> _factory;

        public LiveChatRepository(IDbContextFactory<LiveChatContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.LiveChat> GetLiveChats(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.LiveChat.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.LiveChat GetLiveChat(int LiveChatId)
        {
            return GetLiveChat(LiveChatId, true);
        }

        public Models.LiveChat GetLiveChat(int LiveChatId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.LiveChat.Find(LiveChatId);
            }
            else
            {
                return db.LiveChat.AsNoTracking().FirstOrDefault(item => item.LiveChatId == LiveChatId);
            }
        }

        public Models.LiveChat AddLiveChat(Models.LiveChat LiveChat)
        {
            using var db = _factory.CreateDbContext();
            db.LiveChat.Add(LiveChat);
            db.SaveChanges();
            return LiveChat;
        }

        public Models.LiveChat UpdateLiveChat(Models.LiveChat LiveChat)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(LiveChat).State = EntityState.Modified;
            db.SaveChanges();
            return LiveChat;
        }

        public void DeleteLiveChat(int LiveChatId)
        {
            using var db = _factory.CreateDbContext();
            Models.LiveChat LiveChat = db.LiveChat.Find(LiveChatId);
            db.LiveChat.Remove(LiveChat);
            db.SaveChanges();
        }
    }
}
