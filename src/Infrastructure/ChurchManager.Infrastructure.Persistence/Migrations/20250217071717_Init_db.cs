using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChurchAttendanceType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchAttendanceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    LogoFileUrl = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    SupportedTypes = table.Column<string>(type: "text", nullable: true),
                    IsBaseTemplate = table.Column<bool>(type: "boolean", nullable: false),
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
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionStatusType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscipleshipProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    EntityType = table.Column<string>(type: "text", nullable: true),
                    EntityIds = table.Column<int[]>(type: "integer[]", nullable: true),
                    IsDynamicScope = table.Column<bool>(type: "boolean", nullable: false),
                    ScopeType = table.Column<string>(type: "text", nullable: true),
                    ScopeId = table.Column<int>(type: "integer", nullable: false),
                    CanView = table.Column<bool>(type: "boolean", nullable: false),
                    CanEdit = table.Column<bool>(type: "boolean", nullable: false),
                    CanDelete = table.Column<bool>(type: "boolean", nullable: false),
                    CanManageUsers = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Family", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupFeature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupFeature", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    GroupTerm = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    GroupMemberTerm = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TakesAttendance = table.Column<bool>(type: "boolean", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    IconCssClass = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "History",
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
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CssClass = table.Column<string>(type: "text", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(type: "Date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "Date", nullable: true),
                    iCalendarContent = table.Column<string>(type: "text", nullable: true),
                    WeeklyDayOfWeek = table.Column<int>(type: "integer", nullable: true),
                    WeeklyTimeOfDay = table.Column<TimeSpan>(type: "interval", nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    Frequency = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Timezone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceJobs",
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
                    table.PrimaryKey("PK_ServiceJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemCommunication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemCommunication", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                name: "ChurchAttendance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChurchAttendanceTypeId = table.Column<int>(type: "integer", nullable: false),
                    ChurchId = table.Column<int>(type: "integer", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AttendanceCount = table.Column<int>(type: "integer", nullable: false),
                    MalesCount = table.Column<int>(type: "integer", nullable: true),
                    FemalesCount = table.Column<int>(type: "integer", nullable: true),
                    ChildrenCount = table.Column<int>(type: "integer", nullable: true),
                    FirstTimerCount = table.Column<int>(type: "integer", nullable: true),
                    NewConvertCount = table.Column<int>(type: "integer", nullable: true),
                    ReceivedHolySpiritCount = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchAttendance_ChurchAttendanceType_ChurchAttendanceTypeId",
                        column: x => x.ChurchAttendanceTypeId,
                        principalTable: "ChurchAttendanceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscipleshipStepDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscipleshipProgramId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IconCssClass = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AllowMultiple = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "DiscipleshipProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventType",
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
                    OnlineSupport = table.Column<string>(type: "text", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    IconCssClass = table.Column<string>(type: "text", nullable: true),
                    ChildCare_HasChildCare = table.Column<bool>(type: "boolean", nullable: true),
                    ChildCare_MinChildAge = table.Column<int>(type: "integer", nullable: true),
                    ChildCare_MaxChildAge = table.Column<int>(type: "integer", nullable: true),
                    AgeClassification = table.Column<string>(type: "text", nullable: true),
                    DefaultGroupTypeId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "GroupType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupRole",
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
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupRole_GroupType_GroupTypeId",
                        column: x => x.GroupTypeId,
                        principalTable: "GroupType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceJobHistory",
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
                        name: "FK_ServiceJobHistory_ServiceJobs_ServiceJobId",
                        column: x => x.ServiceJobId,
                        principalTable: "ServiceJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissionAssignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    EntityPermissionId = table.Column<int>(type: "integer", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "EntityPermission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissionAssignment_UserLoginRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "UserLoginRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Church",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChurchGroupId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ShortCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LeaderPersonId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Church", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Group",
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
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "Church",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Group_GroupType_GroupTypeId",
                        column: x => x.GroupTypeId,
                        principalTable: "GroupType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Group_Group_ParentGroupId",
                        column: x => x.ParentGroupId,
                        principalTable: "Group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Group_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Person",
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
                    ConnectionStatus = table.Column<string>(type: "text", nullable: true),
                    DeceasedStatus_IsDeceased = table.Column<bool>(type: "boolean", nullable: true),
                    DeceasedStatus_DeceasedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AgeClassification = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
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
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Person_Church_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Church",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Person_Family_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Family",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupAttendance",
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
                    PhotoUrls = table.Column<List<string>>(type: "text[]", nullable: true),
                    AttendanceReview_IsReviewed = table.Column<bool>(type: "boolean", nullable: true),
                    AttendanceReview_Feedback = table.Column<string>(type: "text", nullable: true),
                    AttendanceReview_ReviewedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupAttendance_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupsFeatures",
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
                        principalTable: "GroupFeature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupsFeatures_Group_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChurchGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LeaderPersonId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchGroup_Person_LeaderPersonId",
                        column: x => x.LeaderPersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Subject = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CommunicationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ListGroupId = table.Column<int>(type: "integer", nullable: true),
                    CommunicationTemplateId = table.Column<int>(type: "integer", nullable: true),
                    CommunicationContent = table.Column<string>(type: "text", nullable: true),
                    SenderPersonId = table.Column<int>(type: "integer", nullable: true),
                    IsBulkCommunication = table.Column<bool>(type: "boolean", nullable: false),
                    SendDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    FutureSendDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Review_ReviewerNote = table.Column<string>(type: "text", nullable: true),
                    Review_ReviewedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Review_ReviewerPersonId = table.Column<int>(type: "integer", nullable: true),
                    Metadata = table.Column<string>(type: "jsonb", nullable: true),
                    SystemCommunicationId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Communication_Group_ListGroupId",
                        column: x => x.ListGroupId,
                        principalTable: "Group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Communication_Person_SenderPersonId",
                        column: x => x.SenderPersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Communication_SystemCommunication_SystemCommunicationId",
                        column: x => x.SystemCommunicationId,
                        principalTable: "SystemCommunication",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiscipleshipStep",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscipleshipStepDefinitionId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EndDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Note = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "DiscipleshipStepDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscipleshipStep_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowUp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssignedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    AssignedPersonId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Severity = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    RequiresAdditionalFollowUp = table.Column<bool>(type: "boolean", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowUp_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMember",
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
                    CommunicationPreference = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMember_GroupRole_GroupRoleId",
                        column: x => x.GroupRoleId,
                        principalTable: "GroupRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMember_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMember_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Category = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Stream = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IconCssClass = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    PhotoUrls = table.Column<List<string>>(type: "text[]", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "Church",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Mission_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Mission_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NoteTypeId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Caption = table.Column<string>(type: "text", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "NoteType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Note_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OnlineUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    ConnectionId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    LastOnlineDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlineUser_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonConnectionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    ConnectionStatusTypeId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonConnectionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonConnectionHistory_ConnectionStatusType_ConnectionStat~",
                        column: x => x.ConnectionStatusTypeId,
                        principalTable: "ConnectionStatusType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonConnectionHistory_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Extension = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsMessagingEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsUnlisted = table.Column<bool>(type: "boolean", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhoneNumber_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PushDevice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Endpoint = table.Column<string>(type: "text", nullable: true),
                    P256DH = table.Column<string>(type: "text", nullable: true),
                    Auth = table.Column<string>(type: "text", nullable: true),
                    UniqueIdentification = table.Column<string>(type: "text", nullable: true),
                    PersonId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushDevice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PushDevice_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Tenant = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogin_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
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
                    ScheduleId = table.Column<int>(type: "integer", nullable: false),
                    ChildCareGroupId = table.Column<int>(type: "integer", nullable: true),
                    EventRegistrationGroupId = table.Column<int>(type: "integer", nullable: false),
                    ContactPersonId = table.Column<int>(type: "integer", nullable: false),
                    ContactEmail = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: true),
                    ContactPhone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Review_ReviewerNote = table.Column<string>(type: "text", nullable: true),
                    Review_ReviewedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Review_ReviewerPersonId = table.Column<int>(type: "integer", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "text", nullable: true),
                    Capacity = table.Column<int>(type: "integer", nullable: true),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "ChurchGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Event_Church_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Church",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Event_EventType_EventTypeId",
                        column: x => x.EventTypeId,
                        principalTable: "EventType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_Group_ChildCareGroupId",
                        column: x => x.ChildCareGroupId,
                        principalTable: "Group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Event_Group_EventRegistrationGroupId",
                        column: x => x.EventRegistrationGroupId,
                        principalTable: "Group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Event_Person_ContactPersonId",
                        column: x => x.ContactPersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Event_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommunicationAttachment",
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
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "Communication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationRecipient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommunicationId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    StatusNote = table.Column<string>(type: "text", nullable: true),
                    SendDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    OpenedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UniqueMessageId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AttemptCount = table.Column<int>(type: "integer", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationRecipient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationRecipient_Communication_CommunicationId",
                        column: x => x.CommunicationId,
                        principalTable: "Communication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunicationRecipient_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupMemberAttendance",
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
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMemberAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMemberAttendance_GroupAttendance_GroupAttendanceId",
                        column: x => x.GroupAttendanceId,
                        principalTable: "GroupAttendance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMemberAttendance_GroupMember_GroupMemberId",
                        column: x => x.GroupMemberId,
                        principalTable: "GroupMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMemberAttendance_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
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
                    Status = table.Column<string>(type: "text", nullable: true),
                    LastError = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_UserLogin_UserId",
                        column: x => x.UserId,
                        principalTable: "UserLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleAssignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserLoginId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserLoginRoleId = table.Column<int>(type: "integer", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "UserLoginRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleAssignment_UserLogin_UserLoginId",
                        column: x => x.UserLoginId,
                        principalTable: "UserLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventRegistration",
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
                    Status = table.Column<string>(type: "text", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventRegistration_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventRegistration_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventRegistration_Person_RegisteredByPersonId",
                        column: x => x.RegisteredByPersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EventSession",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Capacity = table.Column<int>(type: "integer", nullable: true),
                    AttendanceRequired = table.Column<bool>(type: "boolean", nullable: false),
                    SessionOrder = table.Column<int>(type: "integer", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EndDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OnlineSupport = table.Column<string>(type: "text", nullable: true),
                    OnlineMeetingUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsCancelled = table.Column<bool>(type: "boolean", nullable: false),
                    CancellationReason = table.Column<string>(type: "text", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventSessionRegistration",
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
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
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
                        principalTable: "EventRegistration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSessionRegistration_EventSession_EventSessionId",
                        column: x => x.EventSessionId,
                        principalTable: "EventSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventSessionRegistration_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Church_ChurchGroupId",
                table: "Church",
                column: "ChurchGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Church_LeaderPersonId",
                table: "Church",
                column: "LeaderPersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChurchAttendance_ChurchAttendanceTypeId",
                table: "ChurchAttendance",
                column: "ChurchAttendanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchGroup_LeaderPersonId",
                table: "ChurchGroup",
                column: "LeaderPersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Communication_CommunicationTemplateId",
                table: "Communication",
                column: "CommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_ListGroupId",
                table: "Communication",
                column: "ListGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_SenderPersonId",
                table: "Communication",
                column: "SenderPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_SystemCommunicationId",
                table: "Communication",
                column: "SystemCommunicationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationAttachment_CommunicationId",
                table: "CommunicationAttachment",
                column: "CommunicationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRecipient_CommunicationId",
                table: "CommunicationRecipient",
                column: "CommunicationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRecipient_PersonId",
                table: "CommunicationRecipient",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionStatusType_Name",
                table: "ConnectionStatusType",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DiscipleshipStep_DiscipleshipStepDefinitionId",
                table: "DiscipleshipStep",
                column: "DiscipleshipStepDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscipleshipStep_PersonId",
                table: "DiscipleshipStep",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscipleshipStepDefinition_DiscipleshipProgramId",
                table: "DiscipleshipStepDefinition",
                column: "DiscipleshipProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityPermission_EntityIds",
                table: "EntityPermission",
                column: "EntityIds")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "IX_EntityPermission_EntityType",
                table: "EntityPermission",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_EntityPermission_EntityType_RecordStatus",
                table: "EntityPermission",
                columns: new[] { "EntityType", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityPermission_IsSystem_RecordStatus",
                table: "EntityPermission",
                columns: new[] { "IsSystem", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityPermission_ScopeType_ScopeId",
                table: "EntityPermission",
                columns: new[] { "ScopeType", "ScopeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Event_ChildCareGroupId",
                table: "Event",
                column: "ChildCareGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ChurchGroupId",
                table: "Event",
                column: "ChurchGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ChurchId",
                table: "Event",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ContactPersonId",
                table: "Event",
                column: "ContactPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_EventRegistrationGroupId",
                table: "Event",
                column: "EventRegistrationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_EventTypeId",
                table: "Event",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Name",
                table: "Event",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Name_RecordStatus",
                table: "Event",
                columns: new[] { "Name", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Event_ScheduleId",
                table: "Event",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_EventId_PersonId",
                table: "EventRegistration",
                columns: new[] { "EventId", "PersonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_GroupId",
                table: "EventRegistration",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_PersonId",
                table: "EventRegistration",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_RecordStatus",
                table: "EventRegistration",
                column: "RecordStatus");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_RegisteredByPersonId",
                table: "EventRegistration",
                column: "RegisteredByPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_Status",
                table: "EventRegistration",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_EventSession_EventId",
                table: "EventSession",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSession_Name",
                table: "EventSession",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_EventSession_RecordStatus",
                table: "EventSession",
                column: "RecordStatus");

            migrationBuilder.CreateIndex(
                name: "IX_EventSessionRegistration_EventRegistrationId_EventSessionId",
                table: "EventSessionRegistration",
                columns: new[] { "EventRegistrationId", "EventSessionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventSessionRegistration_EventSessionId",
                table: "EventSessionRegistration",
                column: "EventSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSessionRegistration_PersonId",
                table: "EventSessionRegistration",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_EventType_DefaultGroupTypeId",
                table: "EventType",
                column: "DefaultGroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EventType_Name",
                table: "EventType",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_EventType_Name_RecordStatus",
                table: "EventType",
                columns: new[] { "Name", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Family_Code",
                table: "Family",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Family_Name",
                table: "Family",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_AssignedPersonId",
                table: "FollowUp",
                column: "AssignedPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_PersonId",
                table: "FollowUp",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_ChurchId",
                table: "Group",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_GroupTypeId",
                table: "Group",
                column: "GroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_Name",
                table: "Group",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Group_ParentGroupId",
                table: "Group",
                column: "ParentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_ScheduleId",
                table: "Group",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAttendance_AttendanceDate",
                table: "GroupAttendance",
                column: "AttendanceDate");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAttendance_GroupId",
                table: "GroupAttendance",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_GroupId_PersonId",
                table: "GroupMember",
                columns: new[] { "GroupId", "PersonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_GroupRoleId",
                table: "GroupMember",
                column: "GroupRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_PersonId",
                table: "GroupMember",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_RecordStatus",
                table: "GroupMember",
                column: "RecordStatus");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberAttendance_GroupAttendanceId",
                table: "GroupMemberAttendance",
                column: "GroupAttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberAttendance_GroupId",
                table: "GroupMemberAttendance",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberAttendance_GroupMemberId",
                table: "GroupMemberAttendance",
                column: "GroupMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRole_GroupTypeId",
                table: "GroupRole",
                column: "GroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsFeatures_GroupsId",
                table: "GroupsFeatures",
                column: "GroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_History_EntityId",
                table: "History",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_History_EntityType",
                table: "History",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_History_RelatedEntityId",
                table: "History",
                column: "RelatedEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_History_RelatedEntityType",
                table: "History",
                column: "RelatedEntityType");

            migrationBuilder.CreateIndex(
                name: "IX_Message_Classification",
                table: "Message",
                column: "Classification");

            migrationBuilder.CreateIndex(
                name: "IX_Message_IsRead",
                table: "Message",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_Message_Status",
                table: "Message",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserId",
                table: "Message",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Mission_ChurchId",
                table: "Mission",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Mission_GroupId",
                table: "Mission",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Mission_Name",
                table: "Mission",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Mission_Name_RecordStatus",
                table: "Mission",
                columns: new[] { "Name", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Mission_PersonId",
                table: "Mission",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Mission_RecordStatus",
                table: "Mission",
                column: "RecordStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Mission_Type",
                table: "Mission",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Note_NoteTypeId",
                table: "Note",
                column: "NoteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_PersonId",
                table: "Note",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineUser_PersonId",
                table: "OnlineUser",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_ChurchId",
                table: "Person",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_ConnectionStatus",
                table: "Person",
                column: "ConnectionStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Person_FamilyId",
                table: "Person",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_FullName_FirstName",
                table: "Person",
                column: "FullName_FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Person_FullName_LastName",
                table: "Person",
                column: "FullName_LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Person_RecordStatus",
                table: "Person",
                column: "RecordStatus");

            migrationBuilder.CreateIndex(
                name: "IX_PersonConnectionHistory_ConnectionStatusTypeId",
                table: "PersonConnectionHistory",
                column: "ConnectionStatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonConnectionHistory_PersonId",
                table: "PersonConnectionHistory",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonConnectionHistory_PersonId_ConnectionStatusTypeId",
                table: "PersonConnectionHistory",
                columns: new[] { "PersonId", "ConnectionStatusTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumber_PersonId",
                table: "PhoneNumber",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PushDevice_PersonId",
                table: "PushDevice",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionAssignment_EntityPermissionId",
                table: "RolePermissionAssignment",
                column: "EntityPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionAssignment_RoleId",
                table: "RolePermissionAssignment",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionAssignment_RoleId_EntityPermissionId",
                table: "RolePermissionAssignment",
                columns: new[] { "RoleId", "EntityPermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionAssignment_RoleId_RecordStatus",
                table: "RolePermissionAssignment",
                columns: new[] { "RoleId", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceJobHistory_ServiceJobId",
                table: "ServiceJobHistory",
                column: "ServiceJobId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceJobs_JobKey",
                table: "ServiceJobs",
                column: "JobKey");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceJobs_Name",
                table: "ServiceJobs",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_PersonId",
                table: "UserLogin",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_Tenant_RecordStatus",
                table: "UserLogin",
                columns: new[] { "Tenant", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_Tenant_Username",
                table: "UserLogin",
                columns: new[] { "Tenant", "Username" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_Username",
                table: "UserLogin",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginRole_IsSystem_RecordStatus",
                table: "UserLoginRole",
                columns: new[] { "IsSystem", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginRole_Name",
                table: "UserLoginRole",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginRole_Name_RecordStatus",
                table: "UserLoginRole",
                columns: new[] { "Name", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginRole_RecordStatus",
                table: "UserLoginRole",
                column: "RecordStatus");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleAssignment_UserLoginId",
                table: "UserRoleAssignment",
                column: "UserLoginId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleAssignment_UserLoginId_RecordStatus",
                table: "UserRoleAssignment",
                columns: new[] { "UserLoginId", "RecordStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleAssignment_UserLoginId_UserLoginRoleId",
                table: "UserRoleAssignment",
                columns: new[] { "UserLoginId", "UserLoginRoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleAssignment_UserLoginRoleId",
                table: "UserRoleAssignment",
                column: "UserLoginRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Church_ChurchGroup_ChurchGroupId",
                table: "Church",
                column: "ChurchGroupId",
                principalTable: "ChurchGroup",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Church_Person_LeaderPersonId",
                table: "Church",
                column: "LeaderPersonId",
                principalTable: "Person",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Church_ChurchGroup_ChurchGroupId",
                table: "Church");

            migrationBuilder.DropForeignKey(
                name: "FK_Church_Person_LeaderPersonId",
                table: "Church");

            migrationBuilder.DropTable(
                name: "ChurchAttendance");

            migrationBuilder.DropTable(
                name: "CommunicationAttachment");

            migrationBuilder.DropTable(
                name: "CommunicationRecipient");

            migrationBuilder.DropTable(
                name: "DiscipleshipStep");

            migrationBuilder.DropTable(
                name: "EventSessionRegistration");

            migrationBuilder.DropTable(
                name: "FollowUp");

            migrationBuilder.DropTable(
                name: "GroupMemberAttendance");

            migrationBuilder.DropTable(
                name: "GroupsFeatures");

            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Mission");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "OnlineUser");

            migrationBuilder.DropTable(
                name: "PersonConnectionHistory");

            migrationBuilder.DropTable(
                name: "PhoneNumber");

            migrationBuilder.DropTable(
                name: "PushDevice");

            migrationBuilder.DropTable(
                name: "RolePermissionAssignment");

            migrationBuilder.DropTable(
                name: "ServiceJobHistory");

            migrationBuilder.DropTable(
                name: "UserRoleAssignment");

            migrationBuilder.DropTable(
                name: "ChurchAttendanceType");

            migrationBuilder.DropTable(
                name: "Communication");

            migrationBuilder.DropTable(
                name: "DiscipleshipStepDefinition");

            migrationBuilder.DropTable(
                name: "EventRegistration");

            migrationBuilder.DropTable(
                name: "EventSession");

            migrationBuilder.DropTable(
                name: "GroupAttendance");

            migrationBuilder.DropTable(
                name: "GroupMember");

            migrationBuilder.DropTable(
                name: "GroupFeature");

            migrationBuilder.DropTable(
                name: "NoteType");

            migrationBuilder.DropTable(
                name: "ConnectionStatusType");

            migrationBuilder.DropTable(
                name: "EntityPermission");

            migrationBuilder.DropTable(
                name: "ServiceJobs");

            migrationBuilder.DropTable(
                name: "UserLoginRole");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "CommunicationTemplate");

            migrationBuilder.DropTable(
                name: "SystemCommunication");

            migrationBuilder.DropTable(
                name: "DiscipleshipProgram");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "GroupRole");

            migrationBuilder.DropTable(
                name: "EventType");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "GroupType");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "ChurchGroup");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Church");

            migrationBuilder.DropTable(
                name: "Family");
        }
    }
}
