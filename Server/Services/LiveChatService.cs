using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Security;
using Oqtane.Shared;
using Ben.Module.LiveChat.Repository;

namespace Ben.Module.LiveChat.Services
{
    public class ServerLiveChatService : ILiveChatService, ITransientService
    {
        private readonly ILiveChatRepository _LiveChatRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerLiveChatService(ILiveChatRepository LiveChatRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor)
        {
            _LiveChatRepository = LiveChatRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<Models.LiveChat>> GetLiveChatsAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_LiveChatRepository.GetLiveChats(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized LiveChat Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<Models.LiveChat> GetLiveChatAsync(int LiveChatId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_LiveChatRepository.GetLiveChat(LiveChatId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized LiveChat Get Attempt {LiveChatId} {ModuleId}", LiveChatId, ModuleId);
                return null;
            }
        }

        public Task<Models.LiveChat> AddLiveChatAsync(Models.LiveChat LiveChat)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, LiveChat.ModuleId, PermissionNames.Edit))
            {
                LiveChat = _LiveChatRepository.AddLiveChat(LiveChat);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "LiveChat Added {LiveChat}", LiveChat);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized LiveChat Add Attempt {LiveChat}", LiveChat);
                LiveChat = null;
            }
            return Task.FromResult(LiveChat);
        }

        public Task<Models.LiveChat> UpdateLiveChatAsync(Models.LiveChat LiveChat)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, LiveChat.ModuleId, PermissionNames.Edit))
            {
                LiveChat = _LiveChatRepository.UpdateLiveChat(LiveChat);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "LiveChat Updated {LiveChat}", LiveChat);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized LiveChat Update Attempt {LiveChat}", LiveChat);
                LiveChat = null;
            }
            return Task.FromResult(LiveChat);
        }

        public Task DeleteLiveChatAsync(int LiveChatId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _LiveChatRepository.DeleteLiveChat(LiveChatId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "LiveChat Deleted {LiveChatId}", LiveChatId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized LiveChat Delete Attempt {LiveChatId} {ModuleId}", LiveChatId, ModuleId);
            }
            return Task.CompletedTask;
        }
    }
}
