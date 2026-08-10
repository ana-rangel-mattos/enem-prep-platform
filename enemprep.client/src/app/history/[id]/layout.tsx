import React from "react";

interface ISolvedQuestionDetailsLayoutProps {
  children: React.ReactNode;
}
const SolvedQuestionDetailsLayout: React.FC<ISolvedQuestionDetailsLayoutProps> = ({ children }) => {
  return <div className="w-full max-w-full">{children}</div>;
}

export default SolvedQuestionDetailsLayout;