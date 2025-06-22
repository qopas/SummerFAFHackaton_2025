using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ediki.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDemoSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DemoSessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    HostUserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MaxParticipants = table.Column<int>(type: "integer", nullable: false),
                    IsRecording = table.Column<bool>(type: "boolean", nullable: false),
                    RecordingUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    StreamKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RoomId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Settings_AllowChat = table.Column<bool>(type: "boolean", nullable: false),
                    Settings_AllowQuestions = table.Column<bool>(type: "boolean", nullable: false),
                    Settings_AllowScreenAnnotation = table.Column<bool>(type: "boolean", nullable: false),
                    Settings_AutoRecord = table.Column<bool>(type: "boolean", nullable: false),
                    Settings_RequireApproval = table.Column<bool>(type: "boolean", nullable: false),
                    Settings_RecordingQuality = table.Column<int>(type: "integer", nullable: false),
                    Settings_AllowedDomains = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoSessions_AspNetUsers_HostUserId",
                        column: x => x.HostUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DemoActions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DemoSessionId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ActionType = table.Column<int>(type: "integer", nullable: false),
                    EntityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ActionData = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    TimestampInSession = table.Column<TimeSpan>(type: "interval", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoActions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DemoActions_DemoSessions_DemoSessionId",
                        column: x => x.DemoSessionId,
                        principalTable: "DemoSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemoMessages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DemoSessionId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsFromHost = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoMessages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DemoMessages_DemoSessions_DemoSessionId",
                        column: x => x.DemoSessionId,
                        principalTable: "DemoSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemoParticipants",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DemoSessionId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LeftAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HasVideo = table.Column<bool>(type: "boolean", nullable: false),
                    HasAudio = table.Column<bool>(type: "boolean", nullable: false),
                    CanInteract = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoParticipants_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DemoParticipants_DemoSessions_DemoSessionId",
                        column: x => x.DemoSessionId,
                        principalTable: "DemoSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemoRecordings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DemoSessionId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    StreamUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Quality = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Metadata = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoRecordings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoRecordings_DemoSessions_DemoSessionId",
                        column: x => x.DemoSessionId,
                        principalTable: "DemoSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemoActions_ActionType",
                table: "DemoActions",
                column: "ActionType");

            migrationBuilder.CreateIndex(
                name: "IX_DemoActions_CreatedAt",
                table: "DemoActions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DemoActions_DemoSessionId",
                table: "DemoActions",
                column: "DemoSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoActions_TimestampInSession",
                table: "DemoActions",
                column: "TimestampInSession");

            migrationBuilder.CreateIndex(
                name: "IX_DemoActions_UserId",
                table: "DemoActions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoMessages_DemoSessionId",
                table: "DemoMessages",
                column: "DemoSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoMessages_Timestamp",
                table: "DemoMessages",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_DemoMessages_Type",
                table: "DemoMessages",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_DemoMessages_UserId",
                table: "DemoMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoParticipants_DemoSessionId_UserId",
                table: "DemoParticipants",
                columns: new[] { "DemoSessionId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DemoParticipants_JoinedAt",
                table: "DemoParticipants",
                column: "JoinedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DemoParticipants_Status",
                table: "DemoParticipants",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DemoParticipants_UserId",
                table: "DemoParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoRecordings_DemoSessionId",
                table: "DemoRecordings",
                column: "DemoSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoRecordings_StartedAt",
                table: "DemoRecordings",
                column: "StartedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DemoRecordings_Status",
                table: "DemoRecordings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DemoSessions_CreatedAt",
                table: "DemoSessions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DemoSessions_HostUserId",
                table: "DemoSessions",
                column: "HostUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoSessions_ScheduledAt",
                table: "DemoSessions",
                column: "ScheduledAt");

            migrationBuilder.CreateIndex(
                name: "IX_DemoSessions_Status",
                table: "DemoSessions",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemoActions");

            migrationBuilder.DropTable(
                name: "DemoMessages");

            migrationBuilder.DropTable(
                name: "DemoParticipants");

            migrationBuilder.DropTable(
                name: "DemoRecordings");

            migrationBuilder.DropTable(
                name: "DemoSessions");
        }
    }
}
