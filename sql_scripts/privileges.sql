-- Roles
CREATE ROLE enem_user_role;
CREATE ROLE enem_admin_role;

-- Student Role
GRANT USAGE ON SCHEMA auth, planning, content, tracking TO enem_user_role;
GRANT SELECT ON ALL TABLES IN SCHEMA content TO enem_user_role;
GRANT SELECT, UPDATE ON auth."user" TO enem_user_role;
GRANT SELECT, INSERT, UPDATE ON auth.user_preferences TO enem_user_role;
GRANT SELECT, INSERT, UPDATE ON ALL TABLES IN SCHEMA tracking TO enem_user_role;

-- Admin Role
GRANT enem_user_role TO enem_admin_role;
GRANT INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA content TO enem_admin_role;
GRANT INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA auth TO enem_admin_role;

-- Users
CREATE USER enem_user;
CREATE USER enem_admin;

GRANT enem_user_role TO enem_user;
GRANT enem_admin_role TO enem_admin;
