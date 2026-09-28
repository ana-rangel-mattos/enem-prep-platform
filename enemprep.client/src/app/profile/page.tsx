"use client";

import React, { useEffect, useState } from "react";
import Image from "next/image";
import Error from "next/error";
import { useAppSelector } from "@/hooks/use-store";
import { selectUser } from "@/lib/features/auth/authSlice";
import Subtitle from "@/components/ui/Subtitle";
import Title from "@/components/ui/Title";
import Text from "@/components/ui/Text";
import { ISODateToString } from "@/lib/helpers";
import Link from "next/link";
import { Cog, Flame, Quote } from "lucide-react";
import ProfileGoal from "@/components/Profile/ProfileGoal";
import { IUserProfileResponse } from "@/lib/types/user-profile.types";
import { AxiosError } from "axios";
import { toast } from "sonner";
import { userProfileService } from "@/lib/api/userProfileService";
import Loading from "@/components/Loading";
import { Fire } from "@hugeicons/core-free-icons";

const Profile = () => {
  const [userProfile, setUserProfile] = useState<IUserProfileResponse | null>(
    null,
  );
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [profileError, setProfileError] =
    useState<AxiosError<ProblemDetails> | null>(null);
  const user = useAppSelector((state) => selectUser(state));

  useEffect(() => {
    const getUserProfile = async () => {
      try {
        setProfileError(null);
        const profile = await userProfileService.getProfile();
        setUserProfile(profile);
      } catch (error) {
        const axiosError = error as AxiosError<ProblemDetails>;
        const problem = axiosError?.response?.data;
        setProfileError(axiosError);
        toast.error(
          problem?.description || "Erro ao carregar perfil do usuário.",
        );
      } finally {
        setIsLoading(false);
      }
    };

    getUserProfile();
  }, []);

  if (!user) {
    return <Error statusCode={403} />;
  }

  if (isLoading) {
    return <Loading message="Carregando perfil do usuário..." />;
  }

  if (profileError || !userProfile) {
    return (
      <Error
        statusCode={profileError?.response?.data.code || 403}
        title={profileError?.response?.data.description}
      />
    );
  }

  return (
    <div className="relative">
      <Link className="absolute right-4 flex gap-x-1" href="/settings">
        <Cog /> Configurações
      </Link>
      <Title text="Perfil do Usuário" className="mb-5" />
      <div className="bg-card lg:divide-primary flex flex-col items-center gap-y-6 rounded-3xl px-4 py-3.5 lg:flex-row lg:gap-x-6 lg:gap-y-0 lg:divide-x-3 lg:rounded-full">
        <Image
          src={
            user.imageUrl
              ? user.imageUrl
              : `https://ui-avatars.com/api/?${user.fullName.split(" ").join("+")}&color=fff&background=0D8ABC&size=250`
          }
          alt={`User ${user.username}'s profile image.`}
          width={250}
          height={250}
          className="rounded-full"
        />
        <div className="bg-background flex w-full flex-col items-center gap-y-2 rounded-xl lg:justify-center lg:gap-y-3 lg:self-stretch lg:rounded-r-full lg:py-4">
          <Subtitle text={user.fullName} className="mt-4" />
          <Text size="lg" variant="primary" bold italic>
            @{user.username}
          </Text>
          <Flame />
          {userProfile.streakCount}
          <p className="w-full px-4">
            <Quote />
            {userProfile.userBio}
          </p>
          <Text size="base" variant="secondary" bold italic className="mt-auto">
            {user.email}
          </Text>
        </div>
      </div>
      <ProfileGoal user={user} />
      <div className="text-primary flex flex-col items-center">
        <Subtitle text="Estatísticas" className="my-5" />
        <Text size="md" variant="primary" className="mt-1 mb-4">
          <strong>Conta criada em:</strong> {ISODateToString(user.createdAt)}
        </Text>
        <Text size="md" variant="primary" className="mt-1 mb-4">
          <strong>Número de Tentativas (Questões):</strong>{" "}
          {user.attemptedQuestions}
        </Text>
        <Text size="md" variant="primary" className="mt-1 mb-4">
          <strong>Número de Tentativas (Simulados):</strong>{" "}
          {user.correctAnswers}
        </Text>
        <Text size="md" variant="primary" className="mt-1 mb-4">
          <strong>Tentativas Corretas (Questões):</strong> {user.correctAnswers}
        </Text>
        <Text size="md" variant="primary" className="mt-1 mb-4">
          <strong>Porcentagem de Acerto (Questões):</strong> {user.successRate}
        </Text>
        <Text size="md" variant="primary" className="mt-1 mb-4">
          <strong>Média da Nota de Corte:</strong> {user.averageScore}
        </Text>
        <Text size="md" variant="primary" className="mt-1 mb-4">
          <strong>Melhor Nota de Corte:</strong> {user.highestScore}
        </Text>
        <Text size="md" variant="primary" className="mt-1 mb-4">
          <strong>Tempo Médio Gasto por Questão:</strong>{" "}
          {user.averageTimeSpent.slice(0, 8)}
        </Text>
      </div>
    </div>
  );
};

export default Profile;
