import React from "react";
import BaseLayout from "@/components/Layouts/BaseLayout";

const HistoryLayout: React.FC<ILayout> = ({ children }) => {
  return <BaseLayout>{children}</BaseLayout>;
};

export default HistoryLayout;
