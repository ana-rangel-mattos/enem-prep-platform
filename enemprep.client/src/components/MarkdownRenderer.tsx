"use client";

import ReactMarkdown from "react-markdown";
import React from "react";

interface IMarkdownRendererProps {
  content: string;
}

const MarkdownRenderer: React.FC<IMarkdownRendererProps> = ({ content }) => {
  return (
    <ReactMarkdown
      components={{
        p: ({ children }) => (
          <p className="leading-7 not-first:mt-6">{children}</p>
        ),
        img: ({ src, alt }) => (
          <span className="border-border bg-muted/30 block max-w-full overflow-hidden rounded-lg border p-2">
            {/* eslint-disable-next-line @next/next/no-img-element */}
            <img
              src={src}
              alt={alt || "Imagem da questão"}
              className="mx-auto max-h-87.5 w-auto max-w-full rounded object-contain"
              loading="lazy"
            />
          </span>
        ),
        strong: ({ children }) => (
          <strong className="text-primary font-semibold">{children}</strong>
        ),
        em: ({ children }) => (
          <em className="text-muted-foreground italic">{children}</em>
        ),
      }}
    >
      {content}
    </ReactMarkdown>
  );
};

export default MarkdownRenderer;
