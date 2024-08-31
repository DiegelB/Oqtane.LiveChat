using Oqtane.Models;
using Oqtane.Modules;

namespace Ben.Module.LiveChat
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "LiveChat",
            Description = "Live Chat service",
            Version = "1.0.0",
            ServerManagerType = "Ben.Module.LiveChat.Manager.LiveChatManager, Ben.Module.LiveChat.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = "Ben.Module.LiveChat.Shared.Oqtane",
            PackageName = "Ben.Module.LiveChat" 
        };
    }
}
