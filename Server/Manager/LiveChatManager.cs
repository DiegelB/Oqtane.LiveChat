using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Interfaces;
using Oqtane.Enums;
using Oqtane.Repository;
using Ben.Module.LiveChat.Repository;
using System.Threading.Tasks;

namespace Ben.Module.LiveChat.Manager
{
    public class LiveChatManager : MigratableModuleBase, IInstallable, IPortable, ISearchable
    {
        private readonly ILiveChatRepository _LiveChatRepository;
        private readonly IDBContextDependencies _DBContextDependencies;

        public LiveChatManager(ILiveChatRepository LiveChatRepository, IDBContextDependencies DBContextDependencies)
        {
            _LiveChatRepository = LiveChatRepository;
            _DBContextDependencies = DBContextDependencies;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new LiveChatContext(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new LiveChatContext(_DBContextDependencies), tenant, MigrationType.Down);
        }

        public string ExportModule(Oqtane.Models.Module module)
        {
            string content = "";
            List<Models.LiveChat> LiveChats = _LiveChatRepository.GetLiveChats(module.ModuleId).ToList();
            if (LiveChats != null)
            {
                content = JsonSerializer.Serialize(LiveChats);
            }
            return content;
        }

        public void ImportModule(Oqtane.Models.Module module, string content, string version)
        {
            List<Models.LiveChat> LiveChats = null;
            if (!string.IsNullOrEmpty(content))
            {
                LiveChats = JsonSerializer.Deserialize<List<Models.LiveChat>>(content);
            }
            if (LiveChats != null)
            {
                foreach(var LiveChat in LiveChats)
                {
                    _LiveChatRepository.AddLiveChat(new Models.LiveChat { ModuleId = module.ModuleId, Name = LiveChat.Name });
                }
            }
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, DateTime lastIndexedOn)
        {
           var searchContentList = new List<SearchContent>();

           foreach (var LiveChat in _LiveChatRepository.GetLiveChats(pageModule.ModuleId))
           {
               if (LiveChat.ModifiedOn >= lastIndexedOn)
               {
                   searchContentList.Add(new SearchContent
                   {
                       EntityName = "BenLiveChat",
                       EntityId = LiveChat.LiveChatId.ToString(),
                       Title = LiveChat.Name,
                       Body = LiveChat.Name,
                       ContentModifiedBy = LiveChat.ModifiedBy,
                       ContentModifiedOn = LiveChat.ModifiedOn
                   });
               }
           }

           return Task.FromResult(searchContentList);
        }
    }
}
