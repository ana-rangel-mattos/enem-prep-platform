import React from "react";
import { CardDescription, CardHeader, CardTitle } from "@/components/ui/card";

interface ICardHeaderProps extends React.ComponentProps<"div"> {
  title: string;
  description: string;
  children?: React.ReactNode;
}

const FormCardHeader: React.FC<ICardHeaderProps> = ({
  title,
  description,
  children,
  ...props
}) => {
  return (
    <CardHeader {...props}>
      <CardTitle className="text-lg">{title}</CardTitle>
      <CardDescription className="text-base">{description}</CardDescription>
      {children}
    </CardHeader>
  );
};

export default FormCardHeader;
