import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "@/lib/store";
import { IUserResponse } from "@/lib/types/user.types";
import { logoutDeleteCookies } from "@/lib/actions";

interface AuthState {
  user?: IUserResponse;
  isAuthenticated: boolean;
}

const initialState: AuthState = { isAuthenticated: false };

export const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setUser: (state, action: PayloadAction<IUserResponse>) => {
      state.user = action.payload;
      state.isAuthenticated = true;
    },
    logout: (state) => {
      state.user = undefined;
      state.isAuthenticated = false;
      logoutDeleteCookies();
    },
  },
});

export const { setUser, logout } = authSlice.actions;

export const selectUser = (state: RootState): IUserResponse | undefined =>
  state.auth.user;
export const selectIsAuthenticated = (state: RootState): boolean =>
  state.auth.isAuthenticated;

export default authSlice.reducer;
