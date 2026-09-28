import React from "react";

const LoginLayout: React.FC<ILayout> = ({ children }) => {
  return <div className="w-full max-w-sm">{children}</div>;
};

export default LoginLayout;
