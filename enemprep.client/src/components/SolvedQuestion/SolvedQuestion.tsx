"use client";

import { Card, CardTitle } from "@/components/ui/card";
import MarkdownRenderer from "@/components/MarkdownRenderer";
import { ISolvedQuestionResponse } from "@/lib/types/solved-question.types";
import React from "react";
import SolvedQuestionAlternative from "@/components/SolvedQuestion/SolvedQuestionAlternative";
import Link from "next/link";

interface ISolvedQuestionProps {
  solvedQuestion: ISolvedQuestionResponse;
}

const SolvedQuestion: React.FC<ISolvedQuestionProps> = ({ solvedQuestion }) => {
  return (
    <Card className="relative flex max-w-4xl flex-col items-center px-8 py-6">
      <CardTitle>ENEM {solvedQuestion.questionYear}</CardTitle>
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
        href={`/history/${solvedQuestion.solvedQuestionId}`}
        className="hover:bg-primary hover:text-secondary absolute top-6 right-6 block border-0 transition-all duration-300 ease-out hover:cursor-pointer hover:rounded-xl hover:p-2 hover:text-lg hover:font-bold"
      >
        Ver Mais
      </Link>
    </Card>
  );
};

export default SolvedQuestion;
