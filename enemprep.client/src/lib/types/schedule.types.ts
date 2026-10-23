import { DayOfTheWeek } from "@/lib/types/enums.types";

export interface IGetScheduleResponse {
  scheduleId: string;
  name: string;
}

export interface IGetScheduleDetailsResponse {
  scheduleId: string;
  userId: string;
  name: string;
  hasWeekEnd: boolean;
  schedule: Record<DayOfTheWeek, IScheduleSubjectResponse[]>;
}

export interface IScheduleSubjectResponse {
  weekday: DayOfTheWeek;
  orderIndex: number;
}

export interface IPostScheduleRequest {
  name: string;
  hasWeekEnd: boolean;
}

export interface IPostScheduleSubjectRequest {
  subjectId: string;
  weekDay: DayOfTheWeek;
}

export interface IPutScheduleRequest {
  name: string;
  hasWeekEnd: boolean;
}

export interface IPutScheduleOrderRequest {
  subjectId: string;
  newDay: DayOfTheWeek;
  newIndex: number;
}
