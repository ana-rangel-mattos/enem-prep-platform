"use client";

import React, { useEffect, useState } from "react";
import { AxiosError } from "axios";
import Loading from "@/components/Loading";
import Error from "next/error";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { Button } from "@/components/ui/button";
import { SquarePen } from "lucide-react";
import { z } from "zod";
import { toast } from "sonner";
import {
  Field,
  FieldContent,
  FieldDescription,
  FieldError,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field";
import { Controller, useForm } from "react-hook-form";
import { Input } from "@/components/ui/input";
import { Checkbox } from "@/components/ui/checkbox";
import { zodResolver } from "@hookform/resolvers/zod";
import { IGetScheduleDetailsResponse } from "@/lib/types/schedule.types";
import { schedulesService } from "@/lib/api/schedulesService";

const formSchema = z.object({
  name: z
    .string()
    .min(3, "Nome deve conter ao menos 3 caracteres.")
    .max(50, "Nome não deve passar de 50 caracters."),
  hasWeekEnd: z.boolean().default(false).optional(),
});

type FormData = z.infer<typeof formSchema>;

interface IEditSchedulePopoverProps {
  schedule: IGetScheduleDetailsResponse;
}

const EditSchedulePopover: React.FC<IEditSchedulePopoverProps> = ({
  schedule,
}) => {
  const [error, setError] = useState<AxiosError<ProblemDetails> | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const { control, reset, handleSubmit } = useForm<FormData>({
    resolver: zodResolver(formSchema),
    defaultValues: { name: schedule.name, hasWeekEnd: schedule.hasWeekEnd },
  });

  useEffect(() => {
    reset({ name: schedule.name, hasWeekEnd: schedule.hasWeekEnd });
  }, [schedule, reset]);

  const onSubmit = async (data: FormData) => {
    try {
      setIsLoading(true);
      setError(null);

      await schedulesService.UpdateSchedule(schedule.scheduleId, {
        ...data,
        hasWeekEnd: data?.hasWeekEnd ?? schedule.hasWeekEnd,
      });
      toast.success("Cronograma editado com sucesso.");
      reset();
    } catch (error) {
      const axiosError = error as AxiosError<ProblemDetails>;
      const problem = axiosError.response?.data;

      toast.error(problem?.description || "Erro ao editar o cronograma.");
      setError(axiosError);
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) return <Loading message="Criando cronograma..." />;

  if (error)
    return (
      <Error
        statusCode={error.response?.data.code || 500}
        title={error.response?.data.description}
      />
    );

  return (
    <Popover>
      <PopoverTrigger
        className="bg-sidebar rounded-md px-1 py-1 hover:cursor-pointer"
        render={<Button variant="outline" size="icon" />}
      >
        <SquarePen />
        <span className="sr-only">Editar cronograma</span>
      </PopoverTrigger>
      <PopoverContent>
        <form className="grid gap-2" onSubmit={handleSubmit(onSubmit)}>
          <FieldGroup>
            <Controller
              name="name"
              control={control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor="schedule-name" className="font-medium">
                    Nome
                  </FieldLabel>
                  <Input
                    {...field}
                    id="schedule-name"
                    aria-invalid={fieldState.invalid}
                    placeholder="Nomeie o cronograma"
                    autoComplete="off"
                  />
                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
            <Controller
              name="hasWeekEnd"
              control={control}
              render={({ field, fieldState }) => (
                <Field
                  orientation="horizontal"
                  data-invalid={fieldState.invalid}
                >
                  <Checkbox
                    id="has-week-end"
                    name="has-week-end"
                    checked={field.value}
                    onCheckedChange={field.onChange}
                  />
                  <FieldContent>
                    <FieldLabel htmlFor="has-week-end" className="font-medium">
                      Incluir Fim de Semana
                    </FieldLabel>
                    <FieldDescription>
                      Incluir fim de semana no cronograma semanal.
                    </FieldDescription>
                  </FieldContent>
                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
          </FieldGroup>
          <div className="grid w-full">
            <Button size="sm" type="submit" className="hover:cursor-pointer">
              Editar
            </Button>
          </div>
        </form>
      </PopoverContent>
    </Popover>
  );
};

export default EditSchedulePopover;
