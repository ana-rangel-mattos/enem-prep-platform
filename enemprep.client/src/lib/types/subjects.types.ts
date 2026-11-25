import { SubjectName } from "@/lib/types/enums.types";

export interface IPostSubjectRequest {
  subjectName: SubjectName;
}

export interface IGetSubjectResponse {
  subjectId: string;
  name: string;
}
