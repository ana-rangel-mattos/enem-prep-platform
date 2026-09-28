import React from "react";

const ProfileLayout: React.FC<ILayout> = ({ children }) => {
  return <div className="w-full max-w-full overflow-y-auto">{children}</div>;
};

export default ProfileLayout;
