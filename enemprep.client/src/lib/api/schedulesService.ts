import { ApiClient } from "@/lib/api/apiClient";
import {
  IGetScheduleDetailsResponse,
  IGetScheduleResponse,
  IPostScheduleRequest,
  IPostScheduleSubjectRequest,
  IPutScheduleOrderRequest,
  IPutScheduleRequest,
} from "@/lib/types/schedule.types";

class SchedulesService extends ApiClient {
  constructor() {
    super("http://localhost:5265/api/schedules/");
  }

  GetAllSchedules(): Promise<IGetScheduleResponse[]> {
    return this.get("");
  }

  GetScheduleById(scheduleId: string): Promise<IGetScheduleDetailsResponse> {
    return this.get(`${scheduleId}`);
  }

  PostSubject(
    scheduleId: string,
    subject: IPostScheduleSubjectRequest,
  ): Promise<void> {
    return this.post(`${scheduleId}/subjects/new`, subject);
  }

  PostSchedule(subject: IPostScheduleRequest): Promise<void> {
    return this.post(`/new`, subject);
  }

  DeleteSchedule(scheduleId: string): Promise<void> {
    return this.delete(`${scheduleId}`);
  }

  DeleteScheduleSubject(scheduleId: string, subjectId: string): Promise<void> {
    return this.delete(`${scheduleId}/subjects/${subjectId}`);
  }

  UpdateSchedule(
    scheduleId: string,
    newSchedule: IPutScheduleRequest,
  ): Promise<void> {
    return this.put(`${scheduleId}/update`, newSchedule);
  }

  UpdateScheduleOrder(
    scheduleId: string,
    newScheduleOrder: IPutScheduleOrderRequest,
  ): Promise<void> {
    return this.put(`${scheduleId}/reorder`, newScheduleOrder);
  }
}

export const schedulesService = new SchedulesService();
