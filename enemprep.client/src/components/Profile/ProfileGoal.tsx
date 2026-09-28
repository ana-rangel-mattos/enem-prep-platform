import Subtitle from "@/components/ui/Subtitle";
import DeleteUserGoalForm from "@/components/Profile/DeleteUserGoalForm";
import CreateUpdateGoalForm from "@/components/Profile/CreateUpdateGoalForm";
import React, { useEffect, useState } from "react";
import {
  IUserGoalCreateUpdate,
  IUserGoalResponse,
} from "@/lib/types/user-goal.types";
import Loading from "@/components/Loading";
import { userGoalService } from "@/lib/api/userGoalService";
import { AxiosError } from "axios";
import { toast } from "sonner";
import Error from "next/error";
import { useRouter } from "next/navigation";
import { IUserResponse } from "@/lib/types/user.types";

interface IProfileGoalProps {
  user: IUserResponse;
}

const ProfileGoal: React.FC<IProfileGoalProps> = ({ user }) => {
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [userGoal, setUserGoal] = useState<IUserGoalResponse | null>(null);
  const [isEditing, setIsEditing] = useState<boolean>(false);
  const [goalError, setGoalError] = useState<AxiosError<ProblemDetails> | null>(
    null,
  );

  const router = useRouter();

  useEffect(() => {
    const loadUserGoal = async () => {
      try {
        setGoalError(null);
        if (user?.hasGoal) {
          const result = await userGoalService.getUserGoal();
          setUserGoal(result);
        }
      } catch (error) {
        const axiosError = error as AxiosError<ProblemDetails>;
        const problem = axiosError?.response?.data;
        setGoalError(axiosError);
        toast.error(
          problem?.description || "Erro ao carregar objetivo do usuário.",
        );
      } finally {
        setIsLoading(false);
      }
    };

    loadUserGoal();
  }, [user]);

  if (isLoading) {
    return <Loading message="Carregando objetivos do usuário..." />;
  }

  if (goalError || !userGoal) {
    return (
      <Error
        statusCode={goalError?.response?.data.code || 403}
        title={goalError?.response?.data.description}
      />
    );
  }

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

  const handleGoalDeleted = () => {
    setUserGoal(null);
    setIsEditing(false);
    router.push("/profile");
  };

  return (
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
  );
};

export default ProfileGoal;
