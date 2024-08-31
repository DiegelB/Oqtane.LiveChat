using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using Oqtane.Repository.Databases.Interfaces;

namespace Ben.Module.LiveChat.Repository
{
    public class LiveChatContext : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<Models.LiveChat> LiveChat { get; set; }

        public LiveChatContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Models.LiveChat>().ToTable(ActiveDatabase.RewriteName("BenLiveChat"));
        }
    }
}
