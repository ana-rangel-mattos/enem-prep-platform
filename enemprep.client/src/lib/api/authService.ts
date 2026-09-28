import {
  ILoginUser,
  IRegisterUser,
  IUserResponse,
} from "@/lib/types/user.types";
import { ApiClient } from "@/lib/api/apiClient";

class AuthService extends ApiClient {
  constructor() {
    super("http://localhost:5265/api/auth/");
  }

  registerUser(request: IRegisterUser): Promise<void> {
    return this.post("register", request);
  }

  loginUser(request: ILoginUser): Promise<void> {
    return this.post("login", request);
  }
}

export const authService = new AuthService();
