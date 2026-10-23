"use client";

import Title from "@/components/ui/Title";
import ScheduleSelection from "@/components/Schedule/ScheduleSelection";

const Dashboard = () => {
  return (
    <>
      <Title text="Dashboard" className="my-4" />
      <ScheduleSelection />
    </>
  );
};

export default Dashboard;
