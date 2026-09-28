import React from "react";
import {
  Field,
  FieldContent,
  FieldLabel,
  FieldTitle,
} from "@/components/ui/field";
import Image from "next/image";
import { RadioGroupItem } from "@/components/ui/radio-group";
import { IQuestionAlternative } from "@/lib/types/question.types";

interface IQuestionAlternativeProps {
  alternative: Omit<IQuestionAlternative, "isCorrect"> &
    Partial<Pick<IQuestionAlternative, "isCorrect">>;
  questionId: string;
}

const QuestionAlternative: React.FC<IQuestionAlternativeProps> = ({
  alternative,
  questionId,
}) => {
  const itemId = `alt_${questionId}_${alternative.letter}`;

  return (
    <Field orientation="horizontal">
      <FieldContent>
        <div className="flex space-x-4">
          <FieldTitle>{alternative.letter}</FieldTitle>

          <FieldLabel
            htmlFor={itemId}
            className="text-foreground flex-1 cursor-pointer font-normal"
          >
            {alternative.text}
          </FieldLabel>
        </div>

        <div className="flex flex-1 flex-col gap-2">
          {alternative?.file && (
            <div className="relative h-40 w-full max-w-sm">
              <Image
                src={alternative.file}
                alt={`Imagem da alternativa ${alternative.letter}.`}
                fill
                className="object-contain"
              />
            </div>
          )}
        </div>
      </FieldContent>

      <RadioGroupItem value={alternative.letter} id={itemId} className="mt-1" />
    </Field>
  );
};

export default QuestionAlternative;
