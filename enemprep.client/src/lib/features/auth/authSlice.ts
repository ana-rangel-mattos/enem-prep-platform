import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "@/lib/store";

type UserProfile = {
    userId: string;
    username: string;
    role: "ADMIN" | "STUDENT";
    permissions: string[];
}

interface AuthState {
    user?: UserProfile;
    isAuthenticated: boolean;
    isLoading: boolean;
    login?: (userData: UserProfile) => void;
    logout?: () => void;
}

const initialState: AuthState = {
    isAuthenticated: false,
    isLoading: false,
}

export const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
        loadUser: state => {}
    }
})

export default authSlice.reducer;