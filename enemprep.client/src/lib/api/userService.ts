import { ApiClient } from "@/lib/api/apiClient";
import {
  IUpdateUserAccountRequest,
  IUserAccountResponse,
  IUserResponse,
} from "@/lib/types/user.types";

class UserService extends ApiClient {
  constructor() {
    super("user/");
  }

  getLoggedUser(): Promise<IUserResponse> {
    return this.get("me");
  }

  getUserById(userId: string): Promise<IUserResponse> {
    return this.get(`${userId}`);
  }

  deleteUserById(userId: string): Promise<void> {
    return this.delete(`${userId}`);
  }

  getLoggedUserAccount(): Promise<IUserAccountResponse> {
    return this.get("me/account");
  }

  updateLoggedUserAccount(request: IUpdateUserAccountRequest): Promise<void> {
    return this.put("me/account", request);
  }
}

export const userService = new UserService();
