import React, { useState } from "react";
import QuestionHeader from "@/components/Question/QuestionHeader";
import { IQuestionResponse } from "@/lib/types/question.types";
import Stopwatch from "@/components/Stopwatch";
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group";
import {
  Field,
  FieldContent,
  FieldGroup,
  FieldLabel,
  FieldTitle,
} from "@/components/ui/field";
import Image from "next/image";
import { Button } from "@/components/ui/button";
import { z } from "zod";
import { Controller, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import Loading from "@/components/Loading";
import { useRouter } from "next/navigation";
import { toast } from "sonner";
import { solvedQuestionsService } from "@/lib/api/solvedQuestionsService";

interface IPracticeQuestionSolverProps {
  question: IQuestionResponse;
}

const schema = z.object({
  questionId: z.uuid(),
  questionYear: z.int32(),
  correctAlternative: z.string(),
  chosenAlternative: z.enum(["A", "B", "C", "D", "E"]).optional(),
  startedAt: z.string(),
});

type FormData = z.infer<typeof schema>;

const PracticeQuestionSolver: React.FC<IPracticeQuestionSolverProps> = ({
  question,
}) => {
  const [hasFinished, setHasFinished] = useState<boolean>(true);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const router = useRouter();

  const { control, handleSubmit, reset } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: {
      questionId: question.questionId,
      questionYear: question.content.year,
      correctAlternative: question.content.correctAlternative,
      startedAt: new Date().toISOString(),
    },
  });

  const onSubmit = async (data: FormData) => {
    try {
      setIsLoading(true);

      const solvedQuestionId: string = await solvedQuestionsService.postAnswer({
        ...data,
        finishedAt: new Date().toISOString(),
      });
      router.push(`/history/${solvedQuestionId}`);

      toast.success("Questão submetida com sucesso!");
      reset();
    } catch (error: any) {
      const backendError =
        error.response?.data || "Ocorreu um erro inesperado.";
      toast.error(backendError);
    } finally {
      setHasFinished(false);
      setIsLoading(false);
    }
  };

  if (isLoading) return <Loading message="Enviando Resposta..." />;

  return (
    <div className="relative">
      <Stopwatch hasStarted={hasFinished} />
      <QuestionHeader question={question} />
      <form onSubmit={handleSubmit(onSubmit)} className="mt-4">
        <Controller
          name="chosenAlternative"
          control={control}
          render={({ field }) => (
            <FieldGroup>
              <RadioGroup
                value={field.value || ""}
                onValueChange={field.onChange}
              >
                {question.content.alternatives.map((alternative, index) => {
                  const itemId = `alt_${question.questionId}_${alternative.letter}`;

                  return (
                    <Field key={itemId} orientation="horizontal">
                      <FieldContent>
                        <FieldTitle>{alternative.letter}</FieldTitle>

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

                          <FieldLabel
                            htmlFor={itemId}
                            className="text-foreground flex-1 cursor-pointer font-normal"
                          >
                            {alternative.text}
                          </FieldLabel>
                        </div>
                      </FieldContent>

                      <RadioGroupItem
                        value={alternative.letter}
                        id={itemId}
                        className="mt-1"
                      />
                    </Field>
                  );
                })}
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
    </div>
  );
};

export default PracticeQuestionSolver;
