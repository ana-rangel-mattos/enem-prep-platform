import { ApiClient } from "@/lib/api/apiClient";
import {
  IUserGoalCreateUpdate,
  IUserGoalResponse,
} from "@/lib/types/user-goal.types";

class UserGoalService extends ApiClient {
  constructor() {
    super("UserGoal/");
  }

  getUserGoal(): Promise<IUserGoalResponse> {
    return this.get("");
  }

  createUserGoal(request: IUserGoalCreateUpdate): Promise<void> {
    return this.post("new", request);
  }

  updateUserGoal(request: IUserGoalCreateUpdate): Promise<void> {
    return this.put("", request);
  }

  deleteUserGoal(): Promise<void> {
    return this.delete("");
  }
}

export const userGoalService = new UserGoalService();
