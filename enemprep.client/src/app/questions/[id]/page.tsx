"use client";

import React, { use, useEffect, useState } from "react";
import { IQuestionResponse } from "@/lib/types/question.interface";
import { questionsService } from "@/lib/api/questionsService";
import { toast } from "sonner";
import Loading from "@/components/Loading";
import { notFound } from "next/navigation";
import SolveQuestionStartScreen from "@/components/SolveQuestionStartScreen";

interface ISolveQuestionProps {
  params: Promise<{
    id: string;
  }>;
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
      } catch (error: any) {
        const backendError =
          error?.response?.data?.details || "Ocorreu um erro inesperado.";
        toast.error(backendError);
      } finally {
        setIsLoading(false);
      }
    };

    loadQuestion();
  }, [questionId]);

  if (isLoading || !question)
    return <Loading message="Carregando detalhes da questão..." />;

  return (
    <div className="mx-auto max-w-xl">
      {!hasStarted && <SolveQuestionStartScreen question={question} />}
    </div>
  );
};

export default SolveQuestion;
