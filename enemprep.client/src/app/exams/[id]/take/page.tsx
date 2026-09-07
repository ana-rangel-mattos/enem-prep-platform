"use client";

import React, { use, useEffect, useState } from "react";
import Loading from "@/components/Loading";
import { examsService } from "@/lib/api/examsService";
import { toast } from "sonner";
import Title from "@/components/Title";
import { IExamTake } from "@/lib/types/exams.types";
import TimerCountdown from "@/components/TimerCountdown";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import {
  Field,
  FieldContent,
  FieldGroup,
  FieldLabel,
  FieldTitle,
} from "@/components/ui/field";
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group";
import Image from "next/image";
import { Controller, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import ExamQuestion from "@/components/Exam/ExamQuestion";

interface ITakeExamProps {
  params: Promise<{ id: string }>;
}

const TakeExam: React.FC<ITakeExamProps> = ({ params }) => {
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [hasStarted, setHasStarted] = useState<boolean>(false);
  const [exam, setExam] = useState<IExamTake | null>(null);
  const [currentIndex, setCurrentIndex] = useState<number>(0);
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

  const { id } = use(params);

  useEffect(() => {
    async function fetchExam() {
      try {
        setIsLoading(true);
        const exam = await examsService.takeExam(id);
        setExam(exam);
      } catch (error: any) {
        const backendError =
          error?.response?.data?.details || "Ocorreu um erro inesperado.";
        toast.error(backendError);
      } finally {
        setIsLoading(false);
      }
    }

    fetchExam();
  }, [id]);

  const handleSubmitExam = async () => {
    if (isSubmitting || !exam) return;

    setIsSubmitting(true);
    toast.warning("Tempo esgotado! Enviando Simulado...");

    try {
      await examsService.submitExam(exam.examId);
      toast.success("Simulado enviado com sucesso!");
    } catch {
      toast.error("Erro ao enviar simulado.");
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) return <Loading message="Carregando simulado..." />;

  const currentQuestion = exam?.questions[currentIndex];

  if (exam) {
    return (
      <div className="pb-4">
        <Title text={exam.title} />
        <TimerCountdown
          className="mt-4"
          countStartSeconds={exam.questions.length * 3 * 60}
          hasStarted={hasStarted}
        />
        {!hasStarted ? (
          <Button
            size="sm"
            className="bg-secondary hover:bg-chart-3 hover:ring-offset-primary-foreground hover:ring-primary focus:ring-offset-primary-foreground mx-auto mt-2 block border-0 ring-2 ring-transparent transition-all duration-300 ease-out hover:cursor-pointer hover:ring-offset-2 focus:ring-offset-4 focus:outline-none"
            onClick={() => setHasStarted(true)}
          >
            Começar
          </Button>
        ) : (
          currentQuestion && (
            <ExamQuestion question={currentQuestion} examId={exam.examId} />
          )
        )}
      </div>
    );
  }
};

export default TakeExam;
