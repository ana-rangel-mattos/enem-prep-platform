import React from "react";
import BaseLayout from "@/components/Layouts/BaseLayout";

interface IExamsLayoutProps {
  children: React.ReactNode;
}

const ExamsLayout: React.FC<IExamsLayoutProps> = ({ children }) => {
  return <BaseLayout>{children}</BaseLayout>;
};

export default ExamsLayout;
