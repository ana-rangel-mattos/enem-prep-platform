import { ExamLanguage, ExamStatus, SubjectName } from "@/lib/types/enums.types";
import { IQuestionAlternative } from "@/lib/types/question.types";

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
  examId: string;
  questionId: string;
  content: IExamQuestionContent;
}

interface IExamQuestionContent {
  year: number;
  files: string[];
  index: number;
  title: string;
  context: string;
  language: null | string;
  discipline: string;
  alternatives: Omit<IQuestionAlternative, "isCorrect">[];
  alternativesIntroduction: string;
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
