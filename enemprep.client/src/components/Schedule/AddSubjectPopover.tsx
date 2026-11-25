"use client";

import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { SquarePlus } from "lucide-react";
import { Button } from "@/components/ui/button";
import { FieldError, FieldGroup } from "@/components/ui/field";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { Controller, useForm } from "react-hook-form";
import { DayOfTheWeek } from "@/lib/types/enums.types";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import React, { useEffect, useState } from "react";
import { AxiosError } from "axios";
import { toast } from "sonner";
import { schedulesService } from "@/lib/api/schedulesService";
import { IGetSubjectResponse } from "@/lib/types/subjects.types";
import { subjectsService } from "@/lib/api/subjectsService";

const formScheme = z.object({ scheduleId: z.uuid(), subjectId: z.string() });

type FormData = z.infer<typeof formScheme>;

interface IAddSubjectPopoverProps {
  scheduleId: string;
  weekday: DayOfTheWeek;
  onSubjectAdded?: () => void;
}

const AddSubjectPopover: React.FC<IAddSubjectPopoverProps> = ({
  scheduleId,
  weekday,
  onSubjectAdded,
}) => {
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

  const [subjects, setSubjects] = useState<IGetSubjectResponse[] | null>(null);
  const [isLoadingSubjects, setIsLoadingSubjects] = useState<boolean>(false);
  const [subjectsError, setSubjectsError] =
    useState<AxiosError<ProblemDetails> | null>(null);

  const { control, reset, handleSubmit } = useForm({
    resolver: zodResolver(formScheme),
    defaultValues: { scheduleId: scheduleId },
  });

  useEffect(() => {
    const getSubjects = async () => {
      try {
        setIsLoadingSubjects(true);
        setSubjectsError(null);

        const subjects = await subjectsService.GetSubjects();
        setSubjects(subjects);
      } catch (error) {
        const axiosError = error as AxiosError<ProblemDetails>;
        const problem = axiosError.response?.data;

        toast.error(
          problem?.description || "Não foi possível carregar as disciplinas.",
        );
        setSubjectsError(axiosError);
      } finally {
        setIsLoadingSubjects(false);
      }
    };

    getSubjects();
  }, []);

  const onSubmit = async (data: FormData) => {
    try {
      setIsSubmitting(true);
      await schedulesService.PostSubject(scheduleId, {
        subjectId: data.subjectId,
        weekDay: weekday,
      });

      toast.success("Disciplina adicionada com sucesso!");
      reset();
      onSubjectAdded?.();
    } catch (error) {
      const axiosError = error as AxiosError<ProblemDetails>;
      const problem = axiosError.response?.data;

      toast.error(
        problem?.description || "Não foi possível adicionar uma disciplina.",
      );
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Popover>
      <PopoverTrigger
        className="rounded-md hover:cursor-pointer"
        render={<Button variant="outline" size="icon" />}
      >
        <SquarePlus className="text-muted-foreground hover:text-chart-2/70" />
        <span className="sr-only">Adicionar Disciplina</span>
      </PopoverTrigger>
      <PopoverContent>
        <form className="grid gap-2" onSubmit={handleSubmit(onSubmit)}>
          <FieldGroup>
            <Controller
              name="subjectId"
              control={control}
              render={({ field, fieldState }) => (
                <Select value={field.value} onValueChange={field.onChange}>
                  <SelectTrigger className="w-full">
                    <SelectValue
                      placeholder={
                        isLoadingSubjects
                          ? "Carregando disciplinas..."
                          : "Selecione uma disciplina"
                      }
                    />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectGroup>
                      <SelectLabel>Disciplinas</SelectLabel>
                      {subjectsError && (
                        <div className="text-destructive p-2 text-xs">
                          {subjectsError?.response?.data.description}
                        </div>
                      )}
                      {subjects?.map((item) => (
                        <SelectItem key={item.subjectId} value={item.subjectId}>
                          {item.name}
                        </SelectItem>
                      ))}
                    </SelectGroup>
                  </SelectContent>
                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Select>
              )}
            />
          </FieldGroup>
          <div className="grid w-full">
            <Button
              size="sm"
              type="submit"
              disabled={isSubmitting || isLoadingSubjects}
              className="hover:cursor-pointer"
            >
              {isSubmitting ? "Adicionando..." : "Adicionar"}
            </Button>
          </div>
        </form>
      </PopoverContent>
    </Popover>
  );
};

export default AddSubjectPopover;
