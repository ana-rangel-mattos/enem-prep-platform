import { Spinner } from "@/components/ui/spinner";
import { Item, ItemContent, ItemMedia, ItemTitle } from "@/components/ui/item";
import React from "react";

interface ILoadingProps {
  message: string;
}

const Loading: React.FC<ILoadingProps> = ({ message }) => {
  return (
    <div className="flex w-full max-w-xs flex-col gap-4 [--radius:1rem]">
      <Item variant="muted">
        <ItemMedia>
          <Spinner />
        </ItemMedia>
        <ItemContent>
          <ItemTitle className="line-clamp-1">{message}</ItemTitle>
        </ItemContent>
      </Item>
    </div>
  );
};

export default Loading;
