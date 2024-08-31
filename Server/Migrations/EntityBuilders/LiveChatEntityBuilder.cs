using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace Ben.Module.LiveChat.Migrations.EntityBuilders
{
    public class LiveChatEntityBuilder : AuditableBaseEntityBuilder<LiveChatEntityBuilder>
    {
        private const string _entityTableName = "BenLiveChat";
        private readonly PrimaryKey<LiveChatEntityBuilder> _primaryKey = new("PK_BenLiveChat", x => x.LiveChatId);
        private readonly ForeignKey<LiveChatEntityBuilder> _moduleForeignKey = new("FK_BenLiveChat_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public LiveChatEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override LiveChatEntityBuilder BuildTable(ColumnsBuilder table)
        {
            LiveChatId = AddAutoIncrementColumn(table,"LiveChatId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            Name = AddMaxStringColumn(table,"Name");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> LiveChatId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> Name { get; set; }
    }
}
