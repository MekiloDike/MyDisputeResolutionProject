using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisputeResolutionInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "transactionLogReferenceProperty",
                table: "DisputeResponseLogs",
                newName: "transactionLogReference");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "transactionLogReference",
                table: "DisputeResponseLogs",
                newName: "transactionLogReferenceProperty");
        }
    }
}
