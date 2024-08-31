using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Ben.Module.LiveChat.Repository;
using Oqtane.Controllers;
using System.Net;

namespace Ben.Module.LiveChat.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class LiveChatController : ModuleControllerBase
    {
        private readonly ILiveChatRepository _LiveChatRepository;

        public LiveChatController(ILiveChatRepository LiveChatRepository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _LiveChatRepository = LiveChatRepository;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<Models.LiveChat> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return _LiveChatRepository.GetLiveChats(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized LiveChat Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public Models.LiveChat Get(int id)
        {
            Models.LiveChat LiveChat = _LiveChatRepository.GetLiveChat(id);
            if (LiveChat != null && IsAuthorizedEntityId(EntityNames.Module, LiveChat.ModuleId))
            {
                return LiveChat;
            }
            else
            { 
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized LiveChat Get Attempt {LiveChatId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.LiveChat Post([FromBody] Models.LiveChat LiveChat)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, LiveChat.ModuleId))
            {
                LiveChat = _LiveChatRepository.AddLiveChat(LiveChat);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "LiveChat Added {LiveChat}", LiveChat);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized LiveChat Post Attempt {LiveChat}", LiveChat);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                LiveChat = null;
            }
            return LiveChat;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.LiveChat Put(int id, [FromBody] Models.LiveChat LiveChat)
        {
            if (ModelState.IsValid && LiveChat.LiveChatId == id && IsAuthorizedEntityId(EntityNames.Module, LiveChat.ModuleId) && _LiveChatRepository.GetLiveChat(LiveChat.LiveChatId, false) != null)
            {
                LiveChat = _LiveChatRepository.UpdateLiveChat(LiveChat);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "LiveChat Updated {LiveChat}", LiveChat);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized LiveChat Put Attempt {LiveChat}", LiveChat);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                LiveChat = null;
            }
            return LiveChat;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            Models.LiveChat LiveChat = _LiveChatRepository.GetLiveChat(id);
            if (LiveChat != null && IsAuthorizedEntityId(EntityNames.Module, LiveChat.ModuleId))
            {
                _LiveChatRepository.DeleteLiveChat(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "LiveChat Deleted {LiveChatId}", id);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized LiveChat Delete Attempt {LiveChatId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
