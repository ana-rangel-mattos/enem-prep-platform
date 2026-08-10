import React from "react";

interface IProfileLayoutProps {
  children: React.ReactNode;
}
const ProfileLayout: React.FC<IProfileLayoutProps> = ({ children }) => {
  return <div className="w-full max-w-full overflow-y-auto">{children}</div>;
}

export default ProfileLayout;