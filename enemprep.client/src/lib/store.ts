import { combineReducers, configureStore } from "@reduxjs/toolkit";
import authReducer from "@/lib/features/auth/authSlice";
import preferencesReducer from "@/lib/features/preferences/preferencesSlice";

export const makeStore = () => {
  return configureStore({
    reducer: combineReducers({
      auth: authReducer,
      preferences: preferencesReducer,
    }),
  });
};

// Infer the type of makeStore
export type AppStore = ReturnType<typeof makeStore>;
// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<AppStore["getState"]>;
export type AppDispatch = AppStore["dispatch"];
