using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SupportDesk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Department = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    CustomerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CustomerEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Priority = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AssignedAgentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResolvedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Agents_AssignedAgentId",
                        column: x => x.AssignedAgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Body = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "Department", "Email", "FullName", "IsActive" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000001"), "Technical", "ermal@pecb.com", "Ermal Komoni", true },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), "Billing", "njomza@pecb.com", "Njomza PECB", true },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), "Technical", "pecb-test-user1@pecb.com", "PECB Test User 1", true },
                    { new Guid("a0000000-0000-0000-0000-000000000004"), "General", "pecb-test-user2@pecb.com", "PECB Test User 2", true },
                    { new Guid("a0000000-0000-0000-0000-000000000005"), "Technical", "pecb-test-user3@pecb.com", "PECB Test User 3", false }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "AssignedAgentId", "ClosedDate", "CreatedDate", "CustomerEmail", "CustomerName", "Description", "DueDate", "LastModifiedDate", "Priority", "Reference", "ResolvedDate", "Status", "Title" },
                values: new object[,]
                {
                    { new Guid("11111111-0000-0000-0000-000000000001"), null, null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 9, 3, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Low", "TCK-2026-0001", null, "New", "Sample ticket 1" },
                    { new Guid("11111111-0000-0000-0000-000000000005"), null, null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 8, 30, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Normal", "TCK-2026-0005", null, "New", "Sample ticket 5" },
                    { new Guid("11111111-0000-0000-0000-000000000009"), null, null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "High", "TCK-2026-0009", null, "New", "Sample ticket 9" },
                    { new Guid("11111111-0000-0000-0000-000000000013"), null, null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 8, 27, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Critical", "TCK-2026-0013", null, "New", "Sample ticket 13" },
                    { new Guid("11111111-0000-0000-0000-000000000017"), null, null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 9, 3, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Low", "TCK-2026-0017", null, "New", "Sample ticket 17" },
                    { new Guid("11111111-0000-0000-0000-000000000002"), new Guid("a0000000-0000-0000-0000-000000000002"), null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 9, 3, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Low", "TCK-2026-0002", null, "InProgress", "Sample ticket 2" },
                    { new Guid("11111111-0000-0000-0000-000000000003"), new Guid("a0000000-0000-0000-0000-000000000003"), null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 9, 3, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Low", "TCK-2026-0003", new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Utc), "Resolved", "Sample ticket 3" },
                    { new Guid("11111111-0000-0000-0000-000000000004"), new Guid("a0000000-0000-0000-0000-000000000004"), new DateTime(2026, 8, 29, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 9, 3, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Low", "TCK-2026-0004", new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Utc), "Closed", "Sample ticket 4" },
                    { new Guid("11111111-0000-0000-0000-000000000006"), new Guid("a0000000-0000-0000-0000-000000000002"), null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 8, 30, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Normal", "TCK-2026-0006", null, "InProgress", "Sample ticket 6" },
                    { new Guid("11111111-0000-0000-0000-000000000007"), new Guid("a0000000-0000-0000-0000-000000000003"), null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 8, 30, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Normal", "TCK-2026-0007", new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Utc), "Resolved", "Sample ticket 7" },
                    { new Guid("11111111-0000-0000-0000-000000000008"), new Guid("a0000000-0000-0000-0000-000000000004"), new DateTime(2026, 8, 29, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 8, 30, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Normal", "TCK-2026-0008", new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Utc), "Closed", "Sample ticket 8" },
                    { new Guid("11111111-0000-0000-0000-000000000010"), new Guid("a0000000-0000-0000-0000-000000000002"), null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "High", "TCK-2026-0010", null, "InProgress", "Sample ticket 10" },
                    { new Guid("11111111-0000-0000-0000-000000000011"), new Guid("a0000000-0000-0000-0000-000000000003"), null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "High", "TCK-2026-0011", new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Utc), "Resolved", "Sample ticket 11" },
                    { new Guid("11111111-0000-0000-0000-000000000012"), new Guid("a0000000-0000-0000-0000-000000000004"), new DateTime(2026, 8, 29, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "High", "TCK-2026-0012", new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Utc), "Closed", "Sample ticket 12" },
                    { new Guid("11111111-0000-0000-0000-000000000014"), new Guid("a0000000-0000-0000-0000-000000000002"), null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 8, 27, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Critical", "TCK-2026-0014", null, "InProgress", "Sample ticket 14" },
                    { new Guid("11111111-0000-0000-0000-000000000015"), new Guid("a0000000-0000-0000-0000-000000000003"), null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 8, 27, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Critical", "TCK-2026-0015", new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Utc), "Resolved", "Sample ticket 15" },
                    { new Guid("11111111-0000-0000-0000-000000000016"), new Guid("a0000000-0000-0000-0000-000000000004"), new DateTime(2026, 8, 29, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 8, 27, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Critical", "TCK-2026-0016", new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Utc), "Closed", "Sample ticket 16" },
                    { new Guid("11111111-0000-0000-0000-000000000018"), new Guid("a0000000-0000-0000-0000-000000000002"), null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 9, 3, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Low", "TCK-2026-0018", null, "InProgress", "Sample ticket 18" },
                    { new Guid("11111111-0000-0000-0000-000000000019"), new Guid("a0000000-0000-0000-0000-000000000003"), null, new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 9, 3, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Low", "TCK-2026-0019", new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Utc), "Resolved", "Sample ticket 19" },
                    { new Guid("11111111-0000-0000-0000-000000000020"), new Guid("a0000000-0000-0000-0000-000000000004"), new DateTime(2026, 8, 29, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "support@pecb.com", "PECB Customer", "Seeded ticket for demo purposes.", new DateTime(2026, 9, 3, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Low", "TCK-2026-0020", new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Utc), "Closed", "Sample ticket 20" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agents_Email",
                table: "Agents",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TicketId",
                table: "Comments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssignedAgentId",
                table: "Tickets",
                column: "AssignedAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_DueDate",
                table: "Tickets",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Priority",
                table: "Tickets",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Reference",
                table: "Tickets",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Status",
                table: "Tickets",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Agents");
        }
    }
}
