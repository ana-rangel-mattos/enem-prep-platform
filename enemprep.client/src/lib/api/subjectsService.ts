import { ApiClient } from "@/lib/api/apiClient";
import {
  IGetSubjectResponse,
  IPostSubjectRequest,
} from "@/lib/types/subjects.types";

class SubjectsService extends ApiClient {
  constructor() {
    super("subjects/");
  }

  GetSubjects(): Promise<IGetSubjectResponse[]> {
    return this.get("");
  }

  GetSubjectById(subjectId: string): Promise<IGetSubjectResponse> {
    return this.get(`${subjectId}`);
  }

  CreateSubject(request: IPostSubjectRequest): Promise<void> {
    return this.post("", request);
  }

  DeleteSubject(subjectId: string): Promise<void> {
    return this.delete(`${subjectId}`);
  }
}

export const subjectsService = new SubjectsService();
