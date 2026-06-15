import { ILoginUser, IRegisterUser, IUser } from "@/lib/types/user.interface";
import { ApiClient } from "@/lib/api/apiClient";

class AuthService extends ApiClient {
  constructor() {
    super("http://localhost:5265/api/auth/");
  }

  getLoggedUser(): Promise<IUser> {
    return this.get("me");
  }

  registerUser(request: IRegisterUser): Promise<void> {
    return this.post("register", request);
  }

  loginUser(request: ILoginUser): Promise<void> {
    return this.post("login", request);
  }
}

export const authService = new AuthService();
