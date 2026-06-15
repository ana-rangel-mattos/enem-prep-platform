"use client";

import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar";
import {
  BookOpen,
  GraduationCap,
  LayoutDashboard,
  LucideProps,
  ShieldAlert,
  User,
} from "lucide-react";
import Link from "next/link";
import { useAppSelector } from "@/hooks/use-store";
import { selectUser } from "@/lib/features/auth/authSlice";
import { ForwardRefExoticComponent, useMemo } from "react";

interface INavigationProps {
  title: string;
  url: string;
  icon: ForwardRefExoticComponent<Omit<LucideProps, "ref">>;
  requiredRole?: string;
}

const naviagationItems: INavigationProps[] = [
  {
    title: "Dashboard",
    url: "/dashboard",
    icon: LayoutDashboard,
    requiredRole: "Student",
  },
  {
    title: "Questões",
    url: "/questions",
    icon: BookOpen,
    requiredRole: "Student",
  },
  {
    title: "Simulados",
    url: "/exams",
    icon: GraduationCap,
    requiredRole: "Student",
  },
  { title: "Perfil", url: "/profile", icon: User, requiredRole: "Student" },
  {
    title: "Painel Admin",
    url: "/admin",
    icon: ShieldAlert,
    requiredRole: "Admin",
  },
];

export function AppSidebar() {
  const user = useAppSelector((state) => selectUser(state));
  const year = useMemo(() => new Date().getFullYear(), []);

  const visibleItems: INavigationProps[] = naviagationItems.filter(
    (item) =>
      item.requiredRole === null ||
      user?.roles.some((userRole) => userRole === item.requiredRole),
  );

  return (
    <Sidebar>
      <SidebarHeader className="min-h-10 items-center justify-center">
        EnemPrep
      </SidebarHeader>
      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupLabel>Área do Estudante</SidebarGroupLabel>
          <SidebarGroupContent>
            <SidebarMenu>
              {visibleItems.map((item) => (
                <SidebarMenuItem key={item.title} className="not-first:mt-2">
                  <SidebarMenuButton asChild>
                    <Link href={item.url}>
                      <item.icon />
                      <span>{item.title}</span>
                    </Link>
                  </SidebarMenuButton>
                </SidebarMenuItem>
              ))}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>
      <SidebarFooter className="text-muted-foreground border-sidebar-border border-t p-4 text-xs">
        Graduação Engenharia de Software &copy; {year}
      </SidebarFooter>
    </Sidebar>
  );
}
