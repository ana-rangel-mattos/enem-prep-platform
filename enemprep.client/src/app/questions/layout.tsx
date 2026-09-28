import React from "react";

const QuestionsLayout: React.FC<ILayout> = ({ children }) => {
  return <div className="w-full max-w-full overflow-y-auto">{children}</div>;
};

export default QuestionsLayout;
