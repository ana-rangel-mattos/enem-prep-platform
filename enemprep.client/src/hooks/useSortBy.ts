import { ReadonlyURLSearchParams } from "next/navigation";

const useSortBy = (searchParams: ReadonlyURLSearchParams): string | null => {
  return searchParams.get("sortBy");
}

export default useSortBy;