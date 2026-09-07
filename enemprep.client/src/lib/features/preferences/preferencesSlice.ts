import {
  ColorScheme,
  IUserPreferencesResponse,
} from "@/lib/types/user-preferences.types";
import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "@/lib/store";
import { ExamLanguage } from "@/lib/types/exams.types";

export interface PreferencesState {
  userPreferencesId?: string;
  userId?: string;
  questionsPerDay?: number;
  examLanguage?: ExamLanguage;
  colorScheme?: ColorScheme;
}

const initialState: PreferencesState = {};

export const preferencesSlice = createSlice({
  name: "preferences",
  initialState,
  reducers: {
    setPreferences: (
      state,
      action: PayloadAction<IUserPreferencesResponse>,
    ) => {
      state = action.payload;
    },
  },
});

export const { setPreferences } = preferencesSlice.actions;

export const selectLanguage = (state: RootState): ExamLanguage | void =>
  state.preferences.examLanguage;

export const selectColorScheme = (state: RootState): ColorScheme | void =>
  state.preferences.colorScheme;

export const selectQuestionsPerDay = (state: RootState): number | void =>
  state.preferences.questionsPerDay;

export default preferencesSlice.reducer;
