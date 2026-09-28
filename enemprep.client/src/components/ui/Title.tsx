import React from "react";
import { cn } from "@/lib/utils";

interface ITitleProps extends React.ComponentPropsWithoutRef<"h1"> {
  text: string;
}

const Title: React.FC<ITitleProps> = ({ text, className, ...props }) => {
  return (
    <h1 className={cn("text-center text-xl md:text-2xl", className)} {...props}>
      {text}
    </h1>
  );
};

export default Title;
