ALTER TABLE auth.user_preferences ALTER COLUMN color_scheme TYPE TEXT;
ALTER TABLE auth.user_preferences ALTER COLUMN exam_language TYPE TEXT;
ALTER TABLE content.subject ALTER COLUMN name TYPE TEXT;
ALTER TABLE content.question ALTER COLUMN language TYPE TEXT;
ALTER TABLE tracking.exam ALTER COLUMN language_choice TYPE TEXT;
ALTER TABLE tracking.exam ALTER COLUMN status TYPE TEXT;
ALTER TABLE planning.schedule_subject ALTER COLUMN weekday TYPE TEXT;
