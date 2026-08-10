import { ReadonlyURLSearchParams } from "next/navigation";

const usePageSize = (searchParams: ReadonlyURLSearchParams): number => {
  return Number(searchParams.get("pageSize") ?? 10);
};

export default usePageSize;