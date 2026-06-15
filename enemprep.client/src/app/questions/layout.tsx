import React from "react";

interface IQuestionsLayoutProps {
  children: React.ReactNode;
}

const QuestionsLayout: React.FC<IQuestionsLayoutProps> = ({ children }) => {
  return <div className="w-full max-w-full">{children}</div>;
};

export default QuestionsLayout;
