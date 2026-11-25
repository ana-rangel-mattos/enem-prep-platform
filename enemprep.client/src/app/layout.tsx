import type { Metadata } from "next";
import { Inter, Merriweather, Fira_Code } from "next/font/google";
import "./globals.css";
import React from "react";
import { SidebarInset, SidebarProvider } from "@/components/ui/sidebar";
import { cn } from "@/lib/utils";
import { Toaster } from "@/components/ui/sonner";
import StoreProvider from "@/app/StoreProvider";
import { SiteHeader } from "@/components/Navigation/site-header";
import { AppSidebar } from "@/components/Navigation/app-sidebar";
import { ThemeProvider } from "next-themes";

const fontSans = Inter({ subsets: ["latin"], variable: "--font-sans" });

const fontSerif = Merriweather({
  subsets: ["latin"],
  variable: "--font-serif",
});

const fontMono = Fira_Code({ subsets: ["latin"], variable: "--font-mono" });

export const metadata: Metadata = {
  title: "EnemPrep",
  description: "Prepare-se para o ENEM.",
};

interface IRootLayoutProps {
  children: React.ReactNode;
}

const RootLayout: React.FC<Readonly<IRootLayoutProps>> = ({ children }) => {
  return (
    <html
      lang="pt-BR"
      className={cn(fontSans.variable, fontSerif.variable, fontMono.variable)}
      suppressHydrationWarning
    >
      <body
        className="bg-background text-foreground selection:bg-primary/20 antialiased"
        suppressHydrationWarning
      >
        <StoreProvider>
          <ThemeProvider attribute="class">
            <SidebarProvider
              style={
                {
                  "--sidebar-width": "calc(var(--spacing) * 72)",
                  "--header-height": "calc(var(--spacing) * 12)",
                } as React.CSSProperties
              }
            >
              <AppSidebar />
              <SidebarInset>
                <SiteHeader />
                <main className="flex h-full flex-col items-center justify-start overflow-y-auto p-6 focus:outline-none">
                  {children}
                </main>
                <Toaster />
              </SidebarInset>
            </SidebarProvider>
          </ThemeProvider>
        </StoreProvider>
      </body>
    </html>
  );
};

export default RootLayout;
