"use client";

import { Label } from "../ui/label";
import Subtitle from "@/components/ui/Subtitle";
import { ChangeEvent, useEffect, useState } from "react";
import { AxiosError } from "axios";
import { schedulesService } from "@/lib/api/schedulesService";
import { toast } from "sonner";
import {
  NativeSelect,
  NativeSelectOption,
} from "@/components/ui/native-select";
import {
  IGetScheduleDetailsResponse,
  IGetScheduleResponse,
} from "@/lib/types/schedule.types";
import { CalendarDays } from "lucide-react";
import Loading from "@/components/Loading";
import Error from "next/error";
import CreateSchedulePopover from "@/components/Schedule/CreateSchedulePopover";
import ScheduleKanban from "@/components/Schedule/ScheduleKanban";
import EditSchedulePopover from "@/components/Schedule/EditSchedulePopover";

const ScheduleSelection = () => {
  const [schedules, setSchedules] = useState<IGetScheduleResponse[] | null>(
    null,
  );
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [selectedSchedule, setSelectedSchedule] =
    useState<IGetScheduleDetailsResponse | null>(null);
  const [isLoadingSchedule, setIsLoadingSchedule] = useState<boolean>(false);
  const [error, setError] = useState<AxiosError<ProblemDetails> | null>(null);

  useEffect(() => {
    const loadSchedules = async () => {
      try {
        setIsLoading(true);
        setError(null);

        const scheduleList = await schedulesService.GetAllSchedules();
        setSchedules(scheduleList);
      } catch (error) {
        const axiosError = error as AxiosError<ProblemDetails>;
        const problem = axiosError.response?.data;

        toast.error(problem?.description || "Erro ao carregar cronogramas");
        setError(axiosError);
      } finally {
        setIsLoading(false);
      }
    };

    loadSchedules();
  }, []);

  const handleSelectSchedule = async (
    e: ChangeEvent<HTMLSelectElement, HTMLSelectElement>,
  ) => {
    e.preventDefault();

    try {
      setIsLoadingSchedule(true);
      const scheduleId = e.target.value;
      const scheduleDetails =
        await schedulesService.GetScheduleById(scheduleId);
      setSelectedSchedule(scheduleDetails);
    } catch (error) {
      const axiosError = error as AxiosError<ProblemDetails>;
      const problem = axiosError.response?.data;
      toast.error(problem?.description || "Erro ao carregar cronograma");
    } finally {
      setIsLoadingSchedule(false);
    }
  };

  const handleRefreshSchedule = async () => {
    if (!selectedSchedule) return;

    try {
      const freshDetails = await schedulesService.GetScheduleById(
        selectedSchedule.scheduleId,
      );
      setSelectedSchedule(freshDetails);
    } catch {
      toast.error("Falha ao carregar o cronograma");
    }
  };

  if (isLoading) return <Loading message="Carregando cronogramas..." />;

  if (error) {
    return (
      <Error
        statusCode={error?.response?.data.code || 500}
        title={error?.response?.data.description}
      />
    );
  }

  return (
    <div className="mx-auto space-y-2.5 sm:max-w-lg md:max-w-3xl lg:max-w-4xl xl:max-w-6xl">
      <Subtitle text="Cronograma Semanal" className="mb-6" />
      <Label className="text-lg">Selecione o cronograma</Label>
      <div className="flex space-x-2">
        <div className="group relative w-full">
          <NativeSelect
            id="schedules"
            className="w-full *:data-[slot=native-select]:pl-9"
            defaultValue=""
            onChange={handleSelectSchedule}
          >
            <NativeSelectOption value="" disabled>
              {schedules?.length
                ? "Escolha um cronograma para seguir"
                : "Crie um cronograma para começar"}
            </NativeSelectOption>
            {schedules?.map((item) => (
              <NativeSelectOption key={item.scheduleId} value={item.scheduleId}>
                {item.name}
              </NativeSelectOption>
            ))}
          </NativeSelect>
          <div className="text-muted-foreground/80 pointer-events-none absolute inset-y-0 left-0 flex items-center justify-center pl-3 group-has-[select[disabled]]:opacity-50">
            <CalendarDays size={16} aria-hidden="true" />
          </div>
        </div>
        <CreateSchedulePopover />
        {selectedSchedule && (
          <EditSchedulePopover schedule={selectedSchedule} />
        )}
      </div>
      {selectedSchedule ? (
        <ScheduleKanban
          selectedSchedule={selectedSchedule}
          isLoadingSchedule={isLoadingSchedule}
          onScheduleUpdated={handleRefreshSchedule}
        />
      ) : (
        <div className="text-muted-foreground rounded-xl border border-dashed py-12 text-center">
          Selecione um cronograma para vizualizar o quadro kanban.
        </div>
      )}
    </div>
  );
};

export default ScheduleSelection;
