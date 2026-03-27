using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DarwinLingua.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPracticeSchedulingState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PracticeAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    WordEntryPublicId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SessionType = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Outcome = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    DueAtUtcBeforeAttempt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DueAtUtcAfterAttempt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AttemptedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ResponseMilliseconds = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeAttempts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PracticeReviewStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    WordEntryPublicId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DueAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastAttemptedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastSuccessfulAttemptedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastSessionType = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    LastOutcome = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    ConsecutiveSuccessCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ConsecutiveFailureCount = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalAttemptCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeReviewStates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeAttempts_User_AttemptedAtUtc",
                table: "PracticeAttempts",
                columns: new[] { "UserId", "AttemptedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeAttempts_User_Word_AttemptedAtUtc",
                table: "PracticeAttempts",
                columns: new[] { "UserId", "WordEntryPublicId", "AttemptedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeReviewStates_User_DueAtUtc",
                table: "PracticeReviewStates",
                columns: new[] { "UserId", "DueAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeReviewStates_UserId_WordEntryPublicId",
                table: "PracticeReviewStates",
                columns: new[] { "UserId", "WordEntryPublicId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PracticeAttempts");

            migrationBuilder.DropTable(
                name: "PracticeReviewStates");
        }
    }
}
