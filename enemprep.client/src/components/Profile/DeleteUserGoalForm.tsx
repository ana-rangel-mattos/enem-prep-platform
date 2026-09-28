import { IUserGoalResponse } from "@/lib/types/user-goal.types";
import React from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { useForm } from "react-hook-form";
import { Field } from "@/components/ui/field";
import { Button } from "@/components/ui/button";
import { userGoalService } from "@/lib/api/userGoalService";
import { toast } from "sonner";
import Text from "@/components/ui/Text";

interface IDeleteUserGoalProps {
  userGoal: IUserGoalResponse;
  setIsEditing: (isEditing: boolean) => void;
  onGoalDelete: () => void;
}

const DeleteUserGoalForm: React.FC<IDeleteUserGoalProps> = ({
  userGoal,
  setIsEditing,
  onGoalDelete,
}) => {
  const { handleSubmit } = useForm();

  const onDelete = async () => {
    try {
      await userGoalService.deleteUserGoal();
      onGoalDelete();
    } catch (error: any) {
      const backendError =
        error?.response?.data || "Ocorreu um erro inesperado.";
      toast.error(backendError);
    }
  };

  return (
    <Card className="w-full sm:max-w-md md:max-w-lg lg:max-w-xl">
      <CardHeader>
        <CardTitle>Meta Universitária</CardTitle>
        <CardDescription>Aqui está a sua meta!</CardDescription>
      </CardHeader>

      <CardContent className="space-y-2">
        <form
          onSubmit={handleSubmit(onDelete)}
          id="form-delete-user-goal"
        ></form>
        <Text>
          <strong>Universidade:</strong> {userGoal.universityName}
        </Text>
        <Text>
          <strong>Curso:</strong> {userGoal.courseName}
        </Text>
        <Text>
          <strong>Nota de Corte:</strong> {userGoal.cutOffScore}
        </Text>
      </CardContent>

      <CardFooter>
        <Field orientation="horizontal">
          <Button
            type="button"
            variant="outline"
            onClick={() => setIsEditing(true)}
            className="hover:cursor-pointer"
          >
            Atualizar
          </Button>
          <Button
            type="submit"
            variant="destructive"
            form="form-delete-user-goal"
            className="hover:cursor-pointer"
          >
            Deletar
          </Button>
        </Field>
      </CardFooter>
    </Card>
  );
};

export default DeleteUserGoalForm;
