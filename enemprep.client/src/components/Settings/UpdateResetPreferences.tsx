"use client";

import { IUserPreferencesResponse } from "@/lib/types/user-preferences.types";
import { z } from "zod";
import { ColorScheme, ExamLanguage } from "@/lib/types/enums.types";
import React from "react";
import { Controller, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { AxiosError } from "axios";
import { toast } from "sonner";
import { Card, CardContent, CardFooter } from "@/components/ui/card";
import FormCardHeader from "@/components/FormCardHeader";
import { Button } from "@/components/ui/button";
import {
  Field,
  FieldError,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { userPreferencesService } from "@/lib/api/userPreferencesService";

interface IUpdateResetPreferencesProps {
  userPreferences: IUserPreferencesResponse;
  isEditing: boolean;
  setIsEditing: (isEditing: boolean) => void;
  onSavePreferences: () => void;
}

const formSchema = z.object({
  userPreferencesId: z.uuid(),
  userId: z.uuid(),
  questionsPerDay: z.coerce
    .number()
    .positive("O número de questões diárias deve ser positiva.")
    .min(5, "O número mínimo de questões diáras deve ser 5")
    .max(90, "O número máximo de questões diárias deve ser 90."),
  examLanguage: z.preprocess(
    (value) => (value === "" ? undefined : Number(value)),
    z.enum(ExamLanguage).default(ExamLanguage.English),
  ),
  colorScheme: z.preprocess(
    (value) => (value === "" ? undefined : Number(value)),
    z.enum(ColorScheme).default(ColorScheme.OS),
  ),
});

type FormData = z.infer<typeof formSchema>;

const UpdateResetPreferences: React.FC<IUpdateResetPreferencesProps> = ({
  userPreferences,
  onSavePreferences,
  setIsEditing,
  isEditing,
}) => {
  const { control, handleSubmit } = useForm({
    resolver: zodResolver(formSchema),
    defaultValues: {
      userPreferencesId: userPreferences.userPreferencesId,
      userId: userPreferences.userId,
      questionsPerDay: userPreferences.questionsPerDay,
      colorScheme: userPreferences.colorScheme,
      examLanguage: userPreferences.examLanguage,
    },
  });

  const onSubmit = async (data: FormData) => {
    try {
      await userPreferencesService.updateUserPreferences(data);
      onSavePreferences();
      toast.success("Preferências atualizadas com sucesso!");
    } catch (error) {
      const axiosError = error as AxiosError<ProblemDetails>;
      const problem = axiosError.response?.data;
      toast.error(problem?.description || "Erro ao atualizar as preferências.");
    }
  };

  const onReset = async () => {
    try {
      await userPreferencesService.resetUserPreferences();
      toast.success("Preferências resetadas com sucesso!");
    } catch (error) {
      const axiosError = error as AxiosError<ProblemDetails>;
      const problem = axiosError.response?.data;
      toast.error(problem?.description || "Erro ao resetar as preferências.");
    }
  };

  return (
    <Card className="mx-auto w-full sm:max-w-md md:max-w-lg lg:max-w-4xl">
      <FormCardHeader
        title={isEditing ? "Atualizar Preferências" : "Preferências"}
        description="Visualize e atualize suas preferências aqui!"
        className="flex flex-col"
      >
        {!isEditing && (
          <div className="mt-3 flex gap-x-4">
            <Button
              onClick={onReset}
              className="bg-background text-chart-3 hover:bg-destructive hover:text-destructive-foreground min-h-7 min-w-20 rounded-md hover:cursor-pointer"
            >
              Resetar
            </Button>
            <Button
              onClick={() => setIsEditing(true)}
              className="bg-background text-primary hover:text-background min-h-7 min-w-20 rounded-md hover:cursor-pointer"
            >
              Editar
            </Button>
          </div>
        )}
      </FormCardHeader>
      <CardContent>
        <form
          onSubmit={handleSubmit(onSubmit)}
          id="form-update-reset-preferences"
        >
          <FieldGroup>
            <Controller
              name="questionsPerDay"
              control={control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor="questions-per-day">
                    Questões Por Dia
                  </FieldLabel>
                  <Input
                    {...field}
                    type="number"
                    step="1"
                    inputMode="numeric"
                    min={5}
                    max={90}
                    id="questions-per-days"
                    disabled={!isEditing}
                    placeholder="Número de questões por dia. Ex: 5."
                  />
                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
            <Controller
              name="examLanguage"
              control={control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor="foreign-language">
                    Idioma Estrangeiro
                  </FieldLabel>
                  <Select
                    disabled={!isEditing}
                    onValueChange={field.onChange}
                    defaultValue={userPreferences.examLanguage
                      .valueOf()
                      .toString()}
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Idioma Estrangeiro"></SelectValue>
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem
                        value={ExamLanguage.English.valueOf().toString()}
                      >
                        Inglês
                      </SelectItem>
                      <SelectItem
                        value={ExamLanguage.Spanish.valueOf().toString()}
                      >
                        Espanhol
                      </SelectItem>
                    </SelectContent>
                  </Select>
                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
            <Controller
              name="colorScheme"
              control={control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor="foreign-language">Tema</FieldLabel>
                  <Select
                    disabled={!isEditing}
                    onValueChange={field.onChange}
                    defaultValue={userPreferences.colorScheme
                      .valueOf()
                      .toString()}
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Idioma Estrangeiro"></SelectValue>
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value={ColorScheme.OS.valueOf().toString()}>
                        OS (Sistema Operacional)
                      </SelectItem>
                      <SelectItem
                        value={ColorScheme.Light.valueOf().toString()}
                      >
                        Claro
                      </SelectItem>
                      <SelectItem value={ColorScheme.Dark.valueOf().toString()}>
                        Escuro
                      </SelectItem>
                    </SelectContent>
                  </Select>
                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
          </FieldGroup>
        </form>
      </CardContent>
      {isEditing && (
        <CardFooter>
          <Field orientation="horizontal">
            <Button
              type="button"
              variant="outline"
              onClick={() => setIsEditing(false)}
            >
              Cancelar
            </Button>
            <Button type="submit" form="form-update-reset-preferences">
              Atualizar
            </Button>
          </Field>
        </CardFooter>
      )}
    </Card>
  );
};

export default UpdateResetPreferences;
