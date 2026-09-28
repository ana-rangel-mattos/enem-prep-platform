"use client";

import * as React from "react";

import NavMain from "@/components/nav-main";
import { NavSecondary } from "@/components/nav-secondary";
import NavUser from "@/components/nav-user";
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar";
import {
  LayoutDashboard,
  Settings2,
  CommandIcon,
  LucideProps,
  BookOpen,
  GraduationCap,
  User,
  ShieldAlert,
  Landmark,
} from "lucide-react";
import { useAppSelector } from "@/hooks/use-store";
import {
  selectIsAuthenticated,
  selectUser,
} from "@/lib/features/auth/authSlice";
import { ForwardRefExoticComponent, useEffect } from "react";
import { TooltipProvider } from "@/components/ui/tooltip";
import Link from "next/link";

export interface INavigationProps {
  title: string;
  url: string;
  icon: ForwardRefExoticComponent<Omit<LucideProps, "ref">>;
  requiredRole?: string;
}

const data = {
  navMain: [
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
    {
      title: "Questões Resolvidas",
      url: "/history",
      icon: Landmark,
      requiredRole: "Student",
    },
  ],
  navSecondary: [
    {
      title: "Configurações",
      url: "/settings",
      icon: Settings2,
      requiredRole: "Student",
    },
  ],
};

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  const user = useAppSelector((state) => selectUser(state))!;
  const isAuthenticated: boolean = useAppSelector((state) =>
    selectIsAuthenticated(state),
  );

  const visibleItems: INavigationProps[] = data.navMain.filter(
    (item) =>
      item.requiredRole === null ||
      user?.roles.some((userRole) => userRole === item.requiredRole),
  );

  const secondaryVisibleItems: INavigationProps[] = data.navSecondary.filter(
    (item) =>
      item.requiredRole === null ||
      user?.roles.some((userRole) => userRole === item.requiredRole),
  );

  return (
    <TooltipProvider delayDuration={0}>
      <Sidebar collapsible="offcanvas" {...props}>
        <SidebarHeader>
          <SidebarMenu>
            <SidebarMenuItem>
              <SidebarMenuButton
                asChild
                className="data-[slot=sidebar-menu-button]:p-1.5!"
              >
                <Link href="/dashboard">
                  <CommandIcon className="size-5!" />
                  <span className="text-base font-semibold">EnemPrep</span>
                </Link>
              </SidebarMenuButton>
            </SidebarMenuItem>
          </SidebarMenu>
        </SidebarHeader>
        <SidebarContent>
          <NavMain items={visibleItems} />
          <NavSecondary items={secondaryVisibleItems} className="mt-auto" />
        </SidebarContent>
        <SidebarFooter>
          {isAuthenticated && <NavUser user={user} />}
        </SidebarFooter>
      </Sidebar>
    </TooltipProvider>
  );
}
