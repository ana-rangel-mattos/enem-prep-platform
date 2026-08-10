"use client"

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
import SolvedQuestion from "@/components/SolvedQuestion";

const History = () => {
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [results, setResults] = useState<IPagedResponse<ISolvedQuestionResponse>>();

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
      } catch (error: any) {
        const backendError =
          error.response?.data || "Ocorreu um erro inesperado.";
        console.error(backendError);
        toast.error(backendError);
      } finally {
        setIsLoading(false);
      }
    }

    loadQuestions();
  }, [pageNumber, pageSize, sortBy]);

  if (isLoading) return <Loading message="Carregando histórico..." />
  return (
    <>
      <h1 className="text-2xl mb-6">Histórico de Resolução</h1>
      <div className="flex flex-1 items-center gap-y-6 flex-col">
        {results?.totalRecords ? (
          results.data.map((solvedQuestion: ISolvedQuestionResponse) => (
            <SolvedQuestion solvedQuestion={solvedQuestion} key={solvedQuestion.solvedQuestionId} />
          ))
        ) : (
          <p className="mx-auto max-w-max">
            Suas questões resolvidas aparecerão aqui!
          </p>
        )}
      </div>
    </>
  );
}

export default History;