import { ApiClient } from "@/lib/api/apiClient";
import {
  ISolvedQuestionPostRequest,
  ISolvedQuestionResponse,
} from "@/lib/types/solved-question.types";

type QuestionParams = Omit<ISearchParams, "searchTerm">;

class SolvedQuestionsService extends ApiClient {
  constructor() {
    super("SolvedQuestions");
  }

  getSolvedQuestions(
    params: QuestionParams,
  ): Promise<IPagedResponse<ISolvedQuestionResponse>> {
    return this.get("", params);
  }

  postAnswer(request: ISolvedQuestionPostRequest): Promise<string> {
    return this.post("/solve", request);
  }

  getSolvedQuestionById(
    solvedQuestionId: string,
  ): Promise<ISolvedQuestionResponse> {
    return this.get(`/${solvedQuestionId}`);
  }

  getSolvedQuestionsByUserId(
    userId: string,
    params: QuestionParams,
  ): Promise<IPagedResponse<ISolvedQuestionResponse>> {
    return this.get(`/user/${userId}`, params);
  }
}

export const solvedQuestionsService = new SolvedQuestionsService();
