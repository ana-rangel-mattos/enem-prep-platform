import React from "react";
import { NodeViewProps, NodeViewWrapper } from "@tiptap/react";
import { Button } from "@/components/ui/button";
import { ListMusic, Trash2 } from "lucide-react";

const PlaylistNodeView: React.FC<NodeViewProps> = ({
  node,
  updateAttributes,
  deleteNode,
}) => {
  const playlistId = node.attrs.playlistId as string | null;

  return (
    <NodeViewWrapper className="my-4">
      <div className="border-primary/20 bg-muted/40 borded-dashed relative rounded-lg border-2 p-4 transition-all">
        {playlistId ? (
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-3">
              <div className="bg-primary/10 text-primary rounded-md p-2">
                <ListMusic className="size-5" />
              </div>
              <div>
                <p className="text-sm font-semibold">
                  Lista de Exercícios Anexada
                </p>
                <p className="text-muted-foreground font-mono text-xs">
                  {playlistId}
                </p>
              </div>
            </div>
            <Button
              variant="ghost"
              size="icon-sm"
              onClick={deleteNode}
              className="text-destructive hover:text-destructive/80"
            >
              <Trash2 className="size-4" />
            </Button>
          </div>
        ) : (
          <div className="flex flex-col items-center justify-center space-y-2 text-center">
            <ListMusic className="text-muted-foreground size-8" />
            <p className="text-sm font-medium">
              Selecione uma Lista de Exercícios
            </p>
            <Button
              size="sm"
              variant="outline"
              onClick={() => {
                updateAttributes({ playlistId: "example-uuid-here" });
              }}
            >
              Anexar Playlist
            </Button>
          </div>
        )}
      </div>
    </NodeViewWrapper>
  );
};

export default PlaylistNodeView;
