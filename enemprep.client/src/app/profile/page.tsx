"use client"

import React, { useEffect, useState } from "react";
import Image from "next/image";
import { IUser } from "@/lib/types/user.types";
import Loading from "@/components/Loading";
import Error from "next/error";
import { useAppSelector } from "@/hooks/use-store";
import { selectUser } from "@/lib/features/auth/authSlice";
import { userService } from "@/lib/api/userService";
import { toast } from "sonner";

const Profile = () => {
  const [user, setUser] = useState<IUser | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    const loadUser = async () => {
      try {
        const result = await userService.getLoggedUser();
        setUser(result);
      } catch (error: any) {
        const backendError =
          error?.response?.data?.details || "Ocorreu um erro inesperado.";
        toast.error(backendError);
      } finally {
        setIsLoading(false);
      }
    }

    loadUser();
  }, []);

  if (isLoading) {
    return <Loading message="Carregando Detalhes do Usuário..." />;
  }

  if (!user) {
    return <Error statusCode={403} />;
  }

  return (
    <>
      <h1 className="mb-5 text-center text-2xl">Perfil do Usuário</h1>
      <div className="bg-card flex gap-x-6 rounded-full px-4 py-3.5">
        <Image
          src={`https://ui-avatars.com/api/?${user.fullName.split(" ").join("+")}&color=fff&background=0D8ABC&size=250`}
          alt={`User ${user.username}'s profile image.`}
          width={250}
          height={250}
          className="rounded-full"
        />
        <div className="flex flex-col gap-y-3">
          <p className="text-3xl font-bold">{user.fullName}</p>
          <p className="text-primary text-lg font-bold italic">
            @{user.username}
          </p>
          <p className="text-secondary mt-auto text-base font-bold italic">
            {user.email}
          </p>
        </div>
        <div className="flex flex-col gap-y-3">
          <p className="text-base font-bold">Idade da Conta:<span className="ml-2">1 Ano</span></p>
        </div>
      </div>
    </>
  );
}

export default Profile;