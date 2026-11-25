import { ApiClient } from "@/lib/api/apiClient";
import {
  IUserPreferencesResponse,
  IUserPreferencesUpdate,
} from "@/lib/types/user-preferences.types";

class UserPreferencesService extends ApiClient {
  constructor() {
    super("UserPreferences/");
  }

  getLoggedUserPreferences(): Promise<IUserPreferencesResponse> {
    return this.get("");
  }

  resetUserPreferences(): Promise<void> {
    return this.put("reset");
  }

  updateUserPreferences(request: IUserPreferencesUpdate): Promise<void> {
    return this.put("update", request);
  }
}

export const userPreferencesService = new UserPreferencesService();
