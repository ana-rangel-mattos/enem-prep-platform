"use client";

import {
  Kanban,
  KanbanBoard,
  KanbanColumn,
  KanbanColumnContent,
  KanbanColumnHandle,
  KanbanItem,
  KanbanItemHandle,
  KanbanOverlay,
} from "@/components/ui/kanban";
import React, { ComponentProps, useEffect, useState } from "react";
import { DayOfTheWeek } from "@/lib/types/enums.types";
import {
  IGetScheduleDetailsResponse,
  IScheduleSubjectResponse,
} from "@/lib/types/schedule.types";
import Loading from "@/components/Loading";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Calendar, GripVertical, Plus } from "lucide-react";
import { cn } from "@/lib/utils";

interface ISubjectCardProps extends Omit<
  ComponentProps<typeof KanbanItem>,
  "children" | "value"
> {
  subject: IScheduleSubjectResponse;
  isOverlay?: boolean;
}

const SubjectCard: React.FC<ISubjectCardProps> = ({
  subject,
  isOverlay,
  ...props
}) => {
  const card = (
    <div
      className={cn(
        "bg-background rounded-lg border p-3 shadow-xs transition-all",
        !isOverlay && "hover:border-primary/30 hover:shadow-sm",
        isOverlay && "ring-primary/20 rotate-1 shadow-lg ring-2",
      )}
    >
      <div className="mb-3 flex items-center justify-between gap-2">
        <span className="bg-muted text-muted-foreground rounded-md px-2 py-0.5 text-xs font-medium">
          {subject.weekday}
        </span>
        <span className="text-muted-foreground flex items-center gap-1 text-xs whitespace-nowrap tabular-nums">
          <Calendar className="size-3" />
          {/*{task.dueDate}*/}Due Date
        </span>
      </div>
      <div className="flex items-start gap-2.5">
        <span className={cn("mt-1.5 size-2 shrink-0 rounded-full")} />
        <p className="line-clamp-2 text-sm leading-snug font-medium">
          {subject.weekday}
        </p>
      </div>
    </div>
  );

  return (
    <KanbanItem value={subject.weekday.toString()} {...props}>
      {isOverlay ? card : <KanbanItemHandle>{card}</KanbanItemHandle>}
    </KanbanItem>
  );
};

interface ISubjectColumnProps extends Omit<
  ComponentProps<typeof KanbanColumn>,
  "children" | "value"
> {
  value: string;
  subjects: IScheduleSubjectResponse[];
  isOverlay?: boolean;
}

const SubjectColumn: React.FC<ISubjectColumnProps> = ({
  value,
  subjects,
  isOverlay,
  ...props
}) => {
  return (
    <KanbanColumn value={value} {...props}>
      <div className="bg-muted flex h-full flex-col rounded-xl border p-2">
        <div className="relative flex items-center justify-between px-3 pt-2 pb-4">
          <Button
            variant="outline"
            className="absolute right-0 hover:cursor-pointer"
          >
            <Plus className="text-foreground" />
            <span className="sr-only">Add Subject</span>
          </Button>
          <div className="flex min-w-0 items-center gap-2">
            <h3 className="text-sm font-semibold">{value}</h3>
            <Badge variant="outline">{subjects.length}</Badge>
          </div>
          <KanbanColumnHandle
            render={(handleProps) => (
              <Button
                {...handleProps}
                size="icon-xs"
                variant="ghost"
                aria-label={`Move ${value} column`}
              >
                <GripVertical />
              </Button>
            )}
          />
        </div>
        <KanbanColumnContent value={value} className="gap-2.5">
          {subjects.map((subject, index) => (
            <SubjectCard key={index} subject={subject} isOverlay={isOverlay} />
          ))}
        </KanbanColumnContent>
      </div>
    </KanbanColumn>
  );
};

interface IScheduleKanbanProps {
  selectedSchedule: IGetScheduleDetailsResponse;
  isLoadingSchedule: boolean;
}

const ScheduleKanban: React.FC<IScheduleKanbanProps> = ({
  selectedSchedule,
  isLoadingSchedule,
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
    <div className="space-y-4 overflow-x-auto">
      <Kanban
        value={columns}
        onValueChange={setColumns}
        className="mx-auto w-full sm:max-w-lg md:max-w-3xl lg:max-w-4xl xl:max-w-6xl"
      >
        <KanbanBoard className="grid auto-rows-fr grid-cols-[repeat(3,minmax(14rem,1fr))] gap-3">
          {Object.entries(columns).map(([day, subjects]) => (
            <SubjectColumn key={day} value={day} subjects={subjects} />
          ))}
        </KanbanBoard>
        <KanbanOverlay>
          {({ value, variant }) => {
            const activeValue = String(value);

            if (variant === "column") {
              return (
                <SubjectColumn
                  value={activeValue}
                  subjects={columns[activeValue] ?? []}
                  isOverlay
                />
              );
            }

            const subject = Object.values(columns)
              .flat()
              .find((t) => t.weekday.valueOf().toString() === activeValue);

            return subject ? <SubjectCard subject={subject} isOverlay /> : null;
          }}
        </KanbanOverlay>
      </Kanban>
    </div>
  );
};

export default ScheduleKanban;
