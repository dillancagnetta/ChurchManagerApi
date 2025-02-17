-- https://kb.objectrocket.com/postgresql/how-to-use-postgresql-lag-function-674
-- https://www.postgresqltutorial.com/postgresql-lag-function/

-- EF CORE QUERY
SELECT
       date_part('year', c."AttendanceDate")::INT AS "Year",
       date_part('month', c."AttendanceDate")::INT AS "Month",
       COALESCE(SUM(c."AttendanceCount"), 0)::INT AS "TotalAttendance",
       COALESCE(SUM(c."MalesCount"), 0)::INT AS "TotalMales",
       COALESCE(SUM(c."FemalesCount"), 0)::INT AS "TotalFemales",
       COALESCE(SUM(c."ChildrenCount"), 0)::INT AS "TotalChildren"
     --, (COALESCE(SUM(c."ChildrenCount"), 0) / nullif(lag(c."ChildrenCount"), 0))
FROM "ChurchAttendance" AS c
WHERE (c."AttendanceDate" >= '2020-09-1') AND (c."AttendanceDate" <= '2020-12-31')
GROUP BY date_part('year', c."AttendanceDate")::INT, date_part('month', c."AttendanceDate")::INT
ORDER BY date_part('year', c."AttendanceDate")::INT DESC, date_part('month', c."AttendanceDate")::INT DESC;

-- ATTENDANCE WEEKLY BY CHURCH, SERVICE
SELECT
    EXTRACT (YEAR FROM "AttendanceDate") as year,
    EXTRACT (MONTH FROM "AttendanceDate") as month,
    EXTRACT (WEEK FROM "AttendanceDate") as week,
    CH."Name" as "Church",
    CAT."Name" as "ServiceType",
    COALESCE(SUM("AttendanceCount"), 0)::INT AS "TotalAttendance",
    COALESCE(SUM("MalesCount"), 0)::INT AS "TotalMales",
    COALESCE(SUM("FemalesCount"), 0)::INT AS "TotalFemales",
    COALESCE(SUM("ChildrenCount"), 0)::INT AS "TotalChildren"
--, (COALESCE(SUM(c."ChildrenCount"), 0) / nullif(lag(c."ChildrenCount"), 0))
FROM "ChurchAttendance"
JOIN "ChurchAttendanceType" CAT on "ChurchAttendanceTypeId" = CAT."Id"
JOIN "Church" CH ON "ChurchId" = CH."Id"
--WHERE ("AttendanceDate" >= '2020-09-1') AND ("AttendanceDate" <= '2020-12-31')
GROUP BY
    EXTRACT (YEAR FROM "AttendanceDate"),
    EXTRACT (MONTH FROM "AttendanceDate"),
    EXTRACT (WEEK FROM "AttendanceDate"),
    CH."Name",
    CAT."Name"
ORDER BY 1 DESC, 2 DESC;

-- LAG BASIC
WITH cte AS (
   SELECT
        date_part('year', "AttendanceDate") AS Year,
        date_part('month', "AttendanceDate") AS Month,
		COALESCE(SUM("AttendanceCount"), 0) AS TotalAttendance
   FROM "ChurchAttendance"
	GROUP BY Year, Month
	ORDER BY Year, Month
)
SELECT
   Year,
   Month,
   TotalAttendance,
   LAG(TotalAttendance, 1) OVER (
      ORDER BY Year
   ) previous_year_attendance
FROM
   cte;


-- WITH VARIANCE

WITH cte AS (
   SELECT
        date_part('year', "AttendanceDate") AS Year,
        date_part('month', "AttendanceDate") AS Month,
		COALESCE(SUM("AttendanceCount"), 0) AS TotalAttendance
   FROM "ChurchAttendance"
	GROUP BY Year, Month
	ORDER BY Year, Month
), cte2 AS (
   SELECT
       Year,
       Month,
       TotalAttendance,
       LAG(TotalAttendance, 1) OVER (
          ORDER BY Year DESC , Month DESC
       ) previous_year_attendance
   FROM
      cte
)
SELECT
   Year,
   Month,
   TotalAttendance,
   previous_year_attendance,
   (TotalAttendance - previous_year_attendance) AS variance
FROM
   cte2;


-- WITH PARTITION

WITH cte AS (
   SELECT
        date_part('year', "AttendanceDate") AS Year,
        date_part('month', "AttendanceDate") AS Month,
        "ChurchId" AS ChurchId,
		COALESCE(SUM("AttendanceCount"), 0) AS TotalAttendance
   FROM "ChurchAttendance"
	GROUP BY Year, Month, "ChurchId"
	ORDER BY Year, Month
), cte2 AS (
   SELECT
       Year,
       Month,
       ChurchId,
       TotalAttendance,
       LAG(TotalAttendance, 1) OVER (
          PARTITION BY ChurchId
          ORDER BY Year , Month
       ) previous_year_attendance
   FROM
      cte
)
SELECT
   Year,
   Month,
   ChurchId,
   TotalAttendance,
   previous_year_attendance,
   (TotalAttendance - previous_year_attendance) AS variance
FROM
   cte2;


-- ATTENDANCE BREAKDOWN WITH PERCENTAGE

WITH cte AS (
   SELECT
        date_part('year', "AttendanceDate") AS Year,
        date_part('month', "AttendanceDate") AS Month,
        "ChurchId" AS ChurchId,
		COALESCE(SUM("MalesCount"), 0) AS TotalMales,
		COALESCE(SUM("FemalesCount"), 0) AS TotalFemales,
		COALESCE(SUM("ChildrenCount"), 0) AS TotalChildren
   FROM "ChurchAttendance"
	GROUP BY Year, Month, "ChurchId"
	ORDER BY Year , Month
), cte2 AS (
   SELECT
       Year,
       Month,
       ChurchId,
       TotalMales,
       TotalFemales,
       TotalChildren,
       -- MALES
       LAG(TotalMales, 1) OVER (
          PARTITION BY ChurchId
          ORDER BY Year , Month
       ) previous_month_males_attendance,
       -- FEMALES
       LAG(TotalFemales, 1) OVER (
          PARTITION BY ChurchId
          ORDER BY Year , Month
       ) previous_month_females_attendance,
       -- CHILDREN
       LAG(TotalChildren, 1) OVER (
          PARTITION BY ChurchId
          ORDER BY Year , Month
       ) previous_month_children_attendance
   FROM
      cte
)
SELECT
   Year,
   Month,
   ChurchId,
   -- MALES
   TotalMales,
   previous_month_males_attendance,
   (TotalMales - COALESCE(previous_month_males_attendance, 0)) AS variance_males,
   ((TotalMales - COALESCE(previous_month_males_attendance, 0)::FLOAT) / TotalMales::FLOAT * 100)::FLOAT AS variance_males_percentage,
   -- FEMALES
   TotalFemales,
   previous_month_females_attendance,
   (TotalFemales - COALESCE(previous_month_females_attendance, 0)) AS variance_males,
   ((TotalFemales - COALESCE(previous_month_females_attendance, 0)::FLOAT) / TotalFemales::FLOAT * 100)::FLOAT AS variance_females_percentage,
  -- CHILDREN
   TotalChildren,
   previous_month_females_attendance,
   (TotalChildren - COALESCE(previous_month_children_attendance, 0)) AS variance_males,
   ((TotalChildren - COALESCE(previous_month_children_attendance, 0)::FLOAT) / TotalChildren::FLOAT * 100)::FLOAT AS variance_children_percentage
FROM
   cte2;


-- FUNCTIONS
-- https://carto.com/help/working-with-data/sql-stored-procedures/
CREATE OR REPLACE FUNCTION get_total_attendance_basic()
RETURNS TABLE(
   YearA INT,
   MonthA INT,
   TotalAttendancea NUMERIC,
   Previous NUMERIC
)
AS $$
BEGIN
  RETURN QUERY
    -- LAG BASIC
    WITH cte AS (
       SELECT
            date_part('year', "AttendanceDate") AS Year,
            date_part('month', "AttendanceDate") AS Month,
            COALESCE(SUM("AttendanceCount"), 0) AS TotalAttendance
       FROM "ChurchAttendance"
        GROUP BY Year, Month
        ORDER BY Year, Month
    )
    SELECT
       Year::INT,
       Month::INT,
       TotalAttendance::NUMERIC,
       LAG(TotalAttendance, 1) OVER (
          ORDER BY Year
       )::NUMERIC  AS previous_year_attendance
    FROM
       cte;

END;
$$ LANGUAGE plpgsql;

SELECT * FROM get_total_attendance_basic();



