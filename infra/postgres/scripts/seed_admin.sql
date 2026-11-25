-- 1. Inserir o primeiro Usuário (Admin Root)
INSERT INTO auth.user (
    user_id,
    full_name,
    username,
    email,
    hash_password,
    date_of_birth,
    is_private,
    created_at,
    updated_at
) VALUES (
             'F1181460-9376-4BB7-9E3A-B4130FD17BF8',
             'Administrador Root',
             'admin_root',
             'admin@example.com',
             '$2a$12$R9h/lIPzHZ7vGTEyv.S5BeH5A.8bW.fXp9Wb8fU6XW5v7b8fU6XW5',
             '2000-01-01',
             false,
             NOW(),
             NOW()
         );

-- 2. Atribuir Role 'Admin' para o primeiro usuário
INSERT INTO auth."RoleUser" ("RolesId", "UsersUserId")
VALUES (2, 'F1181460-9376-4BB7-9E3A-B4130FD17BF8');

-- 3. Gerar o primeiro código de convite
INSERT INTO auth.invitation_code (
    invitation_code_id,
    "CreatedById",
    "RoleId",
    "Code",
    is_used,
    "ExpiresAt",
    updated_at,
    created_at
) VALUES (
             gen_random_uuid(),
             'F1181460-9376-4BB7-9E3A-B4130FD17BF8',
             2,
             'ENEM-ROOT-2026',
             false,
             NOW() + INTERVAL '30 days',
             NOW(),
             NOW()
         );

INSERT INTO content.subject (subject_id, name)
VALUES ('25da4bd2-1bc7-4d4b-a448-23a2aa12a70b', 'linguagens'),
       ('3714344b-f6cf-49b6-85de-cb659ad54850', 'ciencias-humanas'),
       ('20e9711f-fe09-474e-a25a-0b3d163c61b5', 'ciencias-natureza'),
       ('6f1d7c77-1f8b-44c7-b409-7d7c20c328c9', 'matematica');
