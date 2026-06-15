import React from "react";
import MarkdownRenderer from "@/components/MarkdownRenderer";
import { IQuestionAlternative } from "@/lib/types/question.interface";

interface IQuestionAlternativeStaticItemProps {
  alternative: IQuestionAlternative;
}

const QuestionAlternativeStaticItem: React.FC<
  IQuestionAlternativeStaticItemProps
> = ({ alternative }) => {
  return (
    <li className="bg-muted/50 -mx-(--card-spacing) max-h-48 border-t px-(--card-spacing) py-4 text-sm leading-relaxed">
      <p>
        <span className="text-md text-mist-500">{alternative.letter})</span>{" "}
        <span>{alternative.text}</span>
      </p>
      {alternative.file && <MarkdownRenderer content={alternative.file} />}
    </li>
  );
};

export default QuestionAlternativeStaticItem;
