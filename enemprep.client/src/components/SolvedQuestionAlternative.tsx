"use client"

import { CardDescription } from "@/components/ui/card";
import React from "react";

interface ISolvedQuestionAlternativeProps {
  alternativeTitle: string;
  alternativeLetter?: string;
  alternativeText?: string;
  isCorrect: boolean;
}

const SolvedQuestionAlternative: React.FC<ISolvedQuestionAlternativeProps> = ({ alternativeTitle, alternativeText, alternativeLetter, isCorrect}) => {
  return (
    <CardDescription
      className={`text-foreground w-full text-base rounded-xl px-3.5 py-2 ${!alternativeText ? "bg-gray-600" : isCorrect ? "bg-emerald-700" : "bg-red-900"}`}
    >
      {!alternativeText ? (
        <p>Nenhuma alternativa foi escolhida.</p>
      ) : (
        <>
          <p>{alternativeTitle}</p>
          <p>
            {alternativeLetter}) {alternativeText}
          </p>
        </>
      )}
    </CardDescription>
  );
}

export default SolvedQuestionAlternative;