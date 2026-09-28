import BaseLayout from "@/components/Layouts/BaseLayout";
import React from "react";

const SettingsLayout: React.FC<ILayout> = ({ children }) => {
  return <BaseLayout>{children}</BaseLayout>;
};

export default SettingsLayout;
