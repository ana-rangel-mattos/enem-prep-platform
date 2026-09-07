import { ExamLanguage, ExamStatus, SubjectName } from "@/lib/types/enums.types";

export interface IExamResponse {
  examId: string;
  userId: string;
  title: string;
  status: string;
  statusDescription: string;
  language: string;
  languageDescription: string;
  examYear: number;
  questionCount: number;
  correctQuestionCount?: number;
  incorrectQuestionCount?: number;
  unsolvedQuestionCount?: number;
  totalSpentTime?: string;
  maxSpentTime: string;
  estimatedScore?: number;
  createdAt: string;
}

export interface IExamCreate {
  totalQuestions: number;
  areas: SubjectName[];
  language?: ExamLanguage;
}

export interface IExamUpdateStatus {
  examId: string;
  newStatus: ExamStatus;
}

export interface IExamQuestion {
  examQuestionId: string;
  year: number;
  context: string;
  enunciation: string;
  alternatives: Record<string, string>;
}

export interface IExamTake {
  examId: string;
  title: string;
  questions: IExamQuestion[];
  maxSpentTime: number;
}

export interface IExamSubmitAnswer {
  questionId: string;
  examId: string;
  chosenAlternative?: string;
  timeSpentInSeconds: number;
}
