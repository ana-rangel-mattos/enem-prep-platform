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
import Title from "@/components/Title";
import {
  Card,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";

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
      <div className="flex flex-wrap items-baseline">
        {result?.totalRecords ? (
          <div>
            {result.data.map((exam) => (
              <Card key={exam.examId}>
                <CardHeader>
                  <CardTitle>{exam.title}</CardTitle>{" "}
                  <Badge>{exam.statusDescription}</Badge>
                  <CardDescription>{exam.languageDescription}</CardDescription>
                </CardHeader>
              </Card>
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
