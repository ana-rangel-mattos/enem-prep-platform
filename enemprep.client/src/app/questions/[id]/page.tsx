"use client";

import React, { use, useEffect, useState } from "react";
import { IQuestionResponse } from "@/lib/types/question.types";
import { questionsService } from "@/lib/api/questionsService";
import { toast } from "sonner";
import Loading from "@/components/Loading";
import { notFound } from "next/navigation";
import PracticeQuestionStartScreen from "@/components/Question/PracticeQuestionStartScreen";
import { AxiosError } from "axios";

interface ISolveQuestionProps {
  params: Promise<{ id: string }>;
}

const SolveQuestion: React.FC<ISolveQuestionProps> = ({ params }) => {
  const resolvedParams = use(params);

  const questionId = resolvedParams["id"];

  if (!questionId) {
    notFound();
  }

  const [question, setQuestion] = useState<IQuestionResponse | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [hasStarted, setHasStarted] = useState(false);

  useEffect(() => {
    const loadQuestion = async () => {
      try {
        const result = await questionsService.getQuestionById(questionId);
        setQuestion(result);
      } catch (error) {
        const axiosError = error as AxiosError<ProblemDetails>;
        const problem = axiosError.response?.data;
        toast.error(problem?.description || "Falha ao carregar questão.");
      } finally {
        setIsLoading(false);
      }
    };

    loadQuestion();
  }, [questionId]);

  if (isLoading || !question)
    return <Loading message="Carregando detalhes da questão..." />;

  return (
    <div className="max-w-4lx mx-auto w-full px-4 py-6">
      {!hasStarted && <PracticeQuestionStartScreen question={question} />}
    </div>
  );
};

export default SolveQuestion;
