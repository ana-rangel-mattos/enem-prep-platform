CREATE OR REPLACE FUNCTION tracking.update_user_streak()
RETURNS TRIGGER AS $$
DECLARE last_activity DATE;
BEGIN
    SELECT last_activity_date::DATE INTO last_activity
    FROM tracking.user_profile
    WHERE user_id = NEW.user_id;

    IF NOT FOUND THEN
        INSERT INTO tracking.user_profile (user_id, streak_count, experience_points, last_activity_date)
        VALUES (NEW.user_id, 1, 10, NOW());
        RETURN NEW;
    END IF;

    IF last_activity = CURRENT_DATE THEN
        NULL;
    ELSIF last_activity = CURRENT_DATE - interval '1 day' THEN
        UPDATE tracking.user_profile
        SET
            streak_count = streak_count + 1,
            last_activity_date = now()
        WHERE user_id = NEW.user_id;
    ELSE
        UPDATE tracking.user_profile
        SET
            streak_count = 1,
            last_activity_date = NOW()
        WHERE user_id = NEW.user_id;
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER track_user_streak_solved_question
AFTER INSERT ON tracking.solved_question
    FOR EACH ROW EXECUTE FUNCTION tracking.update_user_streak();

CREATE TRIGGER track_user_streak_exam_question
    AFTER INSERT ON tracking.exam_question
    FOR EACH ROW EXECUTE FUNCTION tracking.update_user_streak();