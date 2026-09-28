import { Button } from "@/components/ui/button";
import React from "react";

interface IEditButtonProps {
  isEditing: boolean;
  setIsEditing: (isEditing: boolean) => void;
}

const EditButton: React.FC<IEditButtonProps> = ({
  isEditing,
  setIsEditing,
}) => {
  return (
    <>
      {!isEditing && (
        <div className="mt-3 flex">
          <Button
            onClick={() => setIsEditing(true)}
            className="bg-background text-primary hover:text-background min-h-7 min-w-20 rounded-md hover:cursor-pointer"
          >
            Editar
          </Button>
        </div>
      )}
    </>
  );
};

export default EditButton;
