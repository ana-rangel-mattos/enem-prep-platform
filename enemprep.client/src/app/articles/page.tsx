import Title from "@/components/ui/Title";
import Link from "next/link";

const Articles = () => {
  return (
    <>
      <Title text="Articles" className="mb-6" />
      <Link
        href="/articles/new"
        className="hover:bg-secondary/30 rounded-md p-2 transition-all duration-300 ease-in-out hover:cursor-pointer hover:text-lg"
      >
        Criar Artigo
      </Link>
    </>
  );
};

export default Articles;
