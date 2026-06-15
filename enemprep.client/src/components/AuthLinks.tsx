"use client";

import Link from "next/link";
import React from "react";
import { useAppSelector } from "@/hooks/use-store";
import { logout, selectIsAuthenticated } from "@/lib/features/auth/authSlice";
import { Button } from "@/components/ui/button";
import { logoutDeleteCookies } from "@/lib/actions";
import { useRouter } from "next/navigation";

export default function AuthLinks() {
  const router = useRouter();
  const isAuthenticated: boolean = useAppSelector((state) =>
    selectIsAuthenticated(state),
  );

  const onLogout = async () => {
    await logoutDeleteCookies();
    logout();
    router.push("/login");
  };

  return (
    <div className="flex w-3/5 gap-x-4">
      {isAuthenticated ? (
        <Button onClick={onLogout} size="sm" className="cursor-pointer">
          Logout
        </Button>
      ) : (
        <>
          <Link href="/login">Login</Link>
          <Link href="/register">Cadastre-se</Link>
        </>
      )}
    </div>
  );
}
