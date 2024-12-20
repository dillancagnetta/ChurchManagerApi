--  overview of the groups attendance records and patterns
WITH group_attendance_stats AS (
    SELECT
        gma."GroupId",
        --g."Name" as GroupName,
        gma."AttendanceDate",
        COUNT(DISTINCT gm."Id") as TotalMembers,
        COUNT(DISTINCT CASE WHEN gma."DidAttend" = true THEN gma."Id" END) as MembersPresent,
        COUNT(DISTINCT CASE WHEN gma."IsFirstTime" = true THEN gma."Id" END) as FirstTimers,
        COUNT(DISTINCT CASE WHEN gma."IsNewConvert" = true THEN gma."Id" END) as NewConverts,
        COUNT(DISTINCT CASE WHEN gma."ReceivedHolySpirit" = true THEN gma."Id" END) as ReceivedHolySpirit
    FROM "GroupMemberAttendance" gma
    JOIN "GroupMember" gm ON gma."GroupId" = gm."GroupId"
    -- WHERE gma."GroupId" = 3 AND "AttendanceDate" > '2024-11-14 04:06:09.606574' -- Testing
    GROUP BY gma."GroupId", gma."AttendanceDate"
)
SELECT
    "GroupId",
    "AttendanceDate",
    TotalMembers,
    MembersPresent,
    ROUND((MembersPresent::decimal / NULLIF(TotalMembers, 0)) * 100, 1) as AttendancePercentage,
    FirstTimers,
    NewConverts,
    ReceivedHolySpirit,
    -- Calculate growth (positive or negative compared to previous attendance)
    MembersPresent - LAG(MembersPresent, 1) OVER (PARTITION BY "GroupId" ORDER BY "AttendanceDate") as Growth,
    -- Calculate days since last attendance
    "AttendanceDate" - LAG("AttendanceDate", 1) OVER (PARTITION BY "GroupId" ORDER BY "AttendanceDate") as DaysSinceLastAttendance
FROM group_attendance_stats
ORDER BY  "GroupId", "AttendanceDate" DESC;


SELECT count(*)  FROM "GroupMemberAttendance" WHERE "GroupId" = 2;

DELETE FROM  "GroupMemberAttendance";
DELETE FROM  "GroupAttendance";

SELECT * FROM "GroupMemberAttendance" WHERE "GroupId" = 7 AND "IsFirstTime" = true
ORDER BY "AttendanceDate", "GroupMemberId";

SELECT * FROM "GroupMemberAttendance" WHERE "GroupId" = 3 AND "AttendanceDate" = '2022-12-15 00:00:00.000000';
SELECT * FROM "GroupMemberAttendance" WHERE "GroupAttendanceId" = 8873;
SELECT * FROM "GroupAttendance" WHERE "Id" = 8873;
SELECT * FROM "GroupAttendance" WHERE "NewConvertCount" > 1;
SELECT * FROM "GroupAttendance" WHERE "GroupId" = 2;
