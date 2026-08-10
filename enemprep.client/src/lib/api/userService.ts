import { ApiClient } from "@/lib/api/apiClient";
import { IUser } from "@/lib/types/user.types";

class UserService extends ApiClient {
  constructor() {
    super("http://localhost:5265/api/user/");
  }

  getLoggedUser(): Promise<IUser> {
    return this.get("me")
  }

  getUserById(userId: string): Promise<IUser> {
    return this.get(`${userId}`)
  }

  deleteUserById(userId: string): Promise<void> {
    return this.delete(`${userId}`);
  }
}

export const userService = new UserService();