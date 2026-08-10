import React from "react";

interface IHistoryLayoutProps {
  children: React.ReactNode;
}

const HistoryLayout: React.FC<IHistoryLayoutProps> = ({ children }) => {
  return <div className="w-full max-w-full overflow-y-auto">{children}</div>;
}

export default HistoryLayout;