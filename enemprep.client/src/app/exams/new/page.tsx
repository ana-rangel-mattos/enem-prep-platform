"use client";

import Title from "@/components/Title";
import { z } from "zod";
import { ExamLanguage, SubjectName } from "@/lib/types/enums.types";
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
import { Controller, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Input } from "@/components/ui/input";
import React, { useState } from "react";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Checkbox } from "@/components/ui/checkbox";
import { Label } from "@/components/ui/label";
import { examsService } from "@/lib/api/examsService";
import { useRouter } from "next/navigation";
import { toast } from "sonner";
import Loading from "@/components/Loading";
import { useAppSelector } from "@/hooks/use-store";
import { selectLanguage } from "@/lib/features/preferences/preferencesSlice";
import { Button } from "@/components/ui/button";

const formScheme = z.object({
  totalQuestions: z.coerce
    .number({ error: "O número questões deve ser um número." })
    .int("O número de questões deve ser um número inteiro.")
    .min(5, "Mínimo de 5 questões por simulado.")
    .max(90, "Máximo de 90 questões por simulado.")
    .positive({ error: "O número de questões deve ser positivo." }),
  areas: z.preprocess(
    (value) => {
      if (!Array.isArray(value)) return [];
      return value.map((item) => Number(item));
    },
    z
      .array(z.enum(SubjectName))
      .min(1, "Deve conter ao menos uma área selecionada."),
  ),
  language: z
    .preprocess(
      (value) => (value === "" ? undefined : Number(value)),
      z.enum(ExamLanguage),
    )
    .optional(),
});

type FormData = z.infer<typeof formScheme>;

const SUBJECT_OPTIONS = [
  { id: SubjectName.Languages, label: "Linguagens" },
  { id: SubjectName.Humanities, label: "Humanidades" },
  { id: SubjectName.NaturalSciences, label: "Natureza" },
  { id: SubjectName.Mathematics, label: "Matemática" },
];

const NewExam = () => {
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const examLanguage: ExamLanguage | void = useAppSelector((state) =>
    selectLanguage(state),
  );

  const { control, reset, handleSubmit } = useForm<FormData>({
    resolver: zodResolver(formScheme),
    defaultValues: { areas: [], language: examLanguage ?? 0 },
  });

  const router = useRouter();

  const onSubmit = async (data: FormData) => {
    try {
      setIsLoading(true);

      const response = await examsService.generateExam(data);

      reset();
      router.push(`/exams/${response.examId}/take`);
    } catch (error: any) {
      const backendError =
        error?.response?.data?.details || "Ocorreu um erro inesperado.";
      toast.error(backendError);
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) return <Loading message="Criando o simulado..." />;

  return (
    <div>
      <Title text="Começe um novo Simulado" />

      <Card className="mx-auto mt-5 w-full sm:max-w-md md:max-w-lg lg:max-w-2xl">
        <CardHeader>
          <CardTitle>Criar Simulado</CardTitle>
          <CardDescription>Configure o simulado.</CardDescription>
        </CardHeader>

        <CardContent>
          <form id="form-create-new-exam" onSubmit={handleSubmit(onSubmit)}>
            <FieldGroup>
              <Controller
                name="totalQuestions"
                control={control}
                render={({ field, fieldState }) => (
                  <Field data-invalid={fieldState.invalid}>
                    <FieldLabel htmlFor="total-questions">
                      Número de Questões
                    </FieldLabel>
                    <Input
                      {...field}
                      id="total-questions"
                      placeholder="Número de questões do simulado. Ex: 15."
                      autoComplete="off"
                      type="number"
                      step={1}
                      min={5}
                      max={90}
                    />
                    {fieldState.invalid && (
                      <FieldError errors={[fieldState.error]} />
                    )}
                  </Field>
                )}
              />
              <Controller
                name="language"
                control={control}
                render={({ field, fieldState }) => (
                  <Field data-invalid={fieldState.invalid}>
                    <FieldLabel htmlFor="foreign-language">
                      Idioma Estrangeiro
                    </FieldLabel>
                    <Select
                      onValueChange={field.onChange}
                      defaultValue={field.value?.valueOf().toString()}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Selecione o idioma (Opcional)"></SelectValue>
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
                name="areas"
                control={control}
                defaultValue={[]}
                render={({ field, fieldState }) => {
                  const currentValues: number[] = field.value || [];

                  return (
                    <Field data-invalid={fieldState.invalid} className="mb-3">
                      <FieldLabel>Áreas do Conhecimento</FieldLabel>

                      <div className="mt-3 grid grid-cols-1 gap-3 sm:grid-cols-2">
                        {SUBJECT_OPTIONS.map((option) => {
                          const isChecked = currentValues.includes(option.id);

                          return (
                            <div
                              key={option.id}
                              className="flex items-start space-y-0 space-x-3"
                            >
                              <Checkbox
                                id={`subject-${option.id}`}
                                checked={isChecked}
                                onCheckedChange={(checked) => {
                                  if (checked) {
                                    field.onChange([
                                      ...currentValues,
                                      option.id,
                                    ]);
                                  } else {
                                    field.onChange(
                                      currentValues.filter(
                                        (value) => value != option.id,
                                      ),
                                    );
                                  }
                                }}
                              />
                              <div className="grid gap-1.5 leading-none">
                                <Label
                                  htmlFor={`subject-${option.id}`}
                                  className="cursor-pointer text-sm leading-none font-medium peer-disabled:opacity-70"
                                >
                                  {option.label}
                                </Label>
                              </div>
                            </div>
                          );
                        })}
                      </div>
                      {fieldState.invalid && (
                        <FieldError errors={[fieldState.error]} />
                      )}
                    </Field>
                  );
                }}
              />
            </FieldGroup>
          </form>

          <CardFooter>
            <Field orientation="horizontal">
              <Button
                type="submit"
                variant="secondary"
                form="form-create-new-exam"
                className="hover:cursor-pointer"
              >
                Criar Simulado
              </Button>
            </Field>
          </CardFooter>
        </CardContent>
      </Card>
    </div>
  );
};

export default NewExam;
