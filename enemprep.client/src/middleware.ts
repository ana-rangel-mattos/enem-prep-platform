import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

const middleware = (request: NextRequest) => {
    const sessionToken = request.cookies.get("session_token")?.value;
    const { pathname } = request.nextUrl;

    if (sessionToken) {
        if (pathname == "/" || pathname == "/login" || pathname == "/register") {
            return NextResponse.redirect(new URL("/dashboard", request.url));
        }
    } else {
        const isProtectedRoute = ["/dashboard", "/exams", "/questions", "/profile", "/admin"].some(
            (route) => pathname.startsWith(route)
        )

        if (isProtectedRoute) {
            return NextResponse.redirect(new URL("/", request.url));
        }
    }

    return NextResponse.next();
}

export const config = {
    matcher: ["/((?!_next/static|_next/image|favicon.ico|assets).*)"],
}

export default middleware;