import React from "react";

const DashboardLayout: React.FC<ILayout> = ({ children }) => {
  return <div className="w-full max-w-sm">{children}</div>;
};

export default DashboardLayout;
