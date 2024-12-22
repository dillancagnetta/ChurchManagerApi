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


-- Member attendance for last 5 meetings
WITH LastFiveMeetings AS (
    SELECT DISTINCT "AttendanceDate", "Id" as "GroupAttendanceId"
    FROM "GroupAttendance"
    WHERE ("DidNotOccur" = false OR "DidNotOccur" IS NULL)
      AND "RecordStatus" = 'Active'
    --AND "AttendanceDate" >= CURRENT_DATE - INTERVAL '35 days'
    ORDER BY "AttendanceDate" DESC
    LIMIT 5
),
     MemberAttendance AS (
         SELECT
             gm."Id" as "GroupMemberId",
             gm."GroupId",
             lm."AttendanceDate",
             CASE
                 WHEN gma."DidAttend" IS TRUE THEN true
                 ELSE false
                 END as "DidAttend"
         FROM LastFiveMeetings lm
                  CROSS JOIN "GroupMember" gm
                  LEFT JOIN "GroupMemberAttendance" gma
                            ON gma."GroupMemberId" = gm."Id"
                                AND gma."GroupAttendanceId" = lm."GroupAttendanceId"
         WHERE gm."RecordStatus" = 'Active'
     ),
     NumberedAttendance AS (
         SELECT
             "GroupMemberId",
             "GroupId",
             "DidAttend",
             ROW_NUMBER() OVER (PARTITION BY "GroupMemberId" ORDER BY "AttendanceDate" DESC) as "WeekNumber"
         FROM MemberAttendance
     )
SELECT
    na."GroupId",
    na."GroupMemberId",
    bool_or(CASE WHEN na."WeekNumber" = 1 THEN na."DidAttend" END) as "Week1",
    bool_or(CASE WHEN na."WeekNumber" = 2 THEN na."DidAttend" END) as "Week2",
    bool_or(CASE WHEN na."WeekNumber" = 3 THEN na."DidAttend" END) as "Week3",
    bool_or(CASE WHEN na."WeekNumber" = 4 THEN na."DidAttend" END) as "Week4",
    bool_or(CASE WHEN na."WeekNumber" = 5 THEN na."DidAttend" END) as "Week5",
    ROUND(
            CAST(COUNT(CASE WHEN na."DidAttend" = true THEN 1 END) AS DECIMAL) /
            CAST(COUNT(*) AS DECIMAL) * 100
    ) as "Rate"
FROM NumberedAttendance na
GROUP BY na."GroupId", na."GroupMemberId"
ORDER BY na."GroupId", na."GroupMemberId";

-- calculate the rate over all meetings instead
CREATE OR REPLACE FUNCTION get_group_attendance_report(group_id_param integer)
    RETURNS TABLE (
                      "GroupId" integer,
                      "GroupMemberId" integer,
                      "Meeting1" boolean,
                      "Meeting2" boolean,
                      "Meeting3" boolean,
                      "Meeting4" boolean,
                      "Meeting5" boolean,
                      "Rate" decimal
                  ) AS $$
WITH LastFiveMeetings AS (
    SELECT DISTINCT "AttendanceDate", "Id" as "GroupAttendanceId"
    FROM "GroupAttendance"
    WHERE ("DidNotOccur" = false OR "DidNotOccur" IS NULL)
      AND "RecordStatus" = 'Active'
      AND "GroupId" = group_id_param
    ORDER BY "AttendanceDate" DESC
    LIMIT 5
),
     MemberAttendance AS (
         SELECT
             gm."Id" as "GroupMemberId",
             gm."GroupId",
             lm."AttendanceDate",
             CASE
                 WHEN gma."DidAttend" IS TRUE THEN true
                 ELSE false
                 END as "DidAttend"
         FROM LastFiveMeetings lm
                  CROSS JOIN "GroupMember" gm
                  LEFT JOIN "GroupMemberAttendance" gma
                            ON gma."GroupMemberId" = gm."Id"
                                AND gma."GroupAttendanceId" = lm."GroupAttendanceId"
         WHERE gm."RecordStatus" = 'Active'
           AND gm."GroupId" = group_id_param
     ),
     NumberedAttendance AS (
         SELECT
             "GroupMemberId",
             "GroupId",
             "DidAttend",
             ROW_NUMBER() OVER (PARTITION BY "GroupMemberId" ORDER BY "AttendanceDate" DESC) as "MeetingNumber"
         FROM MemberAttendance
     ),
     AllTimeAttendance AS (
         SELECT
             gm."GroupId",
             gm."Id" as "GroupMemberId",
             COUNT(CASE WHEN gma."DidAttend" = true THEN 1 END) as "TotalAttended",
             COUNT(DISTINCT ga."Id") as "TotalMeetings"
         FROM "GroupMember" gm
                  JOIN "GroupAttendance" ga ON ga."GroupId" = gm."GroupId"
                  LEFT JOIN "GroupMemberAttendance" gma
                            ON gma."GroupMemberId" = gm."Id"
                                AND gma."GroupAttendanceId" = ga."Id"
         WHERE gm."RecordStatus" = 'Active'
           AND ga."RecordStatus" = 'Active'
           AND (ga."DidNotOccur" = false OR ga."DidNotOccur" IS NULL)
           AND gm."GroupId" = group_id_param
         GROUP BY gm."GroupId", gm."Id"
     )
SELECT
    na."GroupId",
    na."GroupMemberId",
    bool_or(CASE WHEN na."MeetingNumber" = 1 THEN na."DidAttend" END) as "Meeting1",
    bool_or(CASE WHEN na."MeetingNumber" = 2 THEN na."DidAttend" END) as "Meeting2",
    bool_or(CASE WHEN na."MeetingNumber" = 3 THEN na."DidAttend" END) as "Meeting3",
    bool_or(CASE WHEN na."MeetingNumber" = 4 THEN na."DidAttend" END) as "Meeting4",
    bool_or(CASE WHEN na."MeetingNumber" = 5 THEN na."DidAttend" END) as "Meeting5",
    ROUND(
            CAST(ata."TotalAttended" AS DECIMAL) /
            NULLIF(CAST(ata."TotalMeetings" AS DECIMAL), 0) * 100
    ) as "Rate"
FROM NumberedAttendance na
         JOIN AllTimeAttendance ata
              ON ata."GroupMemberId" = na."GroupMemberId"
                  AND ata."GroupId" = na."GroupId"
GROUP BY na."GroupId", na."GroupMemberId", ata."TotalAttended", ata."TotalMeetings"
ORDER BY na."GroupId", na."GroupMemberId";
$$ LANGUAGE SQL;

DROP FUNCTION IF EXISTS get_group_attendance_report;

-- Dynamic member attendance for past X meetings
CREATE OR REPLACE FUNCTION get_attendance_report(num_meetings integer)
    RETURNS TABLE (
                      "GroupId" integer,
                      "GroupMemberId" integer,
                      "WeeklyAttendance" boolean[],
                      "Rate" decimal
                  ) AS $$
BEGIN
    RETURN QUERY
        WITH LastMeetings AS (
            SELECT DISTINCT ga."AttendanceDate", ga."Id" as "GroupAttendanceId"  -- Added GroupAttendanceId
            FROM "GroupAttendance" ga
            WHERE ("DidNotOccur" = false OR "DidNotOccur" IS NULL)
              AND "RecordStatus" = 'Active'
            ORDER BY ga."AttendanceDate" DESC
            LIMIT num_meetings
        ),
             MemberAttendance AS (
                 SELECT
                     gm."Id" as "MemberId",
                     gm."GroupId",
                     lm."AttendanceDate",
                     CASE
                         WHEN gma."DidAttend" IS TRUE THEN true
                         ELSE false
                         END as "DidAttend"
                 FROM LastMeetings lm
                          CROSS JOIN "GroupMember" gm
                          LEFT JOIN "GroupMemberAttendance" gma
                                    ON gma."GroupMemberId" = gm."Id"
                                        AND gma."GroupAttendanceId" = lm."GroupAttendanceId"
                 WHERE gm."RecordStatus" = 'Active'
             ),
             NumberedAttendance AS (
                 SELECT
                     ma."MemberId",
                     ma."GroupId",
                     ma."DidAttend",
                     ROW_NUMBER() OVER (PARTITION BY ma."MemberId" ORDER BY ma."AttendanceDate" DESC) as "WeekNumber"
                 FROM MemberAttendance ma
             )
        SELECT
            na."GroupId",
            na."MemberId",
            array_agg(na."DidAttend" ORDER BY na."WeekNumber") as "WeeklyAttendance",
            ROUND(
                    CAST(COUNT(CASE WHEN na."DidAttend" = true THEN 1 END) AS DECIMAL) /
                    CAST(COUNT(*) AS DECIMAL) * 100
            ) as "Rate"
        FROM NumberedAttendance na
        GROUP BY na."GroupId", na."MemberId"
        ORDER BY na."GroupId", na."MemberId";
END;
$$ LANGUAGE plpgsql;

-- Member Attendance Join with Person info
CREATE OR REPLACE FUNCTION get_group_attendance_report(group_id_param integer)
    RETURNS TABLE (
                      "GroupId" integer,
                      "GroupMemberId" integer,
                      "PersonId" integer,
                      "PersonFullName" text,
                      "PhotoUrl" text,
                      "Meeting1" boolean,
                      "Meeting2" boolean,
                      "Meeting3" boolean,
                      "Meeting4" boolean,
                      "Meeting5" boolean,
                      "AttendanceRatePercent" decimal
                  ) AS $$
WITH LastFiveMeetings AS (
    SELECT DISTINCT "AttendanceDate", "Id" as "GroupAttendanceId"
    FROM "GroupAttendance"
    WHERE ("DidNotOccur" = false OR "DidNotOccur" IS NULL)
      AND "RecordStatus" = 'Active'
      AND "GroupId" = group_id_param
    ORDER BY "AttendanceDate" DESC
    LIMIT 5
),
     MemberAttendance AS (
         SELECT
             gm."Id" as "GroupMemberId",
             gm."GroupId",
             gm."PersonId",
             p."FullName_FirstName" || ' ' || p."FullName_LastName" as "PersonName",
             p."PhotoUrl",
             lm."AttendanceDate",
             CASE
                 WHEN gma."DidAttend" IS TRUE THEN true
                 ELSE false
                 END as "DidAttend"
         FROM LastFiveMeetings lm
                  CROSS JOIN "GroupMember" gm
                  JOIN "Person" p ON p."Id" = gm."PersonId"
                  LEFT JOIN "GroupMemberAttendance" gma
                            ON gma."GroupMemberId" = gm."Id"
                                AND gma."GroupAttendanceId" = lm."GroupAttendanceId"
         WHERE gm."RecordStatus" = 'Active'
           AND gm."GroupId" = group_id_param
     ),
     NumberedAttendance AS (
         SELECT
             "GroupMemberId",
             "PersonId",
             "GroupId",
             "PersonName",
             "PhotoUrl",
             "DidAttend",
             ROW_NUMBER() OVER (PARTITION BY "GroupMemberId" ORDER BY "AttendanceDate" DESC) as "MeetingNumber"
         FROM MemberAttendance
     ),
     AllTimeAttendance AS (
         SELECT
             gm."GroupId",
             gm."Id" as "GroupMemberId",
             COUNT(CASE WHEN gma."DidAttend" = true THEN 1 END) as "TotalAttended",
             COUNT(DISTINCT ga."Id") as "TotalMeetings"
         FROM "GroupMember" gm
                  JOIN "GroupAttendance" ga ON ga."GroupId" = gm."GroupId"
                  LEFT JOIN "GroupMemberAttendance" gma
                            ON gma."GroupMemberId" = gm."Id"
                                AND gma."GroupAttendanceId" = ga."Id"
         WHERE gm."RecordStatus" = 'Active'
           AND ga."RecordStatus" = 'Active'
           AND (ga."DidNotOccur" = false OR ga."DidNotOccur" IS NULL)
           AND gm."GroupId" = group_id_param
         GROUP BY gm."GroupId", gm."Id"
     )
SELECT
    na."GroupId",
    na."GroupMemberId",
    na."PersonId",
    MAX(na."PersonName"),
    MAX(na."PhotoUrl"),
    bool_or(CASE WHEN na."MeetingNumber" = 1 THEN na."DidAttend" END) as "Meeting1",
    bool_or(CASE WHEN na."MeetingNumber" = 2 THEN na."DidAttend" END) as "Meeting2",
    bool_or(CASE WHEN na."MeetingNumber" = 3 THEN na."DidAttend" END) as "Meeting3",
    bool_or(CASE WHEN na."MeetingNumber" = 4 THEN na."DidAttend" END) as "Meeting4",
    bool_or(CASE WHEN na."MeetingNumber" = 5 THEN na."DidAttend" END) as "Meeting5",
    ROUND(
            CAST(ata."TotalAttended" AS DECIMAL) /
            NULLIF(CAST(ata."TotalMeetings" AS DECIMAL), 0) * 100
    ) as "Rate"
FROM NumberedAttendance na
         JOIN AllTimeAttendance ata
              ON ata."GroupMemberId" = na."GroupMemberId"
                  AND ata."GroupId" = na."GroupId"
GROUP BY na."GroupId", na."GroupMemberId", na."PersonId", ata."TotalAttended", ata."TotalMeetings"
ORDER BY na."GroupId", na."GroupMemberId",  na."PersonId";
$$ LANGUAGE SQL;

-- Test the function
SELECT * FROM get_attendance_report(5);  -- for 8 weeks of attendance

SELECT * FROM get_group_attendance_report(3);

SELECT * FROM "Person"