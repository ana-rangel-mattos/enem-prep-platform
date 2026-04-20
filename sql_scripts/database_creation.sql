CREATE SCHEMA IF NOT EXISTS auth;
CREATE SCHEMA IF NOT EXISTS content;
CREATE SCHEMA IF NOT EXISTS tracking;
CREATE SCHEMA IF NOT EXISTS planning;

-- ENUMS
CREATE TYPE planning.day_of_the_week AS ENUM('MONDAY', 'TUESDAY', 'WEDNESDAY', 'THURSDAY', 'FRIDAY', 'SUNDAY', 'SATURDAY');
CREATE TYPE content.language AS ENUM('INGLES', 'ESPANHOL');
CREATE TYPE tracking.exam_status AS ENUM('NOT_STARTED', 'IN_PROGRESS', 'FINISHED', 'CANCELED');
CREATE TYPE content.subject_name AS ENUM('linguagens', 'ciencias-humanas', 'ciencias-natureza', 'matematica');
CREATE TYPE auth.color_scheme AS ENUM('DARK', 'LIGHT', 'OS');

-- USER
CREATE TABLE IF NOT EXISTS auth.user (
    user_id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    full_name VARCHAR(255) NOT NULL,
    username VARCHAR(20) UNIQUE NOT NULL,
    role auth.user_role DEFAULT 'STUDENT' NOT NULL,
    date_of_birth TIMESTAMP NOT NULL,
    email VARCHAR(320) NOT NULL,
    hash_password TEXT NOT NULL,
    is_private BOOL DEFAULT false NOT NULL,
    profile_image BYTEA,
    updated_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW() NOT NULL
);

-- ALTER TABLE auth.user DROP COLUMN role;
-- ALTER TABLE auth.user DROP COLUMN streak_count;
-- ALTER TABLE auth.user DROP COLUMN last_login_date;

CREATE TABLE IF NOT EXISTS auth.sessions (
    "Id" TEXT PRIMARY KEY,
    "Value" BYTEA NOT NULL,
    "ExpiresAtTime" TIMESTAMPTZ NOT NULL,
    "SlidingExpirationInSeconds" BIGINT NULL,
    "AbsoluteExpiration" TIMESTAMPTZ NULL
);

CREATE TABLE IF NOT EXISTS tracking.user_profile (
    user_id UUID PRIMARY KEY REFERENCES auth.user(user_id) ON DELETE CASCADE,
    user_bio VARCHAR(400),
    streak_count INTEGER DEFAULT 0 NOT NULL,
    experience_points INTEGER DEFAULT 0 NOT NULL,
    current_level INTEGER DEFAULT 1 NOT NULL,
    last_activity_date TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    updated_at TIMESTAMPTZ DEFAULT NOW() NOT NULL
);

CREATE TABLE IF NOT EXISTS auth.user_preferences (
    user_preferences_id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    user_id UUID NOT NULL,
    questions_per_day INTEGER DEFAULT 10 NOT NULL,
    exam_language content.language DEFAULT 'INGLES' NOT NULL,
    color_scheme auth.color_scheme DEFAULT 'OS' NOT NULL,
    updated_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    CONSTRAINT fk_user_id FOREIGN KEY (user_id)
        REFERENCES auth.user(user_id)
        ON DELETE CASCADE
        ON UPDATE NO ACTION
);

CREATE TABLE IF NOT EXISTS planning.user_goal (
    user_goal_id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    user_id UUID NOT NULL,
    university_name VARCHAR(255) NOT NULL,
    course_name VARCHAR(255) NOT NULL,
    cut_off_score FLOAT4 NOT NULL,
    updated_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    CONSTRAINT fk_user_id FOREIGN KEY (user_id)
        REFERENCES auth.user(user_id)
        ON DELETE CASCADE
        ON UPDATE NO ACTION,
    CONSTRAINT unique_goal UNIQUE (user_id)
);

-- SUBJECTS | SCHEDULES

CREATE TABLE IF NOT EXISTS content.subject (
    subject_id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    name content.subject_name NOT NULL,
    updated_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW() NOT NULL
);

CREATE TABLE IF NOT EXISTS planning.schedule (
    schedule_id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    user_id UUID NOT NULL,
    name VARCHAR(50) DEFAULT 'Cronograma Semanal' NOT NULL,
    updated_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    CONSTRAINT fk_user_id FOREIGN KEY (user_id)
        REFERENCES auth.user(user_id)
        ON DELETE CASCADE
        ON UPDATE NO ACTION
);

CREATE TABLE IF NOT EXISTS planning.schedule_subject (
    schedule_id UUID REFERENCES planning.schedule(schedule_id)
        ON DELETE CASCADE ON UPDATE NO ACTION,
    subject_id UUID REFERENCES content.subject(subject_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    weekday planning.day_of_the_week NOT NULL,
    subject_order INTEGER NOT NULL,
    updated_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    CONSTRAINT pk_schedule_subject PRIMARY KEY (schedule_id,subject_id)
);

-- EXAMS

CREATE TABLE IF NOT EXISTS tracking.exam (
    exam_id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    user_id UUID NOT NULL,
    title VARCHAR(20) NOT NULL,
    exam_year INTEGER NOT NULL,
    language_choice content.language NOT NULL,
    status tracking.exam_status DEFAULT 'NOT_STARTED' NOT NULL,
    questions_count INTEGER NOT NULL,
    correct_questions_count INTEGER,
    incorrect_questions_count INTEGER,
    unsolved_questions_count INTEGER,
    total_spent_time interval,
    max_spent_time interval NOT NULL,
    estimated_score FLOAT4,
    updated_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    CONSTRAINT fk_user_id FOREIGN KEY (user_id)
        REFERENCES auth.user(user_id)
        ON DELETE CASCADE ON UPDATE NO ACTION
);

CREATE TABLE IF NOT EXISTS tracking.exam_subject (
    subject_id UUID REFERENCES content.subject(subject_id)
        ON DELETE SET NULL ON UPDATE NO ACTION,
    exam_id UUID REFERENCES tracking.exam(exam_id)
        ON DELETE CASCADE ON UPDATE NO ACTION,
    updated_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    CONSTRAINT pk_exam_subject PRIMARY KEY (subject_id, exam_id)
);

-- QUESTION
CREATE TABLE IF NOT EXISTS content.question (
    question_id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    subject_id UUID,
    posted_by_id UUID NOT NULL,
    api_index INTEGER NOT NULL,
    language content.language DEFAULT null,
    content jsonb NOT NULL,
    updated_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    CONSTRAINT fk_subject_id FOREIGN KEY (subject_id)
        REFERENCES content.subject(subject_id)
        ON DELETE SET NULL ON UPDATE NO ACTION,
    CONSTRAINT fk_user_id FOREIGN KEY (posted_by_id)
        REFERENCES auth.user(user_id)
        ON DELETE CASCADE ON UPDATE NO ACTION
);

CREATE TABLE IF NOT EXISTS tracking.solved_question (
    solved_question_id UUID PRIMARY KEY DEFAULT GEN_RANDOM_UUID() NOT NULL,
    user_id UUID NOT NULL
        REFERENCES auth.user(user_id)
        ON DELETE CASCADE ON UPDATE NO ACTION,
    question_id UUID NOT NULL
        REFERENCES content.question(question_id)
        ON DELETE CASCADE ON UPDATE NO ACTION,
    question_year INTEGER NOT NULL,
    correct_alternative char(1) NOT NULL,
    chosen_alternative char(1),
    is_correct BOOL NOT NULL,
    time_spent interval NOT NULL,
    updated_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW() NOT NULL
);

CREATE TABLE IF NOT EXISTS tracking.exam_question (
    exam_id UUID NOT NULL
        REFERENCES tracking.exam(exam_id)
        ON DELETE CASCADE ON UPDATE NO ACTION,
    question_id UUID NOT NULL
        REFERENCES content.question(question_id)
        ON DELETE CASCADE ON UPDATE NO ACTION,
    correct_alternative char(1) NOT NULL,
    chosen_alternative char(1),
    is_correct BOOL NOT NULL,
    time_spent interval NOT NULL,
    updated_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    CONSTRAINT exam_question_pk PRIMARY KEY (exam_id, question_id)
);
