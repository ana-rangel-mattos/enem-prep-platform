"use client";

import React, { useEffect, useState } from "react";
import Image from "next/image";
import Error from "next/error";
import { useAppSelector } from "@/hooks/use-store";
import { selectUser } from "@/lib/features/auth/authSlice";
import { toast } from "sonner";
import Loading from "@/components/Loading";
import Subtitle from "@/components/Subtitle";
import Title from "@/components/Title";
import Text from "@/components/Text";
import { ISODateToString } from "@/lib/helpers";
import CreateUpdateGoalForm from "@/components/CreateUpdateGoalForm";
import { userGoalService } from "@/lib/api/userGoalService";
import {
  IUserGoalCreateUpdate,
  IUserGoalResponse,
} from "@/lib/types/user-goal.types";
import DeleteUserGoalForm from "@/components/DeleteUserGoalForm";
import { useRouter } from "next/navigation";

const Profile = () => {
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [userGoal, setUserGoal] = useState<IUserGoalResponse | null>(null);
  const [isEditing, setIsEditing] = useState<boolean>(false);
  const router = useRouter();
  const user = useAppSelector((state) => selectUser(state));

  useEffect(() => {
    const loadUserGoal = async () => {
      try {
        if (user?.hasGoal) {
          const result = await userGoalService.getUserGoal();
          setUserGoal(result);
        }
      } catch (error: any) {
        const backendError =
          error?.response?.data?.details || "Ocorreu um erro inesperado.";
        toast.error(backendError);
      } finally {
        setIsLoading(false);
      }
    };

    loadUserGoal();
  }, [user]);

  if (isLoading) {
    return <Loading message="Carregando objetivos do usuário..." />;
  }

  if (!user) {
    return <Error statusCode={403} />;
  }

  const handleGoalDeleted = () => {
    setUserGoal(null);
    setIsEditing(false);
    router.push("/profile");
  };

  const handleGoalSaved = (newGoal: IUserGoalCreateUpdate) => {
    setUserGoal((prevGoal) => {
      if (!prevGoal) return null;

      return {
        userId: prevGoal.userId,
        userGoalId: prevGoal.userGoalId,
        universityName: newGoal.universityName,
        courseName: newGoal.courseName,
        cutOffScore: newGoal.cutOffScore,
      };
    });
    setIsEditing(false);
    router.push("/profile");
  };

  return (
    <>
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
          <Text size="base" variant="secondary" bold italic className="mt-auto">
            {user.email}
          </Text>
        </div>
      </div>
      <div className="text-primary flex flex-col items-center justify-center">
        <Subtitle text="Metas" className="my-5" />
        {user.hasGoal && userGoal && !isEditing && (
          <DeleteUserGoalForm
            userGoal={userGoal}
            setIsEditing={setIsEditing}
            onGoalDelete={handleGoalDeleted}
          />
        )}
        {(!user.hasGoal || isEditing) && (
          <CreateUpdateGoalForm
            isEditing={isEditing}
            setIsEditing={setIsEditing}
            userGoal={userGoal ? userGoal : undefined}
            onSaveGoal={handleGoalSaved}
          />
        )}
      </div>
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
    </>
  );
};

export default Profile;
