import { ColorScheme, ExamLanguage } from "@/lib/types/enums.types";

export interface IUserPreferencesResponse {
  userPreferencesId: string;
  userId: string;
  questionsPerDay: number;
  examLanguage: ExamLanguage;
  colorScheme: ColorScheme;
}

export interface IUserPreferencesUpdate {
  questionsPerDay: number;
  examLanguage: ExamLanguage;
  colorScheme: ColorScheme;
}
