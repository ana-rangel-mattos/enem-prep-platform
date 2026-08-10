export interface IQuestionResponse {
  questionId: string;
  apiIndex: number;
  content: {
    year: number;
    files: string[];
    index: number;
    title: string;
    context: string;
    language: null | string;
    discipline: string;
    alternatives: IQuestionAlternative[];
    correctAlternative: string;
    alternativesIntroduction: string;
  };
  subjectId: string;
  createdAt: string;
  updatedAt: string;
}

export interface IQuestionAlternative {
  file: null | string;
  text: string;
  letter: string;
  isCorrect: boolean;
}
