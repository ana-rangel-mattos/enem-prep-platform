import { Controller, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { IExamQuestion } from "@/lib/types/exams.types";
import {
  Field,
  FieldContent,
  FieldGroup,
  FieldLabel,
  FieldTitle,
} from "@/components/ui/field";
import { z } from "zod";
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group";
import React from "react";
import { forEach } from "eslint-config-next";
import Image from "next/image";
import QuestionHeader from "@/components/QuestionHeader";
import {
  Card,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import MarkdownRenderer from "@/components/MarkdownRenderer";
import { Button } from "@/components/ui/button";

interface IExamQuestionProps {
  question: IExamQuestion;
  examId: string;
}

const questionSchema = z.object({
  questionId: z.uuid(),
  examId: z.uuid(),
  chosenAlternative: z.enum(["A", "B", "C", "D", "E"]).optional(),
  timeSpentSeconds: z.number(),
});

type QuestionFormData = z.infer<typeof questionSchema>;

const ExamQuestion: React.FC<IExamQuestionProps> = ({ question, examId }) => {
  const { control, reset, handleSubmit } = useForm<QuestionFormData>({
    resolver: zodResolver(questionSchema),
    defaultValues: { examId: examId, questionId: question.examQuestionId },
  });

  const onSubmit = async () => {};

  return (
    <Card>
      <form className="mt-4" onSubmit={handleSubmit}>
        <CardHeader>
          <CardTitle>{question.year}</CardTitle>
          <MarkdownRenderer content={question.context} />
          <CardDescription className="text-foreground mt-4 font-semibold">
            {question.enunciation}
          </CardDescription>
        </CardHeader>
        <Controller
          name="chosenAlternative"
          control={control}
          render={({ field }) => (
            <FieldGroup>
              <RadioGroup
                value={field.value || ""}
                onValueChange={field.onChange}
              >
                {Object.entries(question.alternatives).map(
                  (alternative, index) => (
                    <Field
                      key={`${alternative[0]}-${index}`}
                      orientation="horizontal"
                    >
                      <FieldContent>
                        <FieldTitle>{alternative[0]}</FieldTitle>

                        <div className="flex flex-1 flex-col gap-2">
                          <FieldLabel
                            htmlFor={`${alternative[0]}-${index}`}
                            className="text-foreground flex-1 cursor-pointer font-normal"
                          >
                            {alternative[1]}
                          </FieldLabel>
                        </div>
                      </FieldContent>

                      <RadioGroupItem
                        value={alternative[0]}
                        id={`${alternative[0]}-${index}`}
                        className="mt-1"
                      />
                    </Field>
                  ),
                )}
              </RadioGroup>
            </FieldGroup>
          )}
        />
        <Button
          size="sm"
          type="submit"
          className="bg-secondary hover:bg-chart-3 hover:ring-offset-primary-foreground hover:ring-primary focus:ring-offset-primary-foreground mt-2 ml-auto block border-0 ring-2 ring-transparent transition-all duration-300 ease-out hover:cursor-pointer hover:ring-offset-2 focus:ring-offset-4 focus:outline-none"
        >
          Responder
        </Button>
      </form>
    </Card>
  );
};

export default ExamQuestion;
