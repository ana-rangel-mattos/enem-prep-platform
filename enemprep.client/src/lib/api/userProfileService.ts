import { ApiClient } from "@/lib/api/apiClient";
import {
  IUpdateUserProfileBio,
  IUserProfileResponse,
} from "@/lib/types/user-profile.types";

class UserProfileService extends ApiClient {
  constructor() {
    super("http://localhost:5265/api/UserProfile/");
  }

  getProfile(): Promise<IUserProfileResponse> {
    return this.get("");
  }

  updateUserBio(request: IUpdateUserProfileBio): Promise<void> {
    return this.put("", request)
  }
}

export const userProfileService = new UserProfileService();
