import React from "react";
import { cn } from "@/lib/utils";

interface ISubtitleProps extends React.ComponentPropsWithoutRef<"p"> {
  text: string;
}

const Subtitle: React.FC<ISubtitleProps> = ({ text, className, ...props }) => {
  return (
    <p className={cn("lg:text-3xl font-bold text-xl text-center text-foreground", className)} {...props}>{text}</p>
  );
}

export default Subtitle;