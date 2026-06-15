export interface IUser {
  userId: string;
  fullName: string;
  username: string;
  dateOfBirth: Date;
  email: string;
  roles: string[];
  permissions: string[];
}

export interface IRegisterUser {
  fullName: string;
  username: string;
  email: string;
  password: string;
  isPrivate?: boolean;
  dateOfBirth: string;
  profileImage?: string;
  code?: string;
}

export interface ILoginUser {
  username?: string;
  email?: string;
  password: string;
}
