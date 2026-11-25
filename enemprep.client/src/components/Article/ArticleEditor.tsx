"use client";

import React from "react";
import { EditorContent, useEditor } from "@tiptap/react";
import { StarterKit } from "@tiptap/starter-kit";
import { Placeholder } from "@tiptap/extensions";
import { PlaylistExtension } from "@/components/Article/extensions/PlaylistExtension";
import { Button } from "@/components/ui/button";
import {
  Bold,
  List,
  ListOrdered,
  Heading1,
  Heading2,
  Heading3,
  Heading4,
  Heading5,
  Heading6,
  Italic,
  ListMusic,
  Subscript,
  Superscript,
  Strikethrough,
  ListTodo,
  Redo,
  Undo,
  Quote,
} from "lucide-react";
import { IArticleBlock } from "@/lib/types/article.types";
import { TaskItem, TaskList } from "@tiptap/extension-list";
import { TextStyleKit } from "@tiptap/extension-text-style";
import EditorButtonGroup from "@/components/Article/EditorButtonGroup";

interface IArticleEditorProps {
  onSave: (blocks: IArticleBlock[]) => void;
}

const ArticleEditor: React.FC<IArticleEditorProps> = ({ onSave }) => {
  const editor = useEditor({
    immediatelyRender: false,
    extensions: [
      TextStyleKit,
      StarterKit,
      TaskItem.configure({ nested: true }),
      TaskList,
      Placeholder.configure({
        placeholder: "Começe a escrever o seu artigo aqui...",
      }),
      PlaylistExtension,
    ],
    editorProps: {
      attributes: {
        class:
          "prose dark:prose-invert max-w-none min-h-[300px] focus:outline-none p-4 border rounded-md",
      },
    },
  });

  if (!editor) return null;

  const handleSave = () => {
    const rawJson = editor.getJSON();

    const articleBlocks = (rawJson.content || []).map((node, index) => {
      const blockId = `block-${index + 1}`;

      if (node.type === "heading") {
        return {
          id: blockId,
          type: "heading",
          text: node.content?.[0]?.text || "",
          level: node.attrs?.level || 1,
        };
      }

      if (node.type === "playlist") {
        return {
          id: blockId,
          type: "playlist",
          playlistId: node.attrs?.playlistId,
        };
      }

      return {
        id: blockId,
        type: "text",
        content: node.content?.[0]?.text || "",
      };
    });

    onSave(articleBlocks);
  };

  return (
    <div className="min-w-full space-y-4">
      <div className="bg-muted/50 flex w-full items-center justify-end gap-1.5 divide-x-2 rounded-md border p-1">
        {/* Headings */}
        <EditorButtonGroup>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() =>
              editor.chain().focus().toggleHeading({ level: 1 }).run()
            }
          >
            <Heading1 className="size-4" />
          </Button>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() =>
              editor.chain().focus().toggleHeading({ level: 2 }).run()
            }
          >
            <Heading2 className="size-4" />
          </Button>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() =>
              editor.chain().focus().toggleHeading({ level: 3 }).run()
            }
          >
            <Heading3 className="size-4" />
          </Button>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() =>
              editor.chain().focus().toggleHeading({ level: 4 }).run()
            }
          >
            <Heading4 className="size-4" />
          </Button>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() =>
              editor.chain().focus().toggleHeading({ level: 5 }).run()
            }
          >
            <Heading5 className="size-4" />
          </Button>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() =>
              editor.chain().focus().toggleHeading({ level: 6 }).run()
            }
          >
            <Heading6 className="size-4" />
          </Button>
        </EditorButtonGroup>
        {/* Text styles */}
        <EditorButtonGroup>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() => editor.chain().focus().toggleBold().run()}
          >
            <Bold className="size-4" />
          </Button>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() => editor.chain().focus().toggleItalic().run()}
          >
            <Italic className="size-4" />
          </Button>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() => editor.chain().focus().toggleStrike().run()}
          >
            <Strikethrough className="size-4" />
          </Button>
        </EditorButtonGroup>
        {/* Math */}
        <EditorButtonGroup>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() => editor.chain().focus().toggleSubscript().run()}
          >
            <Subscript className="size-4" />
          </Button>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() => editor.chain().focus().toggleSuperscript().run()}
          >
            <Superscript className="size-4" />
          </Button>
        </EditorButtonGroup>
        {/* Lists */}
        <EditorButtonGroup>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() => editor.chain().focus().toggleBulletList().run()}
          >
            <List className="size-4" />
          </Button>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() => editor.chain().focus().toggleOrderedList().run()}
          >
            <ListOrdered className="size-4" />
          </Button>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() => editor.chain().focus().toggleTaskList().run()}
          >
            <ListTodo className="size-4" />
          </Button>
        </EditorButtonGroup>
        {/* Outros */}
        <EditorButtonGroup>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() => editor.chain().focus().toggleBlockquote().run()}
          >
            <Quote className="size-4" />
          </Button>
        </EditorButtonGroup>
        {/* Version Control */}
        <EditorButtonGroup>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() => editor.chain().focus().undo().run()}
          >
            <Undo className="size-4" />
          </Button>
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={() => editor.chain().focus().redo().run()}
          >
            <Redo className="size-4" />
          </Button>
        </EditorButtonGroup>
        <Button
          type="button"
          variant="outline"
          size="sm"
          onClick={() =>
            editor.chain().focus().insertContent({ type: "playlist" }).run()
          }
        >
          <ListMusic className="mr-1.5 size-4" />
          Inserir Playlist
        </Button>
      </div>

      <EditorContent editor={editor} />

      <Button onClick={handleSave}>Salvar Artigo</Button>
    </div>
  );
};

export default ArticleEditor;
