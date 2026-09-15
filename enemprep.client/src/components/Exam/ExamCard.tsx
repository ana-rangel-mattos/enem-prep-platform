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
import Text from "@/components/Text";

interface IExamCardProps {
  exam: IExamResponse;
}

const ExamCard: React.FC<IExamCardProps> = ({ exam }) => {
  return (
    <Card className="mx-auto max-w-xl">
      <CardHeader>
        <CardTitle className="relative text-xl">
          {exam.title}
          <Badge className="absolute right-0">{exam.statusDescription}</Badge>
        </CardTitle>{" "}
        <CardDescription>
          <p>
            <Text className="contents">Idioma: </Text>
            {exam.languageDescription}
          </p>
        </CardDescription>
        <CardContent>
          <p>
            <Text className="contents">Questões Corretas: </Text>
            {exam.correctQuestionCount}
          </p>
          <p>
            <Text className="contents">Questões Incorretas: </Text>
            {exam.incorrectQuestionCount}
          </p>
          <p>
            <Text className="contents">Questões Em Branco: </Text>
            {exam.incorrectQuestionCount}
          </p>
        </CardContent>
        <CardFooter>
          <p>
            <Text className="contents">TRI Estimado: </Text>
            {exam.estimatedScore}
          </p>
        </CardFooter>
      </CardHeader>

      <CardContent></CardContent>
    </Card>
  );
};

export default ExamCard;
