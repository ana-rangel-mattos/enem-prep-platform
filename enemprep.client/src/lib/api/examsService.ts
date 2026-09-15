import { ApiClient } from "@/lib/api/apiClient";
import {
  IExamCreate,
  IExamResponse,
  IExamSubmitAnswer,
  IExamTake,
  IExamUpdateStatus,
} from "@/lib/types/exams.types";

class ExamsService extends ApiClient {
  constructor() {
    super("http://localhost:5265/api/exams/");
  }

  getAllExams(params: ISearchParams): Promise<IPagedResponse<IExamResponse>> {
    return this.get("", params);
  }

  generateExam(request: IExamCreate): Promise<IExamResponse> {
    return this.post("new", request);
  }

  updateExamStatus(request: IExamUpdateStatus): Promise<void> {
    return this.put(
      `${request.examId}/update`,
      {},
      { newStatus: request.newStatus },
    );
  }

  takeExam(examId: string): Promise<IExamTake> {
    return this.get(`${examId}`);
  }

  submitAnswer(request: IExamSubmitAnswer): Promise<void> {
    return this.post(`${request.examId}/submit-answer`, request);
  }

  submitExam(examId: string): Promise<IExamResponse> {
    return this.post(`${examId}/complete`);
  }
}

export const examsService = new ExamsService();
