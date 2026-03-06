CREATE SCHEMA IF NOT EXISTS auth;
CREATE SCHEMA IF NOT EXISTS content;
CREATE SCHEMA IF NOT EXISTS tracking;
CREATE SCHEMA IF NOT EXISTS planning;

-- ENUMS
CREATE TYPE auth.user_role AS ENUM('STUDENT', 'ADMIN');
CREATE TYPE planning.day_of_the_week AS ENUM('MONDAY', 'TUESDAY', 'WEDNESDAY', 'THURSDAY', 'FRIDAY', 'SUNDAY', 'SATURDAY');
CREATE TYPE content.language AS ENUM('INGLES', 'ESPANHOL');
CREATE TYPE tracking.exam_status AS ENUM('NOT_STARTED', 'IN_PROGRESS', 'FINISHED', 'CANCELED');
CREATE TYPE content.subject_name AS ENUM('linguagens', 'ciencias-humanas', 'ciencias-natureza', 'matematica');
CREATE TYPE auth.color_scheme AS ENUM('DARK', 'LIGHT', 'OS');

-- USER
CREATE TABLE IF NOT EXISTS auth.user (
    user_id uuid primary key default gen_random_uuid() not null,
    full_name varchar(255) not null,
    username varchar(20) unique not null,
    role auth.user_role default 'STUDENT' not null,
    date_of_birth timestamp not null,
    email varchar(320) not null,
    hash_password text not null,
    is_private bool default false not null,
    streak_count integer default 0 not null,
    last_login_date timestamptz not null,
    profile_image bytea,
    updated_at timestamptz default now() not null,
    created_at timestamptz default now() not null
);

CREATE TABLE IF NOT EXISTS auth.user_preferences (
    user_preferences_id uuid primary key default gen_random_uuid() not null,
    user_id uuid not null,
    questions_per_day integer default 10 not null,
    exam_language content.language default 'INGLES' not null,
    color_scheme auth.color_scheme default 'OS' not null,
    updated_at timestamptz default now() not null,
    created_at timestamptz default now() not null,
    CONSTRAINT fk_user_id FOREIGN KEY (user_id)
        REFERENCES auth.user(user_id)
        ON DELETE CASCADE
        ON UPDATE NO ACTION
);

CREATE TABLE IF NOT EXISTS planning.user_goal (
    user_goal_id uuid primary key default gen_random_uuid() not null,
    user_id uuid not null,
    university_name varchar(255) not null,
    course_name varchar(255) not null,
    cut_off_score float4 not null,
    updated_at timestamptz default now() not null,
    created_at timestamptz default now() not null,
    CONSTRAINT fk_user_id FOREIGN KEY (user_id)
        REFERENCES auth.user(user_id)
        ON DELETE CASCADE
        ON UPDATE NO ACTION,
    CONSTRAINT unique_goal UNIQUE (user_id)
);

-- SUBJECTS | SCHEDULES

CREATE TABLE IF NOT EXISTS content.subject (
    subject_id uuid primary key default gen_random_uuid() not null,
    name content.subject_name not null,
    updated_at timestamptz default now() not null,
    created_at timestamptz default now() not null
);

CREATE TABLE IF NOT EXISTS planning.schedule (
    schedule_id uuid primary key default gen_random_uuid() not null,
    user_id uuid not null,
    name varchar(50) default 'Cronograma Semanal' not null,
    updated_at timestamptz default now() not null,
    created_at timestamptz default now() not null,
    CONSTRAINT fk_user_id FOREIGN KEY (user_id)
        REFERENCES auth.user(user_id)
        ON DELETE CASCADE
        ON UPDATE NO ACTION
);

CREATE TABLE IF NOT EXISTS planning.schedule_subject (
    schedule_id uuid REFERENCES planning.schedule(schedule_id)
        ON DELETE CASCADE ON UPDATE NO ACTION,
    subject_id uuid REFERENCES content.subject(subject_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    weekday planning.day_of_the_week not null,
    subject_order integer not null,
    updated_at timestamptz default now() not null,
    created_at timestamptz default now() not null,
    CONSTRAINT pk_schedule_subject PRIMARY KEY (schedule_id,subject_id)
);

-- EXAMS

CREATE TABLE IF NOT EXISTS tracking.exam (
    exam_id uuid primary key default gen_random_uuid() not null,
    user_id uuid not null,
    title varchar(20) not null,
    exam_year integer not null,
    language_choice content.language not null,
    status tracking.exam_status default 'NOT_STARTED' not null,
    questions_count integer not null,
    correct_questions_count integer,
    incorrect_questions_count integer,
    unsolved_questions_count integer,
    total_spent_time interval,
    max_spent_time interval not null,
    estimated_score float4,
    updated_at timestamptz default now() not null,
    created_at timestamptz default now() not null,
    CONSTRAINT fk_user_id FOREIGN KEY (user_id)
        REFERENCES auth.user(user_id)
        ON DELETE CASCADE ON UPDATE NO ACTION
);

CREATE TABLE IF NOT EXISTS tracking.exam_subject (
    subject_id uuid REFERENCES content.subject(subject_id)
        ON DELETE SET NULL ON UPDATE NO ACTION,
    exam_id uuid REFERENCES tracking.exam(exam_id)
        ON DELETE CASCADE ON UPDATE NO ACTION,
    updated_at timestamptz default now() not null,
    created_at timestamptz default now() not null,
    CONSTRAINT pk_exam_subject PRIMARY KEY (subject_id, exam_id)
);

-- QUESTION
CREATE TABLE IF NOT EXISTS content.question (
    question_id uuid primary key default gen_random_uuid() not null,
    subject_id uuid,
    posted_by_id uuid not null,
    api_index integer not null,
    language content.language default null,
    content jsonb not null,
    updated_at timestamptz default now() not null,
    created_at timestamptz default now() not null,
    CONSTRAINT fk_subject_id FOREIGN KEY (subject_id)
        REFERENCES content.subject(subject_id)
        ON DELETE SET NULL ON UPDATE NO ACTION,
    CONSTRAINT fk_user_id FOREIGN KEY (posted_by_id)
        REFERENCES auth.user(user_id)
        ON DELETE CASCADE ON UPDATE NO ACTION
);

CREATE TABLE IF NOT EXISTS tracking.solved_question (
    solved_question_id uuid primary key default gen_random_uuid() not null,
    user_id uuid not null
        REFERENCES auth.user(user_id)
        ON DELETE CASCADE ON UPDATE NO ACTION,
    question_id uuid not null
        REFERENCES content.question(question_id)
        ON DELETE CASCADE ON UPDATE NO ACTION,
    question_year integer not null,
    correct_alternative char(1) not null,
    chosen_alternative char(1),
    is_correct bool not null,
    time_spent interval not null,
    updated_at timestamptz default now() not null,
    created_at timestamptz default now() not null
);

CREATE TABLE IF NOT EXISTS tracking.exam_question (
    exam_id uuid not null
        REFERENCES tracking.exam(exam_id)
        ON DELETE CASCADE ON UPDATE NO ACTION,
    question_id uuid not null
        REFERENCES content.question(question_id)
        ON DELETE CASCADE ON UPDATE NO ACTION,
    correct_alternative char(1) not null,
    chosen_alternative char(1),
    is_correct bool not null,
    time_spent interval not null,
    updated_at timestamptz default now() not null,
    created_at timestamptz default now() not null,
    CONSTRAINT exam_question_pk PRIMARY KEY (exam_id, question_id)
);
