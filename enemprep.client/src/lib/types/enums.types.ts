export enum SubjectName {
  Languages = 0,
  Humanities = 1,
  NaturalSciences = 2,
  Mathematics = 3,
}

export const SUBJECT_OPTIONS = [
  { id: SubjectName.Languages, label: "Linguagens" },
  { id: SubjectName.Humanities, label: "Humanidades" },
  { id: SubjectName.NaturalSciences, label: "Natureza" },
  { id: SubjectName.Mathematics, label: "Matemática" },
] as const;

export enum ExamLanguage {
  English = 0,
  Spanish = 1,
}

export enum ExamStatus {
  NotStarted = 0,
  InProgress = 1,
  Finished = 2,
  Canceled = 3,
}

export enum ColorScheme {
  Dark = 0,
  Light = 1,
  OS = 2,
}

export enum DayOfTheWeek {
  Monday = 0,
  Tuesday = 1,
  Wednesday = 2,
  Thursday = 3,
  Friday = 4,
  Saturday = 5,
  Sunday = 6,
}
