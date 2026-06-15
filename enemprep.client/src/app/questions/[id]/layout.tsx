import React from "react";

interface ISolveQuestionLayoutProps {
  children: React.ReactNode;
}

const SolveQuestionLayout: React.FC<ISolveQuestionLayoutProps> = ({
  children,
}) => {
  return <div className="w-full max-w-full">{children}</div>;
};

export default SolveQuestionLayout;
