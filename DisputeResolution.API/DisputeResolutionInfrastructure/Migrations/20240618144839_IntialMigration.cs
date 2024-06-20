using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisputeResolutionInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IntialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisputeRequestLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaskCardPan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TerminalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionLogRefernce = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RetrivalNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisputeCreated = table.Column<bool>(type: "bit", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisputeRequestLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DisputeResponseLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    logCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issuerCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issuer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    acquirerCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    acquirer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    merchantCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    merchant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    customerReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transactionStore = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transactionReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    transactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transactionAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    surchargeAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transactionCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    settlementAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    settlementCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    terminalType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    disputeAmountType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    disputeAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reasonCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    merchantDisputant = table.Column<bool>(type: "bit", nullable: false),
                    domainCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    statusStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    accountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    statusActions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transactionLogReferenceProperty = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisputeResponseLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Additionalinfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardAcceptorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardAcceptorLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardScheme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasMerchant = table.Column<bool>(type: "bit", nullable: false),
                    MerchantType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetrievalReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Settled = table.Column<bool>(type: "bit", nullable: false),
                    SinkNodeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceNodeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TerminalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisputeResponseLogId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Additionalinfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Additionalinfo_DisputeResponseLogs_DisputeResponseLogId",
                        column: x => x.DisputeResponseLogId,
                        principalTable: "DisputeResponseLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evidence",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    disputeId = table.Column<long>(type: "bigint", nullable: false),
                    uuId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    base64EncodedBinary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisputeResponseLogId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidence", x => x.id);
                    table.ForeignKey(
                        name: "FK_Evidence_DisputeResponseLogs_DisputeResponseLogId",
                        column: x => x.DisputeResponseLogId,
                        principalTable: "DisputeResponseLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Journal",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    disputeId = table.Column<int>(type: "int", nullable: false),
                    detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    addedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    addedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisputeResponseLogId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journal", x => x.id);
                    table.ForeignKey(
                        name: "FK_Journal_DisputeResponseLogs_DisputeResponseLogId",
                        column: x => x.DisputeResponseLogId,
                        principalTable: "DisputeResponseLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Additionalinfo_DisputeResponseLogId",
                table: "Additionalinfo",
                column: "DisputeResponseLogId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evidence_DisputeResponseLogId",
                table: "Evidence",
                column: "DisputeResponseLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Journal_DisputeResponseLogId",
                table: "Journal",
                column: "DisputeResponseLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Additionalinfo");

            migrationBuilder.DropTable(
                name: "DisputeRequestLogs");

            migrationBuilder.DropTable(
                name: "Evidence");

            migrationBuilder.DropTable(
                name: "Journal");

            migrationBuilder.DropTable(
                name: "DisputeResponseLogs");
        }
    }
}
