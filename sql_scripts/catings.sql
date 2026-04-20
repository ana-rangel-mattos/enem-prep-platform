-- planning.day_of_the_week
CREATE OR REPLACE FUNCTION planning.map_integer_day_of_the_week(day_int integer)
    RETURNS planning.day_of_the_week AS $$
BEGIN
    RETURN CASE day_int
        WHEN 0 THEN 'MONDAY'::planning.day_of_the_week
        WHEN 1 THEN 'TUESDAY'::planning.day_of_the_week
        WHEN 2 THEN 'WEDNESDAY'::planning.day_of_the_week
        WHEN 3 THEN 'THURSDAY'::planning.day_of_the_week
        WHEN 4 THEN 'FRIDAY'::planning.day_of_the_week
        WHEN 5 THEN 'SUNDAY'::planning.day_of_the_week
        WHEN 6 THEN 'SATURDAY'::planning.day_of_the_week
        ELSE 'MONDAY'::planning.day_of_the_week
    END;
END;
$$ LANGUAGE plpgsql IMMUTABLE;

CREATE CAST ( INTEGER AS planning.day_of_the_week )
WITH FUNCTION planning.map_integer_day_of_the_week(integer)
AS ASSIGNMENT;

-- content.language
CREATE OR REPLACE FUNCTION content.map_integer_exam_language(day_int integer)
    RETURNS content.language AS $$
BEGIN
    RETURN CASE day_int
        WHEN 0 THEN 'INGLES'::content.language
        WHEN 1 THEN 'ESPANHOL'::content.language
        ELSE 'INGLES'::content.language
    END;
END;
$$ LANGUAGE plpgsql IMMUTABLE;

CREATE CAST ( INTEGER AS content.language )
WITH FUNCTION content.map_integer_exam_language(integer)
AS ASSIGNMENT;

-- tracking.exam_status
CREATE OR REPLACE FUNCTION tracking.map_integer_exam_status(status_int integer)
    RETURNS tracking.exam_status AS $$
BEGIN
    RETURN CASE status_int
        WHEN 0 THEN 'NOT_STARTED'::tracking.exam_status
        WHEN 1 THEN 'IN_PROGRESS'::tracking.exam_status
        WHEN 2 THEN 'FINISHED'::tracking.exam_status
        WHEN 3 THEN 'CANCELED'::tracking.exam_status
        ELSE 'NOT_STARTED'::tracking.exam_status
    END;
END;
$$ LANGUAGE plpgsql IMMUTABLE;

CREATE CAST ( INTEGER AS tracking.exam_status )
WITH FUNCTION tracking.map_integer_exam_status(integer)
AS ASSIGNMENT;

-- content.subject_name
CREATE OR REPLACE FUNCTION content.map_integer_subject(subject_int integer)
    RETURNS content.subject_name AS $$
BEGIN
    RETURN CASE subject_int
        WHEN 0 THEN 'linguagens'::content.subject_name
        WHEN 1 THEN 'ciencias-humanas'::content.subject_name
        WHEN 2 THEN 'ciencias-natureza'::content.subject_name
        WHEN 3 THEN 'matematica'::content.subject_name
        ELSE 'linguagens'::content.subject_name
    END;
END;
$$ LANGUAGE plpgsql IMMUTABLE;

CREATE CAST ( INTEGER AS content.subject_name )
WITH FUNCTION content.map_integer_subject(integer)
AS ASSIGNMENT;

-- CREATE TYPE auth.color_scheme AS ENUM('DARK', 'LIGHT', 'OS');
CREATE OR REPLACE FUNCTION auth.map_integer_color_scheme(color_scheme_int integer)
    RETURNS auth.color_scheme AS $$
BEGIN
    RETURN CASE color_scheme_int
        WHEN 0 THEN 'OS'::auth.color_scheme
        WHEN 1 THEN 'DARK'::auth.color_scheme
        WHEN 2 THEN 'LIGHT'::auth.color_scheme
        ELSE 'OS'::auth.color_scheme
    END;
END;
$$ LANGUAGE plpgsql IMMUTABLE;

CREATE CAST ( INTEGER AS auth.color_scheme )
WITH FUNCTION auth.map_integer_color_scheme(integer)
AS ASSIGNMENT;