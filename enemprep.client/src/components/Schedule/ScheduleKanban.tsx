"use client";

import React, { useEffect, useState } from "react";
import { DayOfTheWeek } from "@/lib/types/enums.types";
import {
  IGetScheduleDetailsResponse,
  IScheduleSubjectResponse,
} from "@/lib/types/schedule.types";
import Loading from "@/components/Loading";
import { Button } from "@/components/ui/button";
import { DragDropProvider, useDraggable, useDroppable } from "@dnd-kit/react";
import AddSubjectPopover from "@/components/Schedule/AddSubjectPopover";
import { schedulesService } from "@/lib/api/schedulesService";
import { toast } from "sonner";
import { AxiosError } from "axios";

interface IScheduleKanbanProps {
  selectedSchedule: IGetScheduleDetailsResponse;
  isLoadingSchedule: boolean;
  onScheduleUpdated: () => void;
}

const ScheduleKanban: React.FC<IScheduleKanbanProps> = ({
  selectedSchedule,
  isLoadingSchedule,
  onScheduleUpdated,
}) => {
  const [columns, setColumns] = useState<
    Record<DayOfTheWeek, IScheduleSubjectResponse[]>
  >(selectedSchedule.schedule);

  useEffect(() => {
    const updateColumns = () => {
      setColumns(selectedSchedule.schedule);
    };

    updateColumns();
  }, [selectedSchedule]);

  if (isLoadingSchedule)
    return <Loading message="Selecionando cronograma..." />;

  return (
    <DragDropProvider
      onDragEnd={async (event) => {
        const { source, target } = event.operation;

        if (event.canceled || !target || !source) return;

        const activeId = source.id as string;
        let targetColumnKey: DayOfTheWeek | null = null;
        let targetItems: IScheduleSubjectResponse[] = [];

        if (Object.keys(columns).includes(target.id as string)) {
          targetColumnKey = target.id as DayOfTheWeek;
          targetItems = columns[targetColumnKey] || [];
        } else {
          for (const [day, items] of Object.entries(columns)) {
            if (items.some((sub) => sub.scheduleSubjectId === target.id)) {
              targetColumnKey = day as unknown as DayOfTheWeek;
              targetItems = items;
              break;
            }
          }
        }

        if (!targetColumnKey) return;

        let newIndex = targetItems.length;

        if (!Object.keys(columns).includes(target.id as string)) {
          const overIndex = targetItems.findIndex(
            (sub) => sub.scheduleSubjectId === target.id,
          );

          if (overIndex !== -1) {
            newIndex = overIndex;
          }
        }

        const payload = {
          scheduleSubjectId: activeId,
          newDay: DayOfTheWeek[targetColumnKey].valueOf(),
          newIndex: newIndex,
        };

        console.log("Sending reorder payload:", payload);

        try {
          await schedulesService.UpdateScheduleOrder(
            selectedSchedule.scheduleId,
            payload,
          );

          toast.success("Cronograma atualizado com sucesso!");
          onScheduleUpdated();
        } catch (error) {
          const axiosError = error as AxiosError<ProblemDetails>;
          toast.error(
            axiosError.response?.data.description ||
              "Erro ao atualizar o cronograma.",
          );
        }
      }}
    >
      <div className="text-muted-foreground rounded-xl border py-6 text-center font-bold">
        {selectedSchedule.name}
      </div>
      <div className="flex min-w-full space-x-4 overflow-x-auto py-2">
        {Object.entries(columns).map(([column, items]) => (
          <div className="flex flex-col gap-2" key={column}>
            <div className="bg-muted flex max-h-3.5 min-h-10 w-full justify-between rounded-lg border px-3.5 py-2">
              <h3 className="mb-2 font-semibold">{column}</h3>
              <AddSubjectPopover
                scheduleId={selectedSchedule.scheduleId}
                weekday={DayOfTheWeek[column as unknown as DayOfTheWeek]}
                onSubjectAdded={onScheduleUpdated}
              />
            </div>
            <ScheduleColumn weekday={column}>
              {items.map((subject) => (
                <ScheduleItem
                  key={subject.scheduleSubjectId}
                  scheduleSubjectId={subject.scheduleSubjectId}
                >
                  <h4>{subject.subjectName}</h4>
                </ScheduleItem>
              ))}
            </ScheduleColumn>
          </div>
        ))}
      </div>
    </DragDropProvider>
  );
};

interface IScheduleColumnProps {
  weekday: string;
  children: React.ReactNode;
}

const ScheduleColumn: React.FC<IScheduleColumnProps> = ({
  children,
  weekday,
}) => {
  const { isDropTarget, ref } = useDroppable({ id: weekday });

  return (
    <div
      ref={ref}
      className={`bg-muted/50 flex min-h-72 min-w-64 flex-col gap-2 rounded-xl border p-4 ${isDropTarget ? "border-primary bg-primary/5" : ""}`}
    >
      {children}
    </div>
  );
};

interface IScheduleItemProps {
  scheduleSubjectId: string;
  children: React.ReactNode;
}

const ScheduleItem: React.FC<IScheduleItemProps> = ({
  scheduleSubjectId,
  children,
}) => {
  const { ref } = useDraggable({ id: scheduleSubjectId });

  return (
    <div ref={ref} className="touch-none">
      <Button
        variant="outline"
        className="pointer-events-none w-full justify-start hover:cursor-grab active:cursor-grabbing"
      >
        {children}
      </Button>
    </div>
  );
};

export default ScheduleKanban;
