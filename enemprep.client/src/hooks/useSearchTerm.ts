import { ReadonlyURLSearchParams } from "next/navigation";

const useSearchTerm = (searchParams: ReadonlyURLSearchParams): string | null => {
  return searchParams.get("searchTerm");
}

export default useSearchTerm;