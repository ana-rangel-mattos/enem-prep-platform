import React from "react";
import { cn } from "@/lib/utils";

interface IBaseLayoutProps extends React.ComponentPropsWithoutRef<"div"> {
  children: React.ReactNode;
}

const BaseLayout: React.FC<IBaseLayoutProps> = ({
  children,
  className,
  ...props
}) => {
  return (
    <div
      className={cn("w-full max-w-full overflow-y-auto", className)}
      {...props}
    >
      {children}
    </div>
  );
};

export default BaseLayout;
