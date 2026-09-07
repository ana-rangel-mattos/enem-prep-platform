import React from "react";
import BaseLayout from "@/components/Layouts/BaseLayout";

interface IHistoryLayoutProps {
  children: React.ReactNode;
}

const HistoryLayout: React.FC<IHistoryLayoutProps> = ({ children }) => {
  return <BaseLayout>{children}</BaseLayout>;
};

export default HistoryLayout;
