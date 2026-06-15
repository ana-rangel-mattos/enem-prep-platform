import React from "react";

interface IDashboardLayoutProps {
    children: React.ReactNode;
}

const DashboardLayout: React.FC<IDashboardLayoutProps> = ({ children }) => {
    return (
        <div className="w-full max-w-sm">{children}</div>
    );
}

export default DashboardLayout;