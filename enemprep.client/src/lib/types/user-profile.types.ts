export interface IUserProfileResponse {
  userBio?: string;
  streakCount: number;
  experiencePoints: number;
  currentLevel: number;
  lastActivityDate: string;
}

export interface IUpdateUserProfileBio {
  userBio?: string;
}
