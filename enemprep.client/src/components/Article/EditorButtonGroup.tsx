import React from "react";
import { cn } from "@/lib/utils";

interface IEditorButtonGroup extends React.ComponentPropsWithoutRef<"div"> {
  children?: React.ReactNode;
}

const EditorButtonGroup: React.FC<IEditorButtonGroup> = ({
  children,
  className,
  ...props
}) => {
  return (
    <div className={cn("bg-background/70 rounded-md", className)} {...props}>
      {children}
    </div>
  );
};

export default EditorButtonGroup;
