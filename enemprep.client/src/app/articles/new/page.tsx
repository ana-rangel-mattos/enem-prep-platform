"use client";

import ArticleEditor from "@/components/Article/ArticleEditor";
import { IArticleBlock } from "@/lib/types/article.types";

const CreateNewArticle = () => {
  const handleSaveArticle = (blocks: IArticleBlock[]) => {};
  return <ArticleEditor onSave={handleSaveArticle} />;
};

export default CreateNewArticle;
