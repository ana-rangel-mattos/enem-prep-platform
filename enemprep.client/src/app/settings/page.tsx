"use client";

import Title from "@/components/ui/Title";
import Preferences from "@/components/Settings/Preferences";
import UserProfile from "@/components/Settings/UserProfile";
import UserAccount from "@/components/Settings/UserAccount";

const Settings = () => {
  return (
    <>
      <Title text="Configurações" />
      <Preferences />
      <UserProfile />
      <UserAccount />
    </>
  );
};

export default Settings;
