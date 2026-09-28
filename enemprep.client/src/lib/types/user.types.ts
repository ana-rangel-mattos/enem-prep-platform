export interface IUserResponse {
  userId: string;
  fullName: string;
  username: string;
  dateOfBirth: Date;
  email: string;
  imageUrl?: string;
  attemptedQuestions: number;
  correctAnswers: number;
  attemptedExams: number;
  averageScore: number;
  highestScore: number;
  successRate: number;
  averageTimeSpent: string;
  hasGoal: boolean;
  roles: string[];
  permissions: string[];
  createdAt: string;
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

export interface IUserAccountResponse {
  fullName: string;
  email: string;
  username: string;
  isPrivate: boolean;
  imageUrl?: string;
}

export interface IUpdateUserAccountRequest {
  fullName?: string;
  email?: string;
  username?: string;
  isPrivate?: boolean;
  profileImage?: string;
  roleInviteCode?: string;
  confirmationPassword: string;
}
