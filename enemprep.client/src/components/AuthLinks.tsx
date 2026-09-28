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
    router.refresh();
  };

  return (
    <>
      <h1 className="text-base font-medium">Documents</h1>
      <div className="ml-auto flex items-center gap-2">
        {isAuthenticated ? (
          <Button onClick={onLogout} size="sm" className="cursor-pointer">
            Logout
          </Button>
        ) : (
          <>
            <Button
              variant="ghost"
              asChild
              size="lg"
              className="hidden lg:flex"
            >
              <Link href="/login">Login</Link>
            </Button>
            <Button
              variant="ghost"
              asChild
              size="lg"
              className="hidden lg:flex"
            >
              <Link href="/register">Cadastre-se</Link>
            </Button>
          </>
        )}
      </div>
    </>
  );
}
