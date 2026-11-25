import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  allowedDevOrigins: ["192.168.1.241", "192.168.1.167"],
  images: {
    remotePatterns: [
      { protocol: "https", hostname: "ui-avatars.com" },
      { protocol: "https", hostname: "enem.dev" },
      { protocol: "https", hostname: "api.dicebear.com" },
    ],
  },
  output: "standalone",
  typescript: {
    ignoreBuildErrors: true,
  },
};

export default nextConfig;
