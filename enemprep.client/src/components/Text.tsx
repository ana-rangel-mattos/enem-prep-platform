import React from "react";
import { cn } from "@/lib/utils";

interface ITextProps extends React.ComponentPropsWithoutRef<"p"> {
  children: React.ReactNode;
  size?: "xs" | "sm" | "base" | "md" | "lg";
  variant?: "foreground" | "primary" | "secondary";
  bold?: boolean;
  italic?: boolean;
}

const Text: React.FC<ITextProps> = ({
  children,
  size = "md",
  variant = "foreground",
  bold = false,
  italic = false,
  className,
  ...props
}) => {
  return (
    <p
      className={cn(
        `text-${variant} text-${size} ${bold ? "font-bold" : ""} ${italic ? "italic" : ""}`,
        className,
      )}
      {...props}
    >
      {children}
    </p>
  );
};

export default Text;
