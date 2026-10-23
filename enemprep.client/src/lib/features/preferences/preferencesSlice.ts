import { IUserPreferencesResponse } from "@/lib/types/user-preferences.types";
import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "@/lib/store";
import { ColorScheme, ExamLanguage } from "@/lib/types/enums.types";

export interface PreferencesState {
  userPreferencesId?: string;
  userId?: string;
  questionsPerDay?: number;
  examLanguage?: ExamLanguage;
  colorScheme: ColorScheme;
}

const initialState: PreferencesState = { colorScheme: ColorScheme.OS };

export const preferencesSlice = createSlice({
  name: "preferences",
  initialState,
  reducers: {
    setPreferences: (
      state,
      action: PayloadAction<IUserPreferencesResponse>,
    ) => {
      state.userPreferencesId = action.payload.userPreferencesId;
      state.userId = action.payload.userId;
      state.questionsPerDay = action.payload.questionsPerDay;
      state.examLanguage = action.payload.examLanguage;
      state.colorScheme = action.payload.colorScheme;
    },
    resetPreferences: (state) => {
      state.userPreferencesId = undefined;
      state.userId = undefined;
      state.questionsPerDay = undefined;
      state.colorScheme = ColorScheme.OS;
      state.examLanguage = undefined;
    },
  },
});

export const { setPreferences, resetPreferences } = preferencesSlice.actions;

export const selectLanguage = (state: RootState): ExamLanguage | void =>
  state.preferences.examLanguage;

export const selectColorScheme = (state: RootState): ColorScheme =>
  state.preferences.colorScheme;

export const selectQuestionsPerDay = (state: RootState): number | void =>
  state.preferences.questionsPerDay;

export default preferencesSlice.reducer;
