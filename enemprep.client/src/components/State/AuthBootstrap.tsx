import { useAppDispatch } from "@/hooks/use-store";
import { useEffect, useRef } from "react";
import { logout, setUser } from "@/lib/features/auth/authSlice";
import { userService } from "@/lib/api/userService";
import { userPreferencesService } from "@/lib/api/userPreferencesService";
import {
  resetPreferences,
  setPreferences,
} from "@/lib/features/preferences/preferencesSlice";
import { useRouter } from "next/navigation";

export default function AuthBootstrap() {
  const dispatch = useAppDispatch();
  const initialized = useRef(false);

  const router = useRouter();

  useEffect(() => {
    if (initialized.current) return;

    initialized.current = true;

    async function bootstrapAuth() {
      try {
        const user = await userService.getLoggedUser();
        dispatch(setUser(user));

        const preferences =
          await userPreferencesService.getLoggedUserPreferences();
        dispatch(setPreferences(preferences));
      } catch {
        dispatch(logout());
        dispatch(resetPreferences());
        router.push("/login");
      }
    }

    bootstrapAuth();
  }, [dispatch, router]);

  return null;
}
