"use client"

import React, { use, useEffect, useState } from "react";
import { notFound } from "next/navigation";
import { ISolvedQuestionResponse } from "@/lib/types/solved-question.types";
import { solvedQuestionsService } from "@/lib/api/solvedQuestionsService";
import { toast } from "sonner";
import Loading from "@/components/Loading";
import { Card, CardTitle } from "@/components/ui/card";
import MarkdownRenderer from "@/components/MarkdownRenderer";
import SolvedQuestionAlternative from "@/components/SolvedQuestionAlternative";
import Error from "next/error";
import { timeToString } from "@/lib/helpers";
import { Button } from "@/components/ui/button";
import Link from "next/link";

interface ISolvedQuestionDetailsProps {
  params: Promise<{
    id: string;
  }>;
}

const SolvedQuestionDetails: React.FC<ISolvedQuestionDetailsProps> = ({ params }) => {
  const resolvedParams = use(params);

  const solvedQuestionId = resolvedParams["id"];

  if (!solvedQuestionId) {
    notFound();
  }

  const [solvedQuestion, setSolvedQuestion] = useState<ISolvedQuestionResponse | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    const loadSolvedQuestion = async () => {
      try {
        const result = await solvedQuestionsService.getSolvedQuestionById(solvedQuestionId);
        setSolvedQuestion(result);
      } catch (error: any) {
        const backendError =
          error?.response?.data?.details || "Ocorreu um erro inesperado.";
        toast.error(backendError);
      } finally {
        setIsLoading(false);
      }
    }

    loadSolvedQuestion();
  }, [solvedQuestionId]);

  if (isLoading) {
    return <Loading message="Carregando questão..." />
  }

  if (!solvedQuestion) {
    return <Error statusCode={404} />
  }

  return (
    <div className="mx-auto w-full">
      <Card className="relative px-8 py-6">
        <CardTitle>ENEM {solvedQuestion.questionYear}</CardTitle>
        <div className="absolute top-6 right-6 font-bold">
          <p className="text-foreground text-xl">
            Tempo de Resolução:
            <span className="text-secondary bg-primary ml-2 rounded-lg p-2 text-2xl">
              {timeToString(solvedQuestion.timeSpent)}
            </span>
          </p>
        </div>
        {solvedQuestion?.questionText && (
          <MarkdownRenderer content={solvedQuestion.questionText} />
        )}
        <SolvedQuestionAlternative
          alternativeTitle="Alternativa escolhida:"
          alternativeLetter={solvedQuestion.chosenAlternative}
          alternativeText={solvedQuestion.chosenAlternativeText}
          isCorrect={solvedQuestion.isCorrect}
        />
        <SolvedQuestionAlternative
          alternativeTitle="Alternativa correta:"
          alternativeLetter={solvedQuestion.correctAlternative}
          alternativeText={solvedQuestion.correctAlternativeText}
          isCorrect={true}
        />
        <Link
          href={`/questions/${solvedQuestion.questionId}`}
          className="hover:bg-primary hover:text-secondary w-45 text-center block transition-all duration-300 ease-out hover:cursor-pointer rounded-xl p-2 hover:text-lg hover:font-bold"
        >
          Resolver Questão
        </Link>
      </Card>
    </div>
  );
}

export default SolvedQuestionDetails;