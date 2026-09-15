"use client";

import React, { use, useEffect, useState } from "react";
import Loading from "@/components/Loading";
import { examsService } from "@/lib/api/examsService";
import { toast } from "sonner";
import Title from "@/components/Title";
import { IExamQuestion, IExamTake } from "@/lib/types/exams.types";
import TimerCountdown from "@/components/TimerCountdown";
import { Button } from "@/components/ui/button";
import ExamQuestion from "@/components/Exam/ExamQuestion";
import Text from "@/components/Text";
import { useRouter } from "next/navigation";
import { ExamStatus } from "@/lib/types/enums.types";
import { AxiosError } from "axios";

interface ITakeExamProps {
  params: Promise<{ id: string }>;
}

const TakeExam: React.FC<ITakeExamProps> = ({ params }) => {
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [hasStarted, setHasStarted] = useState<boolean>(false);
  const [exam, setExam] = useState<IExamTake | null>(null);
  const [currentIndex, setCurrentIndex] = useState<number>(0);
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [currentQuestion, setCurrentQuestion] = useState<
    undefined | IExamQuestion
  >(undefined);

  const { id } = use(params);

  const router = useRouter();

  useEffect(() => {
    async function fetchExam() {
      try {
        setIsLoading(true);
        const exam = await examsService.takeExam(id);
        setExam(exam);
        setCurrentQuestion(exam.questions[0]);
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

    try {
      await examsService.updateExamStatus({
        examId: exam.examId,
        newStatus: ExamStatus.Finished.valueOf(),
      });
      await examsService.submitExam(exam.examId);
      toast.success("Simulado enviado com sucesso!");
      // router.push(`/exams/${exam.examId}/details`);
      router.push(`/exams`);
    } catch (error) {
      const axiosError = error as AxiosError<ProblemDetails>;

      console.error(axiosError.response?.data);
      toast.error("Erro ao enviar simulado.");
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) return <Loading message="Carregando simulado..." />;

  const handleNextQuestion = () => {
    setCurrentIndex((prev) => prev + 1);
    setCurrentQuestion(exam?.questions[currentIndex]);
  };

  const handleStartExam = async () => {
    if (!exam) return;

    try {
      await examsService.updateExamStatus({
        examId: exam?.examId,
        newStatus: ExamStatus.InProgress.valueOf(),
      });
      setHasStarted(true);
    } catch (error) {
      const axiosError = error as AxiosError<ProblemDetails>;
      const problem = axiosError?.response?.data;
      toast.error(`Erro ao começar simulado: ${problem?.description}`);
    }
  };

  if (exam) {
    return (
      <div className="pb-4">
        <Title text={exam.title} />
        <TimerCountdown
          className="mt-4"
          countStartSeconds={exam.questions.length * 3 * 60}
          hasStarted={hasStarted}
          onExpire={handleSubmitExam}
        />
        {!hasStarted ? (
          <Button
            size="sm"
            className="bg-secondary hover:bg-chart-3 hover:ring-offset-primary-foreground hover:ring-primary focus:ring-offset-primary-foreground mx-auto mt-2 block border-0 ring-2 ring-transparent transition-all duration-300 ease-out hover:cursor-pointer hover:ring-offset-2 focus:ring-offset-4 focus:outline-none"
            onClick={() => handleStartExam()}
          >
            Começar
          </Button>
        ) : (
          currentIndex < exam.questions.length &&
          currentQuestion && (
            <>
              <Text size="lg" variant="primary" bold className="mb-2">
                QUESTÃO {currentIndex + 1} / {exam.questions.length}
              </Text>
              <ExamQuestion
                question={currentQuestion}
                onNextQuestion={
                  currentIndex === exam.questions.length - 1
                    ? handleSubmitExam
                    : handleNextQuestion
                }
              />
            </>
          )
        )}
      </div>
    );
  }
};

export default TakeExam;
