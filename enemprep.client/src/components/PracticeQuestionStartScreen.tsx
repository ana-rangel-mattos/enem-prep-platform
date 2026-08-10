import QuestionHeader from "@/components/QuestionHeader";
import { Button } from "@/components/ui/button";
import React, { useState } from "react";
import { IQuestionResponse } from "@/lib/types/question.types";
import PracticeQuestionSolver from "@/components/PracticeQuestionSolver";

interface IPracticeQuestionStartScreenProps {
  question: IQuestionResponse;
}

const PracticeQuestionStartScreen: React.FC<
  IPracticeQuestionStartScreenProps
> = ({ question }) => {
  const [hasStarted, setHasStarted] = useState(false);

  if (hasStarted) {
    return <PracticeQuestionSolver question={question} />;
  }

  return (
    <>
      <QuestionHeader question={question} />
      <Button
        size="sm"
        className="bg-secondary hover:bg-chart-3 hover:ring-offset-primary-foreground hover:ring-primary focus:ring-offset-primary-foreground mt-2 ml-auto block border-0 ring-2 ring-transparent transition-all duration-300 ease-out hover:cursor-pointer hover:ring-offset-2 focus:ring-offset-4 focus:outline-none"
        onClick={() => setHasStarted(true)}
      >
        Começar
      </Button>
    </>
  );
};

export default PracticeQuestionStartScreen;
