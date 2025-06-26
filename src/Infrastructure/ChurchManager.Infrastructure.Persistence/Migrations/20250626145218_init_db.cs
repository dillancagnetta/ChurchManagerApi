using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class init_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Finances");

            migrationBuilder.EnsureSchema(
                name: "ChangeRequests");

            migrationBuilder.EnsureSchema(
                name: "Churches");

            migrationBuilder.EnsureSchema(
                name: "Communications");

            migrationBuilder.EnsureSchema(
                name: "People");

            migrationBuilder.EnsureSchema(
                name: "Discipleship");

            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.EnsureSchema(
                name: "Events");

            migrationBuilder.EnsureSchema(
                name: "Groups");

            migrationBuilder.EnsureSchema(
                name: "Common");

            migrationBuilder.EnsureSchema(
                name: "Missions");

            migrationBuilder.EnsureSchema(
                name: "Jobs");

            migrationBuilder.CreateTable(
                name: "BankStatementImport",
                schema: "Finances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    BankAccount = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ImportDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    TransactionCount = table.Column<int>(type: "integer", nullable: false),
                    ProcessedCount = table.Column<int>(type: "integer", nullable: false),
                    UnmatchedCount = table.Column<int>(type: "integer", nullable: false),
                    ErrorCount = table.Column<int>(type: "integer", nullable: false),
                    Errors = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatementStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StatementEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankStatementImport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChurchAttendanceType",
                schema: "Churches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchAttendanceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationPreferenceType",
                schema: "Communications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultNotSetValue = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultCommunicationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CanOverride = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationPreferenceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationTemplate",
                schema: "Communications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    LogoFileUrl = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    SupportedTypes = table.Column<string>(type: "text", nullable: false),
                    IsBaseTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConnectionStatusType",
                schema: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionStatusType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscipleshipProgram",
                schema: "Discipleship",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscipleshipProgram", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityPermission",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    EntityType = table.Column<string>(type: "text", nullable: true),
                    EntityIds = table.Column<int[]>(type: "integer[]", nullable: false),
                    IsDynamicScope = table.Column<bool>(type: "boolean", nullable: false),
                    ScopeType = table.Column<string>(type: "text", nullable: true),
                    ScopeId = table.Column<int>(type: "integer", nullable: true),
                    CanView = table.Column<bool>(type: "boolean", nullable: false),
                    CanEdit = table.Column<bool>(type: "boolean", nullable: false),
                    CanDelete = table.Column<bool>(type: "boolean", nullable: false),
                    CanManageUsers = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityPermission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Family",
                schema: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Address_Street = table.Column<string>(type: "text", nullable: true),
                    Address_City = table.Column<string>(type: "text", nullable: true),
                    Address_Country = table.Column<string>(type: "text", nullable: true),
                    Address_Province = table.Column<string>(type: "text", nullable: true),
                    Address_PostalCode = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Family", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fund",
                schema: "Finances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FundType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    ParentFundId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fund", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fund_Fund_ParentFundId",
                        column: x => x.ParentFundId,
                        principalSchema: "Finances",
                        principalTable: "Fund",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupFeature",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupFeature", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupType",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    GroupTerm = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    GroupMemberTerm = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TakesAttendance = table.Column<bool>(type: "boolean", nullable: false),
                    SendAttendanceReminder = table.Column<bool>(type: "boolean", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    ShowInNavigation = table.Column<bool>(type: "boolean", nullable: false),
                    IconCssClass = table.Column<string>(type: "text", nullable: false),
                    AllowAnyChildGroupType = table.Column<bool>(type: "boolean", nullable: false),
                    AllowedChildGroupTypesIds = table.Column<int[]>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "History",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    Category = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    EntityType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EntityId = table.Column<int>(type: "integer", nullable: false),
                    Verb = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Caption = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RelatedEntityType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RelatedEntityId = table.Column<int>(type: "integer", nullable: true),
                    RelatedData = table.Column<string>(type: "text", nullable: true),
                    ChangeType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ValueName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    NewRawValue = table.Column<string>(type: "text", nullable: true),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    OldRawValue = table.Column<string>(type: "text", nullable: true),
                    IsSensitive = table.Column<bool>(type: "boolean", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NoteType",
                schema: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CssClass = table.Column<string>(type: "text", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "Date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "Date", nullable: true),
                    iCalendarContent = table.Column<string>(type: "text", nullable: true),
                    WeeklyDayOfWeek = table.Column<int>(type: "integer", nullable: true),
                    WeeklyTimeOfDay = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Frequency = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Timezone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceJob",
                schema: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobKey = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Assembly = table.Column<string>(type: "character varying(260)", maxLength: 260, nullable: true),
                    Class = table.Column<string>(type: "character varying(260)", maxLength: 260, nullable: false),
                    CronExpression = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    LastSuccessfulRunDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastRunDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastRunDurationSeconds = table.Column<int>(type: "integer", nullable: true),
                    LastStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastStatusMessage = table.Column<string>(type: "text", nullable: true),
                    JobParameters = table.Column<string>(type: "jsonb", nullable: true),
                    NotificationEmails = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    NotificationStatus = table.Column<int>(type: "integer", nullable: false),
                    EnableHistory = table.Column<bool>(type: "boolean", nullable: false),
                    HistoryCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceJob", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemCommunication",
                schema: "Communications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemCommunication", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginRole",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscipleshipStepDefinition",
                schema: "Discipleship",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscipleshipProgramId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IconCssClass = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AllowMultiple = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscipleshipStepDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscipleshipStepDefinition_DiscipleshipProgram_Discipleship~",
                        column: x => x.DiscipleshipProgramId,
                        principalSchema: "Discipleship",
                        principalTable: "DiscipleshipProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventType",
                schema: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RequiresRegistration = table.Column<bool>(type: "boolean", nullable: false),
                    AllowFamilyRegistration = table.Column<bool>(type: "boolean", nullable: false),
                    AllowNonFamilyRegistration = table.Column<bool>(type: "boolean", nullable: false),
                    TakesAttendance = table.Column<bool>(type: "boolean", nullable: false),
                    RequiresChildInfo = table.Column<bool>(type: "boolean", nullable: false),
                    OnlineSupport = table.Column<string>(type: "text", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    IconCssClass = table.Column<string>(type: "text", nullable: true),
                    ChildCare_HasChildCare = table.Column<bool>(type: "boolean", nullable: true),
                    ChildCare_MinChildAge = table.Column<int>(type: "integer", nullable: true),
                    ChildCare_MaxChildAge = table.Column<int>(type: "integer", nullable: true),
                    AgeClassification = table.Column<string>(type: "text", nullable: true),
                    DefaultGroupTypeId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventType_GroupType_DefaultGroupTypeId",
                        column: x => x.DefaultGroupTypeId,
                        principalSchema: "Groups",
                        principalTable: "GroupType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupRole",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsLeader = table.Column<bool>(type: "boolean", nullable: false),
                    CanView = table.Column<bool>(type: "boolean", nullable: false),
                    CanEdit = table.Column<bool>(type: "boolean", nullable: false),
                    CanManageMembers = table.Column<bool>(type: "boolean", nullable: false),
                    GroupTypeId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupRole_GroupType_GroupTypeId",
                        column: x => x.GroupTypeId,
                        principalSchema: "Groups",
                        principalTable: "GroupType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceJobHistory",
                schema: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceJobId = table.Column<int>(type: "integer", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    StopDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StatusMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceJobHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceJobHistory_ServiceJob_ServiceJobId",
                        column: x => x.ServiceJobId,
                        principalSchema: "Jobs",
                        principalTable: "ServiceJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissionAssignment",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    EntityPermissionId = table.Column<int>(type: "integer", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissionAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissionAssignment_EntityPermission_EntityPermissionId",
                        column: x => x.EntityPermissionId,
                        principalSchema: "Auth",
                        principalTable: "EntityPermission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissionAssignment_UserLoginRole_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "UserLoginRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Benefactor",
                schema: "Finances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TaxId = table.Column<string>(type: "text", nullable: true),
                    PersonId = table.Column<int>(type: "integer", nullable: true),
                    FamilyId = table.Column<int>(type: "integer", nullable: true),
                    GroupId = table.Column<int>(type: "integer", nullable: true),
                    ChurchId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benefactor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Benefactor_Family_FamilyId",
                        column: x => x.FamilyId,
                        principalSchema: "People",
                        principalTable: "Family",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Giving",
                schema: "Finances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BenefactorId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Amount_Currency = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    Amount_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    PaymentMethod = table.Column<string>(type: "text", nullable: false),
                    GivingType = table.Column<string>(type: "text", nullable: false),
                    FundId = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ExternalReferenceId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ReceiptSent = table.Column<bool>(type: "boolean", nullable: false),
                    ParsedReference = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BankTransactionId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BankStatementImportId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Giving", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Giving_BankStatementImport_BankStatementImportId",
                        column: x => x.BankStatementImportId,
                        principalSchema: "Finances",
                        principalTable: "BankStatementImport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Giving_Benefactor_BenefactorId",
                        column: x => x.BenefactorId,
                        principalSchema: "Finances",
                        principalTable: "Benefactor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Giving_Fund_FundId",
                        column: x => x.FundId,
                        principalSchema: "Finances",
                        principalTable: "Fund",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportedTransaction",
                schema: "Finances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImportId = table.Column<int>(type: "integer", nullable: false),
                    OriginalReference = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Amount_Currency = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    Amount_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    BankTransactionId = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TransactionType = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    IsMatched = table.Column<bool>(type: "boolean", nullable: false),
                    GivingId = table.Column<int>(type: "integer", nullable: true),
                    ParsedReference = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsResolved = table.Column<bool>(type: "boolean", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Memo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Error = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    BankStatementImportId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportedTransaction_BankStatementImport_BankStatementImport~",
                        column: x => x.BankStatementImportId,
                        principalSchema: "Finances",
                        principalTable: "BankStatementImport",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImportedTransaction_BankStatementImport_ImportId",
                        column: x => x.ImportId,
                        principalSchema: "Finances",
                        principalTable: "BankStatementImport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportedTransaction_Giving_GivingId",
                        column: x => x.GivingId,
                        principalSchema: "Finances",
                        principalTable: "Giving",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequest",
                schema: "ChangeRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequestedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    Source = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ChurchId = table.Column<int>(type: "integer", nullable: true),
                    ReviewedByPersonId = table.Column<int>(type: "integer", nullable: true),
                    ReviewedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReviewNotes = table.Column<string>(type: "text", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyChangeRequest",
                schema: "ChangeRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChangeRequestId = table.Column<int>(type: "integer", nullable: false),
                    EntityType = table.Column<string>(type: "text", nullable: false),
                    EntityId = table.Column<int>(type: "integer", nullable: false),
                    PropertyPath = table.Column<string>(type: "text", nullable: false),
                    CurrentValue = table.Column<string>(type: "text", nullable: true),
                    RequestedValue = table.Column<string>(type: "text", nullable: true),
                    PropertyType = table.Column<string>(type: "text", nullable: true),
                    IsApplied = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyChangeRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyChangeRequest_ChangeRequest_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalSchema: "ChangeRequests",
                        principalTable: "ChangeRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Church",
                schema: "Churches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChurchGroupId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ShortCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LeaderPersonId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Church", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChurchAttendance",
                schema: "Churches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChurchAttendanceTypeId = table.Column<int>(type: "integer", nullable: false),
                    ChurchId = table.Column<int>(type: "integer", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DidNotOccur = table.Column<bool>(type: "boolean", nullable: true),
                    AttendanceCount = table.Column<int>(type: "integer", nullable: true),
                    MalesCount = table.Column<int>(type: "integer", nullable: true),
                    FemalesCount = table.Column<int>(type: "integer", nullable: true),
                    ChildrenCount = table.Column<int>(type: "integer", nullable: true),
                    TeensCount = table.Column<int>(type: "integer", nullable: true),
                    FirstTimerCount = table.Column<int>(type: "integer", nullable: true),
                    NewConvertCount = table.Column<int>(type: "integer", nullable: true),
                    ReceivedHolySpiritCount = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    PhotoUrls = table.Column<List<string>>(type: "text[]", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchAttendance_ChurchAttendanceType_ChurchAttendanceTypeId",
                        column: x => x.ChurchAttendanceTypeId,
                        principalSchema: "Churches",
                        principalTable: "ChurchAttendanceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChurchAttendance_Church_ChurchId",
                        column: x => x.ChurchId,
                        principalSchema: "Churches",
                        principalTable: "Church",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChurchServiceTime",
                schema: "Churches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChurchId = table.Column<int>(type: "integer", nullable: false),
                    ChurchAttendanceTypeId = table.Column<int>(type: "integer", nullable: false),
                    DayOfWeek = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchServiceTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchServiceTime_ChurchAttendanceType_ChurchAttendanceType~",
                        column: x => x.ChurchAttendanceTypeId,
                        principalSchema: "Churches",
                        principalTable: "ChurchAttendanceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChurchServiceTime_Church_ChurchId",
                        column: x => x.ChurchId,
                        principalSchema: "Churches",
                        principalTable: "Church",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentGroupId = table.Column<int>(type: "integer", nullable: true),
                    GroupTypeId = table.Column<int>(type: "integer", nullable: false),
                    ChurchId = table.Column<int>(type: "integer", nullable: true),
                    ScheduleId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    GroupCapacity = table.Column<int>(type: "integer", nullable: true),
                    IsOnline = table.Column<bool>(type: "boolean", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Group_Church_ChurchId",
                        column: x => x.ChurchId,
                        principalSchema: "Churches",
                        principalTable: "Church",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Group_GroupType_GroupTypeId",
                        column: x => x.GroupTypeId,
                        principalSchema: "Groups",
                        principalTable: "GroupType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Group_Group_ParentGroupId",
                        column: x => x.ParentGroupId,
                        principalSchema: "Groups",
                        principalTable: "Group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Group_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalSchema: "Common",
                        principalTable: "Schedule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName_Title = table.Column<string>(type: "text", nullable: true),
                    FullName_FirstName = table.Column<string>(type: "text", nullable: true),
                    FullName_NickName = table.Column<string>(type: "text", nullable: true),
                    FullName_MiddleName = table.Column<string>(type: "text", nullable: true),
                    FullName_LastName = table.Column<string>(type: "text", nullable: true),
                    FullName_Suffix = table.Column<string>(type: "text", nullable: true),
                    ConnectionStatus = table.Column<string>(type: "text", nullable: false),
                    DeceasedStatus_IsDeceased = table.Column<bool>(type: "boolean", nullable: true),
                    DeceasedStatus_DeceasedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AgeClassification = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    BirthDate_BirthDay = table.Column<int>(type: "integer", nullable: true),
                    BirthDate_BirthMonth = table.Column<int>(type: "integer", nullable: true),
                    BirthDate_BirthYear = table.Column<int>(type: "integer", nullable: true),
                    Source = table.Column<string>(type: "text", nullable: true),
                    FirstVisitDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    BaptismStatus_IsBaptised = table.Column<bool>(type: "boolean", nullable: true),
                    BaptismStatus_BaptismDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MaritalStatus = table.Column<string>(type: "text", nullable: true),
                    AnniversaryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Email_Address = table.Column<string>(type: "text", nullable: true),
                    Email_IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CommunicationPreference = table.Column<string>(type: "text", nullable: true),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    Occupation = table.Column<string>(type: "text", nullable: true),
                    FamilyId = table.Column<int>(type: "integer", nullable: false),
                    ReceivedHolySpirit = table.Column<bool>(type: "boolean", nullable: true),
                    GivingGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChurchId = table.Column<int>(type: "integer", nullable: true),
                    UserLoginId = table.Column<string>(type: "text", nullable: true),
                    ViewedCount = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Person_Church_ChurchId",
                        column: x => x.ChurchId,
                        principalSchema: "Churches",
                        principalTable: "Church",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Person_Family_FamilyId",
                        column: x => x.FamilyId,
                        principalSchema: "People",
                        principalTable: "Family",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupAttendance",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DidNotOccur = table.Column<bool>(type: "boolean", nullable: true),
                    AttendanceCount = table.Column<int>(type: "integer", nullable: true),
                    FirstTimerCount = table.Column<int>(type: "integer", nullable: true),
                    NewConvertCount = table.Column<int>(type: "integer", nullable: true),
                    ReceivedHolySpiritCount = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Offering_Currency = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    Offering_Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    PhotoUrls = table.Column<List<string>>(type: "text[]", nullable: false),
                    AttendanceReview_IsReviewed = table.Column<bool>(type: "boolean", nullable: true),
                    AttendanceReview_Feedback = table.Column<string>(type: "text", nullable: true),
                    AttendanceReview_ReviewedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupAttendance_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Groups",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupsFeatures",
                schema: "Groups",
                columns: table => new
                {
                    FeaturesId = table.Column<int>(type: "integer", nullable: false),
                    GroupsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsFeatures", x => new { x.FeaturesId, x.GroupsId });
                    table.ForeignKey(
                        name: "FK_GroupsFeatures_GroupFeature_FeaturesId",
                        column: x => x.FeaturesId,
                        principalSchema: "Groups",
                        principalTable: "GroupFeature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupsFeatures_Group_GroupsId",
                        column: x => x.GroupsId,
                        principalSchema: "Groups",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChurchGroup",
                schema: "Churches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LeaderPersonId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchGroup_Person_LeaderPersonId",
                        column: x => x.LeaderPersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Communication",
                schema: "Communications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Subject = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CommunicationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ListGroupId = table.Column<int>(type: "integer", nullable: true),
                    CommunicationTemplateId = table.Column<int>(type: "integer", nullable: true),
                    CommunicationContent = table.Column<string>(type: "text", nullable: true),
                    SenderPersonId = table.Column<int>(type: "integer", nullable: true),
                    IsBulkCommunication = table.Column<bool>(type: "boolean", nullable: false),
                    SendDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    FutureSendDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Review_ReviewerNote = table.Column<string>(type: "text", nullable: true),
                    Review_ReviewedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Review_ReviewerPersonId = table.Column<int>(type: "integer", nullable: true),
                    Metadata = table.Column<string>(type: "jsonb", nullable: true),
                    SystemCommunicationId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communication_CommunicationTemplate_CommunicationTemplateId",
                        column: x => x.CommunicationTemplateId,
                        principalSchema: "Communications",
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Communication_Group_ListGroupId",
                        column: x => x.ListGroupId,
                        principalSchema: "Groups",
                        principalTable: "Group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Communication_Person_SenderPersonId",
                        column: x => x.SenderPersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Communication_SystemCommunication_SystemCommunicationId",
                        column: x => x.SystemCommunicationId,
                        principalSchema: "Communications",
                        principalTable: "SystemCommunication",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommunicationPreference",
                schema: "Communications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    PreferenceTypeId = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    CommunicationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationPreference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationPreference_CommunicationPreferenceType_Prefere~",
                        column: x => x.PreferenceTypeId,
                        principalSchema: "Communications",
                        principalTable: "CommunicationPreferenceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommunicationPreference_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConnectionStatusHistory",
                schema: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    ConnectionStatusTypeId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectionStatusHistory_ConnectionStatusType_ConnectionStat~",
                        column: x => x.ConnectionStatusTypeId,
                        principalSchema: "People",
                        principalTable: "ConnectionStatusType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConnectionStatusHistory_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscipleshipStep",
                schema: "Discipleship",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscipleshipStepDefinitionId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EndDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Note = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscipleshipStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscipleshipStep_DiscipleshipStepDefinition_DiscipleshipSte~",
                        column: x => x.DiscipleshipStepDefinitionId,
                        principalSchema: "Discipleship",
                        principalTable: "DiscipleshipStepDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscipleshipStep_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowUp",
                schema: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssignedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    AssignedPersonId = table.Column<int>(type: "integer", nullable: true),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Severity = table.Column<string>(type: "text", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    RequiresAdditionalFollowUp = table.Column<bool>(type: "boolean", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUp_Person_AssignedPersonId",
                        column: x => x.AssignedPersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowUp_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMember",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    GroupRoleId = table.Column<int>(type: "integer", nullable: false),
                    FirstVisitDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ArchiveStatus_IsArchived = table.Column<bool>(type: "boolean", nullable: true),
                    ArchiveStatus_ArchivedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CommunicationPreference = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMember_GroupRole_GroupRoleId",
                        column: x => x.GroupRoleId,
                        principalSchema: "Groups",
                        principalTable: "GroupRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMember_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Groups",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMember_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mission",
                schema: "Missions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Category = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Stream = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IconCssClass = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EndDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PersonId = table.Column<int>(type: "integer", nullable: true),
                    ChurchId = table.Column<int>(type: "integer", nullable: true),
                    GroupId = table.Column<int>(type: "integer", nullable: true),
                    Attendance_AttendanceCount = table.Column<int>(type: "integer", nullable: true),
                    Attendance_FirstTimerCount = table.Column<int>(type: "integer", nullable: true),
                    Attendance_NewConvertCount = table.Column<int>(type: "integer", nullable: true),
                    Attendance_ReceivedHolySpiritCount = table.Column<int>(type: "integer", nullable: true),
                    Offering_Currency = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    Offering_Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    PhotoUrls = table.Column<List<string>>(type: "text[]", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mission_Church_ChurchId",
                        column: x => x.ChurchId,
                        principalSchema: "Churches",
                        principalTable: "Church",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mission_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Groups",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mission_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                schema: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NoteTypeId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Caption = table.Column<string>(type: "text", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: true),
                    PersonId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Note_NoteType_NoteTypeId",
                        column: x => x.NoteTypeId,
                        principalSchema: "People",
                        principalTable: "NoteType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Note_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OnlineUser",
                schema: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    ConnectionId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LastOnlineDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlineUser_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumber",
                schema: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Extension = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsMessagingEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsUnlisted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhoneNumber_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PushDevice",
                schema: "Communications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Endpoint = table.Column<string>(type: "text", nullable: false),
                    P256DH = table.Column<string>(type: "text", nullable: false),
                    Auth = table.Column<string>(type: "text", nullable: false),
                    UniqueIdentification = table.Column<string>(type: "text", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushDevice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PushDevice_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Tenant = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogin_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                schema: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    EventTypeId = table.Column<int>(type: "integer", nullable: false),
                    ChurchId = table.Column<int>(type: "integer", nullable: true),
                    ChurchGroupId = table.Column<int>(type: "integer", nullable: true),
                    ChildCareGroupId = table.Column<int>(type: "integer", nullable: true),
                    EventRegistrationGroupId = table.Column<int>(type: "integer", nullable: true),
                    ContactPersonId = table.Column<int>(type: "integer", nullable: false),
                    ContactEmail = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: true),
                    ContactPhone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Review_ReviewerNote = table.Column<string>(type: "text", nullable: true),
                    Review_ReviewedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Review_ReviewerPersonId = table.Column<int>(type: "integer", nullable: true),
                    RegistrationDates_StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RegistrationDates_EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: true),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_ChurchGroup_ChurchGroupId",
                        column: x => x.ChurchGroupId,
                        principalSchema: "Churches",
                        principalTable: "ChurchGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_Church_ChurchId",
                        column: x => x.ChurchId,
                        principalSchema: "Churches",
                        principalTable: "Church",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_EventType_EventTypeId",
                        column: x => x.EventTypeId,
                        principalSchema: "Events",
                        principalTable: "EventType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_Group_ChildCareGroupId",
                        column: x => x.ChildCareGroupId,
                        principalSchema: "Groups",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Event_Group_EventRegistrationGroupId",
                        column: x => x.EventRegistrationGroupId,
                        principalSchema: "Groups",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Event_Person_ContactPersonId",
                        column: x => x.ContactPersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationAttachment",
                schema: "Communications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommunicationId = table.Column<int>(type: "integer", nullable: false),
                    CommunicationType = table.Column<string>(type: "text", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    MimeType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    FileUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FileContents = table.Column<string>(type: "text", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationAttachment_Communication_CommunicationId",
                        column: x => x.CommunicationId,
                        principalSchema: "Communications",
                        principalTable: "Communication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationRecipient",
                schema: "Communications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommunicationId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    StatusNote = table.Column<string>(type: "text", nullable: true),
                    SendDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    OpenedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UniqueMessageId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AttemptCount = table.Column<int>(type: "integer", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationRecipient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationRecipient_Communication_CommunicationId",
                        column: x => x.CommunicationId,
                        principalSchema: "Communications",
                        principalTable: "Communication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunicationRecipient_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupMemberAttendance",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupMemberId = table.Column<int>(type: "integer", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DidAttend = table.Column<bool>(type: "boolean", nullable: true),
                    IsFirstTime = table.Column<bool>(type: "boolean", nullable: true),
                    IsNewConvert = table.Column<bool>(type: "boolean", nullable: true),
                    ReceivedHolySpirit = table.Column<bool>(type: "boolean", nullable: true),
                    Note = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    GroupAttendanceId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMemberAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMemberAttendance_GroupAttendance_GroupAttendanceId",
                        column: x => x.GroupAttendanceId,
                        principalSchema: "Groups",
                        principalTable: "GroupAttendance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMemberAttendance_GroupMember_GroupMemberId",
                        column: x => x.GroupMemberId,
                        principalSchema: "Groups",
                        principalTable: "GroupMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMemberAttendance_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Groups",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                schema: "Communications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    SentDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IconCssClass = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ImagePath = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Classification = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Link = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    UseRouter = table.Column<bool>(type: "boolean", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    SendWebPush = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    LastError = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_UserLogin_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "UserLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleAssignment",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserLoginId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserLoginRoleId = table.Column<int>(type: "integer", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoleAssignment_UserLoginRole_UserLoginRoleId",
                        column: x => x.UserLoginRoleId,
                        principalSchema: "Auth",
                        principalTable: "UserLoginRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleAssignment_UserLogin_UserLoginId",
                        column: x => x.UserLoginId,
                        principalSchema: "Auth",
                        principalTable: "UserLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventRegistration",
                schema: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RegisteredForAllSessions = table.Column<bool>(type: "boolean", nullable: false),
                    RequiresChildCare = table.Column<bool>(type: "boolean", nullable: false),
                    NumberOfChildren = table.Column<int>(type: "integer", nullable: true),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    RegisteredByPersonId = table.Column<int>(type: "integer", nullable: true),
                    GroupId = table.Column<int>(type: "integer", nullable: true),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRegistration_Event_EventId",
                        column: x => x.EventId,
                        principalSchema: "Events",
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventRegistration_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Groups",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventRegistration_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventRegistration_Person_RegisteredByPersonId",
                        column: x => x.RegisteredByPersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EventSession",
                schema: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Capacity = table.Column<int>(type: "integer", nullable: true),
                    AttendanceRequired = table.Column<bool>(type: "boolean", nullable: false),
                    SessionOrder = table.Column<int>(type: "integer", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    ScheduleId = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OnlineSupport = table.Column<string>(type: "text", nullable: false),
                    OnlineMeetingUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsCancelled = table.Column<bool>(type: "boolean", nullable: false),
                    CancellationReason = table.Column<string>(type: "text", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventSession_Event_EventId",
                        column: x => x.EventId,
                        principalSchema: "Events",
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSession_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalSchema: "Common",
                        principalTable: "Schedule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EventSessionRegistration",
                schema: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RegisteredDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EventRegistrationId = table.Column<int>(type: "integer", nullable: false),
                    EventSessionId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    AttendingOnline = table.Column<bool>(type: "boolean", nullable: true),
                    AttendingInPerson = table.Column<bool>(type: "boolean", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSessionRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventSessionRegistration_EventRegistration_EventRegistratio~",
                        column: x => x.EventRegistrationId,
                        principalSchema: "Events",
                        principalTable: "EventRegistration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSessionRegistration_EventSession_EventSessionId",
                        column: x => x.EventSessionId,
                        principalSchema: "Events",
                        principalTable: "EventSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventSessionRegistration_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankStatementImport_BankAccount",
                schema: "Finances",
                table: "BankStatementImport",
                column: "BankAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Benefactor_ChurchId",
                schema: "Finances",
                table: "Benefactor",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Benefactor_FamilyId",
                schema: "Finances",
                table: "Benefactor",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Benefactor_GroupId",
                schema: "Finances",
                table: "Benefactor",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Benefactor_PersonId",
                schema: "Finances",
                table: "Benefactor",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Benefactor_Type",
                schema: "Finances",
                table: "Benefactor",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequest_ChurchId",
                schema: "ChangeRequests",
                table: "ChangeRequest",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequest_ReviewedByPersonId",
                schema: "ChangeRequests",
                table: "ChangeRequest",
                column: "ReviewedByPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Church_ChurchGroupId",
                schema: "Churches",
                table: "Church",
                column: "ChurchGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Church_LeaderPersonId",
                schema: "Churches",
                table: "Church",
                column: "LeaderPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchAttendance_ChurchAttendanceTypeId",
                schema: "Churches",
                table: "ChurchAttendance",
                column: "ChurchAttendanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchAttendance_ChurchId",
                schema: "Churches",
                table: "ChurchAttendance",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchGroup_LeaderPersonId",
                schema: "Churches",
                table: "ChurchGroup",
                column: "LeaderPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchServiceTime_ChurchAttendanceTypeId",
                schema: "Churches",
                table: "ChurchServiceTime",
                column: "ChurchAttendanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchServiceTime_ChurchId",
                schema: "Churches",
                table: "ChurchServiceTime",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_CommunicationTemplateId",
                schema: "Communications",
                table: "Communication",
                column: "CommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_ListGroupId",
                schema: "Communications",
                table: "Communication",
                column: "ListGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_SenderPersonId",
                schema: "Communications",
                table: "Communication",
                column: "SenderPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_SystemCommunicationId",
                schema: "Communications",
                table: "Communication",
                column: "SystemCommunicationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationAttachment_CommunicationId",
                schema: "Communications",
                table: "CommunicationAttachment",
                column: "CommunicationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationPreference_PersonId_PreferenceTypeId_Communica~",
                schema: "Communications",
                table: "CommunicationPreference",
                columns: new[] { "PersonId", "PreferenceTypeId", "CommunicationType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationPreference_PreferenceTypeId",
                schema: "Communications",
                table: "CommunicationPreference",
                column: "PreferenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationPreferenceType_Name",
                schema: "Communications",
                table: "CommunicationPreferenceType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRecipient_CommunicationId",
                schema: "Communications",
                table: "CommunicationRecipient",
                column: "CommunicationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRecipient_PersonId",
                schema: "Communications",
                table: "CommunicationRecipient",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionStatusHistory_ConnectionStatusTypeId",
                schema: "People",
                table: "ConnectionStatusHistory",
                column: "ConnectionStatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionStatusHistory_PersonId",
                schema: "People",
                table: "ConnectionStatusHistory",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionStatusHistory_PersonId_ConnectionStatusTypeId",
                schema: "People",
                table: "ConnectionStatusHistory",
                columns: new[] { "PersonId", "ConnectionStatusTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionStatusType_Name",
                schema: "People",
                table: "ConnectionStatusType",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DiscipleshipStep_DiscipleshipStepDefinitionId",
                schema: "Discipleship",
                table: "DiscipleshipStep",
                column: "DiscipleshipStepDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscipleshipStep_PersonId",
                schema: "Discipleship",
                table: "DiscipleshipStep",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscipleshipStepDefinition_DiscipleshipProgramId",
                schema: "Discipleship",
                table: "DiscipleshipStepDefinition",
                column: "DiscipleshipProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityPermission_EntityIds",
                schema: "Auth",
                table: "EntityPermission",
                column: "EntityIds")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "IX_EntityPermission_EntityType",
                schema: "Auth",
                table: "EntityPermission",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_EntityPermission_EntityType_RecordStatus",
                schema: "Auth",
                table: "EntityPermission",
                columns: new[] { "EntityType", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityPermission_IsSystem_RecordStatus",
                schema: "Auth",
                table: "EntityPermission",
                columns: new[] { "IsSystem", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityPermission_ScopeType_ScopeId",
                schema: "Auth",
                table: "EntityPermission",
                columns: new[] { "ScopeType", "ScopeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Event_ChildCareGroupId",
                schema: "Events",
                table: "Event",
                column: "ChildCareGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ChurchGroupId",
                schema: "Events",
                table: "Event",
                column: "ChurchGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ChurchId",
                schema: "Events",
                table: "Event",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ContactPersonId",
                schema: "Events",
                table: "Event",
                column: "ContactPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_EventRegistrationGroupId",
                schema: "Events",
                table: "Event",
                column: "EventRegistrationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_EventTypeId",
                schema: "Events",
                table: "Event",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Name",
                schema: "Events",
                table: "Event",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Name_RecordStatus",
                schema: "Events",
                table: "Event",
                columns: new[] { "Name", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_EventId_PersonId",
                schema: "Events",
                table: "EventRegistration",
                columns: new[] { "EventId", "PersonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_GroupId",
                schema: "Events",
                table: "EventRegistration",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_PersonId",
                schema: "Events",
                table: "EventRegistration",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_RecordStatus",
                schema: "Events",
                table: "EventRegistration",
                column: "RecordStatus");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_RegisteredByPersonId",
                schema: "Events",
                table: "EventRegistration",
                column: "RegisteredByPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_Status",
                schema: "Events",
                table: "EventRegistration",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_EventSession_EventId",
                schema: "Events",
                table: "EventSession",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSession_Name",
                schema: "Events",
                table: "EventSession",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_EventSession_RecordStatus",
                schema: "Events",
                table: "EventSession",
                column: "RecordStatus");

            migrationBuilder.CreateIndex(
                name: "IX_EventSession_ScheduleId",
                schema: "Events",
                table: "EventSession",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSessionRegistration_EventRegistrationId_EventSessionId",
                schema: "Events",
                table: "EventSessionRegistration",
                columns: new[] { "EventRegistrationId", "EventSessionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventSessionRegistration_EventSessionId",
                schema: "Events",
                table: "EventSessionRegistration",
                column: "EventSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSessionRegistration_PersonId",
                schema: "Events",
                table: "EventSessionRegistration",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_EventType_DefaultGroupTypeId",
                schema: "Events",
                table: "EventType",
                column: "DefaultGroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EventType_Name",
                schema: "Events",
                table: "EventType",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_EventType_Name_RecordStatus",
                schema: "Events",
                table: "EventType",
                columns: new[] { "Name", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Family_Code",
                schema: "People",
                table: "Family",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Family_Name",
                schema: "People",
                table: "Family",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_AssignedPersonId",
                schema: "People",
                table: "FollowUp",
                column: "AssignedPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_PersonId",
                schema: "People",
                table: "FollowUp",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Fund_FundType",
                schema: "Finances",
                table: "Fund",
                column: "FundType");

            migrationBuilder.CreateIndex(
                name: "IX_Fund_ParentFundId",
                schema: "Finances",
                table: "Fund",
                column: "ParentFundId");

            migrationBuilder.CreateIndex(
                name: "IX_Giving_BankStatementImportId",
                schema: "Finances",
                table: "Giving",
                column: "BankStatementImportId");

            migrationBuilder.CreateIndex(
                name: "IX_Giving_BenefactorId",
                schema: "Finances",
                table: "Giving",
                column: "BenefactorId");

            migrationBuilder.CreateIndex(
                name: "IX_Giving_FundId",
                schema: "Finances",
                table: "Giving",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_Giving_GivingType",
                schema: "Finances",
                table: "Giving",
                column: "GivingType");

            migrationBuilder.CreateIndex(
                name: "IX_Giving_PaymentMethod",
                schema: "Finances",
                table: "Giving",
                column: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_Group_ChurchId",
                schema: "Groups",
                table: "Group",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_GroupTypeId",
                schema: "Groups",
                table: "Group",
                column: "GroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_Name",
                schema: "Groups",
                table: "Group",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Group_ParentGroupId",
                schema: "Groups",
                table: "Group",
                column: "ParentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_ScheduleId",
                schema: "Groups",
                table: "Group",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAttendance_AttendanceDate",
                schema: "Groups",
                table: "GroupAttendance",
                column: "AttendanceDate");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAttendance_GroupId",
                schema: "Groups",
                table: "GroupAttendance",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_GroupId",
                schema: "Groups",
                table: "GroupMember",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_GroupRoleId",
                schema: "Groups",
                table: "GroupMember",
                column: "GroupRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_PersonId",
                schema: "Groups",
                table: "GroupMember",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_RecordStatus",
                schema: "Groups",
                table: "GroupMember",
                column: "RecordStatus");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberAttendance_GroupAttendanceId",
                schema: "Groups",
                table: "GroupMemberAttendance",
                column: "GroupAttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberAttendance_GroupId",
                schema: "Groups",
                table: "GroupMemberAttendance",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberAttendance_GroupMemberId",
                schema: "Groups",
                table: "GroupMemberAttendance",
                column: "GroupMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRole_GroupTypeId",
                schema: "Groups",
                table: "GroupRole",
                column: "GroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsFeatures_GroupsId",
                schema: "Groups",
                table: "GroupsFeatures",
                column: "GroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_History_EntityId",
                schema: "Common",
                table: "History",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_History_EntityType",
                schema: "Common",
                table: "History",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_History_RelatedEntityId",
                schema: "Common",
                table: "History",
                column: "RelatedEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_History_RelatedEntityType",
                schema: "Common",
                table: "History",
                column: "RelatedEntityType");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedTransaction_BankStatementImportId",
                schema: "Finances",
                table: "ImportedTransaction",
                column: "BankStatementImportId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedTransaction_GivingId",
                schema: "Finances",
                table: "ImportedTransaction",
                column: "GivingId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedTransaction_ImportId",
                schema: "Finances",
                table: "ImportedTransaction",
                column: "ImportId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedTransaction_OriginalReference",
                schema: "Finances",
                table: "ImportedTransaction",
                column: "OriginalReference");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedTransaction_TransactionType",
                schema: "Finances",
                table: "ImportedTransaction",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_Message_Classification",
                schema: "Communications",
                table: "Message",
                column: "Classification");

            migrationBuilder.CreateIndex(
                name: "IX_Message_IsRead",
                schema: "Communications",
                table: "Message",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_Message_Status",
                schema: "Communications",
                table: "Message",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserId",
                schema: "Communications",
                table: "Message",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Mission_ChurchId",
                schema: "Missions",
                table: "Mission",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Mission_GroupId",
                schema: "Missions",
                table: "Mission",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Mission_Name",
                schema: "Missions",
                table: "Mission",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Mission_Name_RecordStatus",
                schema: "Missions",
                table: "Mission",
                columns: new[] { "Name", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Mission_PersonId",
                schema: "Missions",
                table: "Mission",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Mission_RecordStatus",
                schema: "Missions",
                table: "Mission",
                column: "RecordStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Mission_Type",
                schema: "Missions",
                table: "Mission",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Note_NoteTypeId",
                schema: "People",
                table: "Note",
                column: "NoteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_PersonId",
                schema: "People",
                table: "Note",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineUser_PersonId",
                schema: "People",
                table: "OnlineUser",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_ChurchId",
                schema: "People",
                table: "Person",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_ConnectionStatus",
                schema: "People",
                table: "Person",
                column: "ConnectionStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Person_FamilyId",
                schema: "People",
                table: "Person",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_FullName_FirstName",
                schema: "People",
                table: "Person",
                column: "FullName_FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Person_FullName_LastName",
                schema: "People",
                table: "Person",
                column: "FullName_LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Person_RecordStatus",
                schema: "People",
                table: "Person",
                column: "RecordStatus");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumber_Number",
                schema: "People",
                table: "PhoneNumber",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumber_PersonId",
                schema: "People",
                table: "PhoneNumber",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyChangeRequest_ChangeRequestId",
                schema: "ChangeRequests",
                table: "PropertyChangeRequest",
                column: "ChangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyChangeRequest_EntityId",
                schema: "ChangeRequests",
                table: "PropertyChangeRequest",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyChangeRequest_EntityType",
                schema: "ChangeRequests",
                table: "PropertyChangeRequest",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyChangeRequest_IsApplied",
                schema: "ChangeRequests",
                table: "PropertyChangeRequest",
                column: "IsApplied");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyChangeRequest_PropertyPath",
                schema: "ChangeRequests",
                table: "PropertyChangeRequest",
                column: "PropertyPath");

            migrationBuilder.CreateIndex(
                name: "IX_PushDevice_PersonId",
                schema: "Communications",
                table: "PushDevice",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionAssignment_EntityPermissionId",
                schema: "Auth",
                table: "RolePermissionAssignment",
                column: "EntityPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionAssignment_RoleId",
                schema: "Auth",
                table: "RolePermissionAssignment",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionAssignment_RoleId_EntityPermissionId",
                schema: "Auth",
                table: "RolePermissionAssignment",
                columns: new[] { "RoleId", "EntityPermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionAssignment_RoleId_RecordStatus",
                schema: "Auth",
                table: "RolePermissionAssignment",
                columns: new[] { "RoleId", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceJob_JobKey",
                schema: "Jobs",
                table: "ServiceJob",
                column: "JobKey");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceJob_Name",
                schema: "Jobs",
                table: "ServiceJob",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceJobHistory_ServiceJobId",
                schema: "Jobs",
                table: "ServiceJobHistory",
                column: "ServiceJobId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_PersonId",
                schema: "Auth",
                table: "UserLogin",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_Tenant_RecordStatus",
                schema: "Auth",
                table: "UserLogin",
                columns: new[] { "Tenant", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_Tenant_Username",
                schema: "Auth",
                table: "UserLogin",
                columns: new[] { "Tenant", "Username" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_Username",
                schema: "Auth",
                table: "UserLogin",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginRole_IsSystem_RecordStatus",
                schema: "Auth",
                table: "UserLoginRole",
                columns: new[] { "IsSystem", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginRole_Name",
                schema: "Auth",
                table: "UserLoginRole",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginRole_Name_RecordStatus",
                schema: "Auth",
                table: "UserLoginRole",
                columns: new[] { "Name", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginRole_RecordStatus",
                schema: "Auth",
                table: "UserLoginRole",
                column: "RecordStatus");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleAssignment_UserLoginId",
                schema: "Auth",
                table: "UserRoleAssignment",
                column: "UserLoginId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleAssignment_UserLoginId_RecordStatus",
                schema: "Auth",
                table: "UserRoleAssignment",
                columns: new[] { "UserLoginId", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleAssignment_UserLoginId_UserLoginRoleId",
                schema: "Auth",
                table: "UserRoleAssignment",
                columns: new[] { "UserLoginId", "UserLoginRoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleAssignment_UserLoginRoleId",
                schema: "Auth",
                table: "UserRoleAssignment",
                column: "UserLoginRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Benefactor_Church_ChurchId",
                schema: "Finances",
                table: "Benefactor",
                column: "ChurchId",
                principalSchema: "Churches",
                principalTable: "Church",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Benefactor_Group_GroupId",
                schema: "Finances",
                table: "Benefactor",
                column: "GroupId",
                principalSchema: "Groups",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Benefactor_Person_PersonId",
                schema: "Finances",
                table: "Benefactor",
                column: "PersonId",
                principalSchema: "People",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeRequest_Church_ChurchId",
                schema: "ChangeRequests",
                table: "ChangeRequest",
                column: "ChurchId",
                principalSchema: "Churches",
                principalTable: "Church",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeRequest_Person_ReviewedByPersonId",
                schema: "ChangeRequests",
                table: "ChangeRequest",
                column: "ReviewedByPersonId",
                principalSchema: "People",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Church_ChurchGroup_ChurchGroupId",
                schema: "Churches",
                table: "Church",
                column: "ChurchGroupId",
                principalSchema: "Churches",
                principalTable: "ChurchGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Church_Person_LeaderPersonId",
                schema: "Churches",
                table: "Church",
                column: "LeaderPersonId",
                principalSchema: "People",
                principalTable: "Person",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_Church_ChurchId",
                schema: "People",
                table: "Person");

            migrationBuilder.DropTable(
                name: "ChurchAttendance",
                schema: "Churches");

            migrationBuilder.DropTable(
                name: "ChurchServiceTime",
                schema: "Churches");

            migrationBuilder.DropTable(
                name: "CommunicationAttachment",
                schema: "Communications");

            migrationBuilder.DropTable(
                name: "CommunicationPreference",
                schema: "Communications");

            migrationBuilder.DropTable(
                name: "CommunicationRecipient",
                schema: "Communications");

            migrationBuilder.DropTable(
                name: "ConnectionStatusHistory",
                schema: "People");

            migrationBuilder.DropTable(
                name: "DiscipleshipStep",
                schema: "Discipleship");

            migrationBuilder.DropTable(
                name: "EventSessionRegistration",
                schema: "Events");

            migrationBuilder.DropTable(
                name: "FollowUp",
                schema: "People");

            migrationBuilder.DropTable(
                name: "GroupMemberAttendance",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "GroupsFeatures",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "History",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "ImportedTransaction",
                schema: "Finances");

            migrationBuilder.DropTable(
                name: "Message",
                schema: "Communications");

            migrationBuilder.DropTable(
                name: "Mission",
                schema: "Missions");

            migrationBuilder.DropTable(
                name: "Note",
                schema: "People");

            migrationBuilder.DropTable(
                name: "OnlineUser",
                schema: "People");

            migrationBuilder.DropTable(
                name: "PhoneNumber",
                schema: "People");

            migrationBuilder.DropTable(
                name: "PropertyChangeRequest",
                schema: "ChangeRequests");

            migrationBuilder.DropTable(
                name: "PushDevice",
                schema: "Communications");

            migrationBuilder.DropTable(
                name: "RolePermissionAssignment",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "ServiceJobHistory",
                schema: "Jobs");

            migrationBuilder.DropTable(
                name: "UserRoleAssignment",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "ChurchAttendanceType",
                schema: "Churches");

            migrationBuilder.DropTable(
                name: "CommunicationPreferenceType",
                schema: "Communications");

            migrationBuilder.DropTable(
                name: "Communication",
                schema: "Communications");

            migrationBuilder.DropTable(
                name: "ConnectionStatusType",
                schema: "People");

            migrationBuilder.DropTable(
                name: "DiscipleshipStepDefinition",
                schema: "Discipleship");

            migrationBuilder.DropTable(
                name: "EventRegistration",
                schema: "Events");

            migrationBuilder.DropTable(
                name: "EventSession",
                schema: "Events");

            migrationBuilder.DropTable(
                name: "GroupAttendance",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "GroupMember",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "GroupFeature",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "Giving",
                schema: "Finances");

            migrationBuilder.DropTable(
                name: "NoteType",
                schema: "People");

            migrationBuilder.DropTable(
                name: "ChangeRequest",
                schema: "ChangeRequests");

            migrationBuilder.DropTable(
                name: "EntityPermission",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "ServiceJob",
                schema: "Jobs");

            migrationBuilder.DropTable(
                name: "UserLoginRole",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserLogin",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "CommunicationTemplate",
                schema: "Communications");

            migrationBuilder.DropTable(
                name: "SystemCommunication",
                schema: "Communications");

            migrationBuilder.DropTable(
                name: "DiscipleshipProgram",
                schema: "Discipleship");

            migrationBuilder.DropTable(
                name: "Event",
                schema: "Events");

            migrationBuilder.DropTable(
                name: "GroupRole",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "BankStatementImport",
                schema: "Finances");

            migrationBuilder.DropTable(
                name: "Benefactor",
                schema: "Finances");

            migrationBuilder.DropTable(
                name: "Fund",
                schema: "Finances");

            migrationBuilder.DropTable(
                name: "EventType",
                schema: "Events");

            migrationBuilder.DropTable(
                name: "Group",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "GroupType",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "Schedule",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Church",
                schema: "Churches");

            migrationBuilder.DropTable(
                name: "ChurchGroup",
                schema: "Churches");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "People");

            migrationBuilder.DropTable(
                name: "Family",
                schema: "People");
        }
    }
}
