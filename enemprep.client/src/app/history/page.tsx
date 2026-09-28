"use client";

import { useEffect, useState } from "react";
import Loading from "@/components/Loading";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import usePageNumber from "@/hooks/usePageNumber";
import usePageSize from "@/hooks/usePageSize";
import useSortBy from "@/hooks/useSortBy";
import { solvedQuestionsService } from "@/lib/api/solvedQuestionsService";
import { ISolvedQuestionResponse } from "@/lib/types/solved-question.types";
import { Card, CardDescription, CardTitle } from "@/components/ui/card";
import MarkdownRenderer from "@/components/MarkdownRenderer";
import { toast } from "sonner";
import SolveQuestion from "@/app/questions/[id]/page";
import SolvedQuestion from "@/components/SolvedQuestion/SolvedQuestion";
import { AxiosError } from "axios";
import Title from "@/components/ui/Title";

const History = () => {
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [results, setResults] =
    useState<IPagedResponse<ISolvedQuestionResponse>>();

  const router = useRouter();
  const pathName = usePathname();
  const searchParams = useSearchParams();

  const pageNumber = usePageNumber(searchParams);
  const pageSize = usePageSize(searchParams);
  const sortBy = useSortBy(searchParams);

  useEffect(() => {
    const loadQuestions = async () => {
      try {
        const result = await solvedQuestionsService.getSolvedQuestions({
          pageNumber,
          pageSize,
          sortBy: sortBy ?? undefined,
        });
        setResults(result);
      } catch (error) {
        const axiosError = error as AxiosError<ProblemDetails>;
        const problem = axiosError?.response?.data;
        toast.error(problem?.description || "Erro ao carregar questões.");
      } finally {
        setIsLoading(false);
      }
    };

    loadQuestions();
  }, [pageNumber, pageSize, sortBy]);

  if (isLoading) return <Loading message="Carregando histórico..." />;
  return (
    <>
      <Title text="Histórico de Resolução" className="mb-6" />
      <div className="flex flex-1 flex-col items-center gap-y-6">
        {results?.totalRecords ? (
          results.data.map((solvedQuestion: ISolvedQuestionResponse) => (
            <SolvedQuestion
              solvedQuestion={solvedQuestion}
              key={solvedQuestion.solvedQuestionId}
            />
          ))
        ) : (
          <p className="mx-auto max-w-max">
            Suas questões resolvidas aparecerão aqui!
          </p>
        )}
      </div>
    </>
  );
};

export default History;
