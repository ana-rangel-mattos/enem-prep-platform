import React from "react";
import BaseLayout from "@/components/Layouts/BaseLayout";

const DashboardLayout: React.FC<ILayout> = ({ children }) => {
  return <BaseLayout>{children}</BaseLayout>;
};

export default DashboardLayout;
