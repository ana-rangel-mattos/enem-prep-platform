import { useAppDispatch } from "@/hooks/use-store";
import { useEffect, useRef } from "react";
import { authService } from "@/lib/api/authService";
import { logout, setUser } from "@/lib/features/auth/authSlice";

export default function AuthBootstrap() {
  const dispatch = useAppDispatch();
  const initialized = useRef(false);

  useEffect(() => {
    if (initialized.current) return;

    initialized.current = true;

    async function bootstrapAuth() {
      try {
        const user = await authService.getLoggedUser();
        dispatch(setUser(user));
      } catch (err) {
        console.log("AUTH ERROR", err);
        dispatch(logout());
      }
    }

    bootstrapAuth();
  }, [dispatch]);

  return null;
}
