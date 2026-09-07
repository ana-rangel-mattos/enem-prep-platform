"use client";

import { IUserGoalCreateUpdate } from "@/lib/types/user-goal.types";
import React from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { Controller, useForm } from "react-hook-form";
import { toast } from "sonner";
import { z } from "zod";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Field,
  FieldError,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { userGoalService } from "@/lib/api/userGoalService";
import { useRouter } from "next/navigation";

interface ICreateUpdateGoalProps {
  userGoal?: IUserGoalCreateUpdate;
  isEditing: boolean;
  setIsEditing: (isEditing: boolean) => void;
  onSaveGoal: (goal: IUserGoalCreateUpdate) => void;
}

const formSchema = z.object({
  universityName: z
    .string()
    .min(5, "Nome da Universidade deve conter ao menos 5 caracteres.")
    .max(255, "Nome da Universidade não deve passar de 255 caracteres."),
  courseName: z
    .string()
    .min(5, "Nome do Curso deve conter ao menos 5 caracteres.")
    .max(255, "Nome do Curso não deve passar de 255 caracteres."),
  cutOffScore: z.coerce
    .number()
    .positive("Nota de Corte deve ser positiva.")
    .min(200, "A Nota de Corte mínima deve ser de 200,00.")
    .max(1000, "A Nota de Corte não deve ultrapassar 1.000."),
});

type FormData = z.infer<typeof formSchema>;

const CreateUpdateGoalForm: React.FC<ICreateUpdateGoalProps> = ({
  userGoal,
  isEditing,
  setIsEditing,
  onSaveGoal,
}) => {
  const { control, handleSubmit, reset } = useForm<FormData>({
    resolver: zodResolver(formSchema),
    defaultValues: userGoal
      ? (userGoal as unknown as FormData)
      : { universityName: "", courseName: "", cutOffScore: 0 },
  });

  const onSubmit = async (data: FormData) => {
    try {
      if (!userGoal) {
        await userGoalService.createUserGoal(data);
      } else {
        await userGoalService.updateUserGoal(data);
      }
      reset();
      onSaveGoal(data);
    } catch (error: any) {
      const backendError =
        error?.response?.data?.details || "Ocorreu um erro inesperado.";
      toast.error(backendError);
    }
  };

  return (
    <Card className="w-full sm:max-w-md md:max-w-lg lg:max-w-2xl">
      <CardHeader>
        <CardTitle>{!userGoal ? "Atualizar Meta" : "Criar Meta"}</CardTitle>
        <CardDescription>
          Crie e atualize as suas metas universitárias aqui!
        </CardDescription>
      </CardHeader>

      <CardContent>
        <form
          onSubmit={handleSubmit(onSubmit)}
          id="form-create-update-user-goal"
        >
          <FieldGroup>
            <Controller
              name="universityName"
              control={control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor="form-univeristy-name">
                    Universidade
                  </FieldLabel>
                  <Input
                    {...field}
                    id="form-univeristy-name"
                    placeholder="Nome da Universidade"
                    autoComplete="off"
                  />
                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
            <Controller
              name="courseName"
              control={control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor="form-course-name">Curso</FieldLabel>
                  <Input
                    {...field}
                    id="form-course-name"
                    placeholder="Nome do Curso"
                    autoComplete="off"
                  />
                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
            <Controller
              name="cutOffScore"
              control={control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor="form-cut-off-score">
                    Nota de Corte
                  </FieldLabel>
                  <Input
                    {...field}
                    type="number"
                    step="any"
                    inputMode="decimal"
                    id="form-cut-off-score"
                    placeholder="Nota de Corte. Ex: 752,36."
                  />
                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
          </FieldGroup>
        </form>
      </CardContent>
      <CardFooter>
        <Field orientation="horizontal">
          <Button
            type="button"
            variant="outline"
            onClick={() => (isEditing ? setIsEditing(false) : reset())}
          >
            {isEditing ? "Cancelar" : "Resetar"}
          </Button>
          <Button type="submit" form="form-create-update-user-goal">
            {userGoal ? "Atualizar" : "Criar"}
          </Button>
        </Field>
      </CardFooter>
    </Card>
  );
};

export default CreateUpdateGoalForm;
