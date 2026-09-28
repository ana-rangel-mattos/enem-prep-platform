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
import React, { useState } from "react";
import QuestionHeader from "@/components/Question/QuestionHeader";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import Image from "next/image";
import { examsService } from "@/lib/api/examsService";
import { toast } from "sonner";
import QuestionAlternative from "@/components/Question/QuestionAlternative";

interface IExamQuestionProps {
  question: IExamQuestion;
  onNextQuestion: () => void;
}

const questionSchema = z.object({
  questionId: z.uuid(),
  examId: z.uuid(),
  chosenAlternative: z.enum(["A", "B", "C", "D", "E"]).optional(),
});

type QuestionFormData = z.infer<typeof questionSchema>;

const ExamQuestion: React.FC<IExamQuestionProps> = ({
  question,
  onNextQuestion,
}) => {
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [startedAt] = useState<Date>(new Date());

  const { control, reset, handleSubmit } = useForm<QuestionFormData>({
    resolver: zodResolver(questionSchema),
    defaultValues: { examId: question.examId, questionId: question.questionId },
  });

  const onSubmitAnswer = async (data: QuestionFormData) => {
    try {
      setIsLoading(true);

      await examsService.submitAnswer({
        examId: question.examId,
        questionId: question.questionId,
        timeSpentInSeconds: Math.abs(
          Math.floor((new Date().getTime() - startedAt.getTime()) / 1000),
        ),
        chosenAlternative: data.chosenAlternative,
      });

      toast.success("Questão submetida com sucesso!", {
        position: "top-right",
      });
      reset();
      onNextQuestion();
    } catch (error: any) {
      const backendError =
        error.response?.data || "Ocorreu um erro inesperado.";
      toast.error(backendError, { position: "top-right" });
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Card>
      <QuestionHeader question={question} />
      <CardContent>
        <form className="mt-4" onSubmit={handleSubmit(onSubmitAnswer)}>
          <Controller
            name="chosenAlternative"
            control={control}
            render={({ field }) => (
              <FieldGroup>
                <RadioGroup
                  value={field.value || ""}
                  onValueChange={field.onChange}
                  className="space-y-2"
                >
                  {question.content.alternatives.map((alternative, index) => (
                    <QuestionAlternative
                      key={`${alternative.letter}_${index}`}
                      alternative={alternative}
                      questionId={question.questionId}
                    />
                  ))}
                </RadioGroup>
              </FieldGroup>
            )}
          />
          <Button
            size="sm"
            type="submit"
            disabled={isLoading}
            className="bg-secondary hover:bg-chart-3 hover:ring-offset-primary-foreground hover:ring-primary focus:ring-offset-primary-foreground mt-4 mr-auto block border-0 ring-2 ring-transparent transition-all duration-300 ease-out hover:cursor-pointer hover:ring-offset-2 focus:ring-offset-4 focus:outline-none"
          >
            {isLoading ? "Enviando resposta" : "Responder"}
          </Button>
        </form>
      </CardContent>
    </Card>
  );
};

export default ExamQuestion;
