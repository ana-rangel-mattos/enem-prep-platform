import { useAppDispatch } from "@/hooks/use-store";
import { useEffect, useRef } from "react";
import { logout, setUser } from "@/lib/features/auth/authSlice";
import { userService } from "@/lib/api/userService";

export default function AuthBootstrap() {
  const dispatch = useAppDispatch();
  const initialized = useRef(false);

  useEffect(() => {
    if (initialized.current) return;

    initialized.current = true;

    async function bootstrapAuth() {
      try {
        const user = await userService.getLoggedUser();
        dispatch(setUser(user));
      } catch {
        dispatch(logout());
      }
    }

    bootstrapAuth();
  }, [dispatch]);

  return null;
}
