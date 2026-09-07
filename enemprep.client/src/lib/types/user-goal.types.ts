export interface IUserGoalResponse {
  userId: string;
  userGoalId: string;
  universityName: string;
  courseName: string;
  cutOffScore: number;
}

export interface IUserGoalCreateUpdate {
  universityName: string;
  courseName: string;
  cutOffScore: number;
}
