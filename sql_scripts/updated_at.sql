CREATE OR REPLACE FUNCTION update_updated_at()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ language plpgsql;

CREATE TRIGGER user_updated_at
BEFORE UPDATE ON auth."user"
    FOR EACH ROW EXECUTE FUNCTION update_updated_at();

CREATE TRIGGER user_preferences_updated_at
BEFORE UPDATE ON auth.user_preferences
    FOR EACH ROW EXECUTE FUNCTION update_updated_at();

CREATE TRIGGER user_goal_updated_at
    BEFORE UPDATE ON planning.user_goal
    FOR EACH ROW EXECUTE FUNCTION update_updated_at();

CREATE TRIGGER subject_updated_at
    BEFORE UPDATE ON content.subject
    FOR EACH ROW EXECUTE FUNCTION update_updated_at();

CREATE TRIGGER question_updated_at
    BEFORE UPDATE ON content.question
    FOR EACH ROW EXECUTE FUNCTION update_updated_at();

CREATE TRIGGER schedule_updated_at
    BEFORE UPDATE ON planning.schedule
    FOR EACH ROW EXECUTE FUNCTION update_updated_at();

CREATE TRIGGER schedule_subject_updated_at
    BEFORE UPDATE ON planning.schedule_subject
    FOR EACH ROW EXECUTE FUNCTION update_updated_at();

CREATE TRIGGER exam_updated_at
    BEFORE UPDATE ON tracking.exam
    FOR EACH ROW EXECUTE FUNCTION update_updated_at();

CREATE TRIGGER exam_subject_updated_at
    BEFORE UPDATE ON tracking.exam_subject
    FOR EACH ROW EXECUTE FUNCTION update_updated_at();

CREATE TRIGGER exam_question_updated_at
    BEFORE UPDATE ON tracking.exam_question
    FOR EACH ROW EXECUTE FUNCTION update_updated_at();

CREATE TRIGGER solved_question_updated_at
    BEFORE UPDATE ON tracking.solved_question
    FOR EACH ROW EXECUTE FUNCTION update_updated_at();
