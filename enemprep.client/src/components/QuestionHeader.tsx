import React from "react";
import { CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import MarkdownRenderer from "@/components/MarkdownRenderer";
import { IQuestionResponse } from "@/lib/types/question.interface";

interface IQuestionHeaderProps {
  question: IQuestionResponse;
}

const QuestionHeader: React.FC<IQuestionHeaderProps> = ({ question }) => {
  return (
    <CardHeader>
      <CardTitle>{question.content.title}</CardTitle>
      <MarkdownRenderer content={question.content.context} />
      <CardDescription className="text-foreground mt-4 font-semibold">
        {question.content.alternativesIntroduction}
      </CardDescription>
    </CardHeader>
  );
};

export default QuestionHeader;
