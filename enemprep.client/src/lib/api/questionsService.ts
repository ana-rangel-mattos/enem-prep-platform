import { ApiClient } from "@/lib/api/apiClient";
import { IQuestionResponse } from "@/lib/types/question.interface";

class QuestionsService extends ApiClient {
  constructor() {
    super("http://localhost:5265/api/questions/");
  }

  getQuestions(
    params: ISearchParams,
  ): Promise<IPagedResponse<IQuestionResponse>> {
    return this.get("", params);
  }

  getQuestionById(id: string): Promise<IQuestionResponse> {
    return this.get(id);
  }

  createQuestion(request: object): Promise<void> {
    return this.post("new", request);
  }
}

export const questionsService = new QuestionsService();
