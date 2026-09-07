import { useAppDispatch } from "@/hooks/use-store";
import { useEffect, useRef } from "react";
import { userPreferencesService } from "@/lib/api/userPreferencesService";
import { setPreferences } from "@/lib/features/preferences/preferencesSlice";

export default function PreferencesBootstrap() {
  const dispatch = useAppDispatch();
  const initialized = useRef(false);

  useEffect(() => {
    if (initialized.current) return;

    initialized.current = true;

    async function handleUserPreferences() {
      try {
        const preferences =
          await userPreferencesService.getLoggedUserPreferences();
        dispatch(setPreferences(preferences));
      } catch {}
    }

    handleUserPreferences();
  }, [dispatch]);

  return null;
}
