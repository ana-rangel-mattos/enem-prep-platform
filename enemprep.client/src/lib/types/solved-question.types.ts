export interface ISolvedQuestionResponse {
  solvedQuestionId: string;
  questionId: string;
  questionYear: number;
  correctAlternative: string;
  chosenAlternative?: string;
  questionText?: string;
  correctAlternativeText?: string;
  chosenAlternativeText?: string;
  timeSpent: number;
  isCorrect: boolean;
  createdAt: string;
}

export interface ISolvedQuestionPostRequest {
  questionId: string;
  questionYear: number;
  correctAlternative: string;
  chosenAlternative?: string;
  startedAt: string;
  finishedAt: string;
}