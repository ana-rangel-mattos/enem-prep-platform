"use client";

import { usePathname, useRouter, useSearchParams } from "next/navigation";
import usePageNumber from "@/hooks/usePageNumber";
import usePageSize from "@/hooks/usePageSize";
import useSearchTerm from "@/hooks/useSearchTerm";
import useSortBy from "@/hooks/useSortBy";
import { useEffect, useState } from "react";
import Loading from "@/components/Loading";
import { IExamResponse } from "@/lib/types/exams.types";
import { examsService } from "@/lib/api/examsService";
import Link from "next/link";
import Title from "@/components/ui/Title";
import ExamCard from "@/components/Exam/ExamCard";
import { AxiosError } from "axios";
import { toast } from "sonner";

const Exams = () => {
  const [result, setResult] = useState<IPagedResponse<IExamResponse> | null>(
    null,
  );
  const [isLoading, setIsLoading] = useState<boolean>(true);

  const router = useRouter();
  const pathName = usePathname();
  const searchParams = useSearchParams();

  const pageNumber = usePageNumber(searchParams);
  const pageSize = usePageSize(searchParams);
  const searchTerm = useSearchTerm(searchParams);
  const sortBy = useSortBy(searchParams);

  useEffect(() => {
    const loadExams = async () => {
      try {
        const result = await examsService.getAllExams({
          pageNumber,
          pageSize,
          searchTerm: searchTerm ?? undefined,
          sortBy: sortBy ?? undefined,
        });
        setResult(result);
      } catch (error) {
        const axiosError = error as AxiosError<ProblemDetails>;
        const problem = axiosError?.response?.data;
        toast.error(problem?.description || "Erro ao carregar simulados.");
      } finally {
        setIsLoading(false);
      }
    };

    loadExams();
  }, [pageNumber, pageSize, searchTerm, sortBy]);

  if (isLoading) return <Loading message="Carregando simulados..." />;

  return (
    <div>
      <Title text="Histórico de Simulados" />
      <Link
        href="/exams/new"
        className="hover:bg-primary hover:text-secondary top-6 right-6 my-5 block border-0 text-center transition-all duration-300 ease-out hover:cursor-pointer hover:rounded-xl hover:p-2 hover:text-lg hover:font-bold"
      >
        Criar Simulado
      </Link>
      <div>
        {result?.totalRecords ? (
          <div className="space-y-4">
            {result.data.map((exam) => (
              <ExamCard key={exam.examId} exam={exam} />
            ))}
          </div>
        ) : (
          <div className="mx-auto max-w-max">
            Seus simulados aparecerão aqui!
          </div>
        )}
      </div>
    </div>
  );
};

export default Exams;
