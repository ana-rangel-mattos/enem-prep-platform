import { ReadonlyURLSearchParams } from "next/navigation";

const usePageNumber = (searchParams: ReadonlyURLSearchParams): number => {
  return Number(searchParams.get("pageNumber") ?? 1);
}

export default usePageNumber;