"use server";

import { cookies } from "next/headers";

export const logoutDeleteCookies = async () => {
  const cookieStore = await cookies();

  cookieStore.delete(".AspNetCore.Session");
};
