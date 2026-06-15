import React from "react";
import { CardContent } from "@/components/ui/card";
import { IQuestionResponse } from "@/lib/types/question.interface";
import QuestionAlternativeStaticItem from "@/components/QuestionAlternativeStaticItem";

interface IQuestionAlternativesStaticProps {
  question: IQuestionResponse;
}

const QuestionAlternativesStatic: React.FC<
  IQuestionAlternativesStaticProps
> = ({ question }) => {
  return (
    <CardContent className="-mb-(--card-spacing)">
      <ul>
        {question.content.alternatives.map((alternative, index) => (
          <QuestionAlternativeStaticItem
            alternative={alternative}
            key={question.questionId + index}
          />
        ))}
      </ul>
    </CardContent>
  );
};

export default QuestionAlternativesStatic;
