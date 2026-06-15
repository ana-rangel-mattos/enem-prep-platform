"use client";

import { useEffect, useState } from "react";
import { IQuestionResponse } from "@/lib/types/question.interface";
import { questionsService } from "@/lib/api/questionsService";
import Loading from "@/components/Loading";
import { Card } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { PencilLine } from "lucide-react";
import Link from "next/link";
import QuestionHeader from "@/components/QuestionHeader";
import QuestionAlternativesStatic from "@/components/QuestionAlternativesStatic";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationNext,
  PaginationPrevious,
} from "@/components/ui/pagination";
import { Field, FieldLabel } from "@/components/ui/field";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

const Questions = () => {
  const [result, setResult] = useState<IPagedResponse<IQuestionResponse>>();
  const [isLoading, setIsLoading] = useState(true);

  const router = useRouter();
  const pathName = usePathname();
  const searchParams = useSearchParams();

  const pageNumber = Number(searchParams.get("pageNumber") ?? 1);
  const pageSize = Number(searchParams.get("pageSize") ?? 10);
  const searchTerm = searchParams.get("searchTerm");
  const sortBy = searchParams.get("sortBy");

  useEffect(() => {
    const loadQuestions = async () => {
      try {
        const results = await questionsService.getQuestions({
          pageNumber,
          pageSize,
          searchTerm: searchTerm ?? undefined,
          sortBy: sortBy ?? undefined,
        });
        setResult(results);
      } finally {
        setIsLoading(false);
      }
    };

    loadQuestions();
  }, [pageSize, pageNumber, searchTerm, sortBy]);

  if (isLoading) return <Loading message="Carregando questões..." />;

  return (
    <div>
      <div className="flex flex-wrap items-baseline gap-4">
        {result &&
          result.data.map((question) => (
            <Card
              className="relative mx-auto w-full sm:max-w-sm md:max-w-md"
              key={question.questionId}
            >
              <Link href={`/questions/${question.questionId}`}>
                <Button
                  variant="outline"
                  size="sm"
                  className="absolute top-2 right-2 text-emerald-500"
                >
                  <PencilLine /> Resolver Questão
                </Button>
              </Link>
              <QuestionHeader question={question} />
              <QuestionAlternativesStatic question={question} />
            </Card>
          ))}
      </div>

      <div className="mt-4 flex">
        <Field orientation="horizontal" className="">
          <FieldLabel htmlFor="select-questions-per-page">
            Questões por Pagína
          </FieldLabel>
          <Select
            defaultValue="10"
            value={pageSize.toString()}
            onValueChange={(value) => {
              router.push(`${pathName}?pageNumber=1&pageSize=${value}`);
            }}
          >
            <SelectTrigger className="w-20" id="select-questions-per-page">
              <SelectValue />
            </SelectTrigger>
            <SelectContent align="start">
              <SelectGroup>
                <SelectItem value="5">5</SelectItem>
                <SelectItem value="10">10</SelectItem>
                <SelectItem value="25">25</SelectItem>
                <SelectItem value="50">50</SelectItem>
              </SelectGroup>
            </SelectContent>
          </Select>
        </Field>
        <Pagination className="mx-0 w-auto">
          <PaginationContent>
            {result?.hasPreviousPage && (
              <PaginationItem>
                <PaginationPrevious
                  href={`${pathName}?pageNumber=${pageNumber - 1}&pageSize=${pageSize}`}
                />
              </PaginationItem>
            )}

            {result?.hasNextPage && (
              <PaginationItem>
                <PaginationNext
                  href={`${pathName}?pageNumber=${pageNumber + 1}&pageSize=${pageSize}`}
                />
              </PaginationItem>
            )}
          </PaginationContent>
        </Pagination>
      </div>
    </div>
  );
};

export default Questions;
