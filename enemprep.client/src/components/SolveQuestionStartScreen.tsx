import QuestionHeader from "@/components/QuestionHeader";
import { Button } from "@/components/ui/button";
import React from "react";
import { IQuestionResponse } from "@/lib/types/question.interface";

interface ISolveQuestionStartScreenProps {
  question: IQuestionResponse;
}

const SolveQuestionStartScreen: React.FC<ISolveQuestionStartScreenProps> = ({
  question,
}) => {
  return (
    <>
      <QuestionHeader question={question} />
      <Button
        size="sm"
        className="bg-secondary hover:bg-chart-3 hover:ring-offset-primary-foreground hover:ring-primary focus:ring-offset-primary-foreground mt-2 ml-auto block border-0 ring-2 ring-transparent transition-all duration-300 ease-out hover:cursor-pointer hover:ring-offset-2 focus:ring-offset-4 focus:outline-none"
      >
        Começar
      </Button>
    </>
  );
};

export default SolveQuestionStartScreen;
