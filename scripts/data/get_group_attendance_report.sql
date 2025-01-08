-- Member Attendance in Last 5 meetings for a group Join with Person info
CREATE OR REPLACE FUNCTION get_group_attendance_report(group_id_param integer)
    RETURNS TABLE (
                      "GroupId" integer,
                      "GroupMemberId" integer,
                      "PersonId" integer,
                      "PersonName" text,
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

SELECT * FROM get_group_attendance_report(3);