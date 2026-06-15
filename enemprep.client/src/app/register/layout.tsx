import React from "react";

interface IRegisterLayout {
  children: React.ReactNode;
}

const RegisterLayout: React.FC<IRegisterLayout> = ({ children }) => {
  return <div className="w-full max-w-sm">{children}</div>;
};

export default RegisterLayout;
