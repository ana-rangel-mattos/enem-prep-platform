import React from "react";

interface ILoginLayoutProps {
    children: React.ReactNode;
}

const LoginLayout: React.FC<ILoginLayoutProps> = ({ children }) => {
    return (
        <div className="w-full max-w-sm">{children}</div>
    );
}

export default LoginLayout;