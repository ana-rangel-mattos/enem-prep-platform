import {
    Sidebar,
    SidebarContent,
    SidebarFooter,
    SidebarGroup, SidebarGroupContent, SidebarGroupLabel,
    SidebarHeader, SidebarMenu, SidebarMenuButton, SidebarMenuItem,
} from "@/components/ui/sidebar";
import {BookOpen, GraduationCap, LayoutDashboard, User} from "lucide-react";
import Link from "next/link";

const naviagationItems = [
    { title: "Dashboard", url: "/dashboard", icon: LayoutDashboard },
    { title: "Questões", url: "/questions", icon: BookOpen },
    { title: "Simulados", url: "/exams", icon: GraduationCap },
    { title: "Perfil", url: "/profile", icon: User },
]

export function AppSidebar() {
    return (
        <Sidebar>
            <SidebarHeader className="min-h-10 justify-center items-center">
                EnemPrep
            </SidebarHeader>
            <SidebarContent>
                <SidebarGroup>
                    <SidebarGroupLabel>Área do Estudante</SidebarGroupLabel>
                    <SidebarGroupContent>
                        <SidebarMenu>
                            {naviagationItems.map((item) => (
                                <SidebarMenuItem key={item.title}>
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
            <SidebarFooter className="p-4 text-xs text-muted-foreground border-t border-sidebar-border">
                Graduação Engenharia de Software &copy; {new Date().getFullYear()}
            </SidebarFooter>
        </Sidebar>
    )
}