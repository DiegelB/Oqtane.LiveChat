using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using Ben.Module.LiveChat.Repository;
using Ben.Module.LiveChat.Services;

namespace Ben.Module.LiveChat.Startup
{
    public class LiveChatServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // not implemented
        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // not implemented
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ILiveChatService, ServerLiveChatService>();
            services.AddDbContextFactory<LiveChatContext>(opt => { }, ServiceLifetime.Transient);
        }
    }
}
