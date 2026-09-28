import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { IExamResponse } from "@/lib/types/exams.types";
import React from "react";
import Text from "@/components/ui/Text";
import { Button } from "@/components/ui/button";

interface IExamCardProps {
  exam: IExamResponse;
}

const ExamCard: React.FC<IExamCardProps> = ({ exam }) => {
  return (
    <Card className="mx-auto max-w-2xl">
      <CardHeader>
        <CardTitle className="relative text-xl">
          {exam.title}
          <Badge className="absolute right-0">{exam.statusDescription}</Badge>
        </CardTitle>{" "}
        <CardDescription className="my-3">
          <h3 className="mb-1.5 text-lg font-bold">
            Informações sobre a prova:
          </h3>
          <Text className="contents">Idioma: </Text>
          {exam.languageDescription}
          <br />
          <Text className="contents">Tempo Disponível: </Text>
          {exam.maxSpentTime}
          <br />
          <Text className="contents">Tempo Gasto: </Text>
          {exam.totalSpentTime}
        </CardDescription>
        <CardContent className="mb-3">
          <h3 className="mb-1.5 text-lg font-bold">Sobre as Questões:</h3>
          <Text className="contents">Questões Corretas: </Text>
          {exam.correctQuestionCount}
          <br />
          <Text className="contents">Questões Incorretas: </Text>
          {exam.incorrectQuestionCount}
          <br />
          <Text className="contents">Questões Em Branco: </Text>
          {exam.incorrectQuestionCount}
        </CardContent>
        <CardFooter>
          <Text className="contents">TRI Estimado: </Text>
          {exam.estimatedScore}
        </CardFooter>
        <Button className="hover:cursor-pointer">Detalhes</Button>
      </CardHeader>

      <CardContent></CardContent>
    </Card>
  );
};

export default ExamCard;
