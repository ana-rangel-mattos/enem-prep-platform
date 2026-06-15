import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "@/lib/store";
import { IUser } from "@/lib/types/user.interface";

interface AuthState {
    user?: IUser;
    isAuthenticated: boolean;
}

const initialState: AuthState = {
    isAuthenticated: false,
}

export const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
        setUser: (state, action: PayloadAction<IUser>) => {
            state.user = action.payload;
            state.isAuthenticated = true;
        },
        logout: state => {
            state.user = undefined;
            state.isAuthenticated = false;
        }
    }
})

export const { setUser, logout } = authSlice.actions;

export const selectUser = (state: RootState) : IUser | undefined => state.auth.user;
export const selectIsAuthenticated = (state: RootState): boolean => state.auth.isAuthenticated;

export default authSlice.reducer;