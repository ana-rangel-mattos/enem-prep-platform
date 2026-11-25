import { Node } from "@tiptap/core";
import { mergeAttributes, ReactNodeViewRenderer } from "@tiptap/react";
import PlaylistNodeView from "@/components/Article/extensions/PlaylistNodeView";

export const PlaylistExtension = Node.create({
  name: "playlist",
  group: "block",
  atom: true,

  addAttributes() {
    return { playlistId: { default: null } };
  },

  parseHTML() {
    return [{ tag: 'div[data-type="playlist"]' }];
  },

  renderHTML({ HTMLAttributes }) {
    return [
      "div",
      mergeAttributes(HTMLAttributes, { "data-type": "playlist" }),
    ];
  },

  addNodeView() {
    return ReactNodeViewRenderer(PlaylistNodeView);
  },
});
