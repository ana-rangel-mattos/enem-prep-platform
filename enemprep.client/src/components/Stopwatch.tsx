import React, { useEffect, useState } from "react";

interface IStopwatchProps {
  hasStarted?: boolean;
}

const Stopwatch: React.FC<IStopwatchProps> = ({ hasStarted = true }) => {
  const [time, setTime] = useState<number>(0);

  useEffect(() => {
    let intervalId: NodeJS.Timeout;
    if (hasStarted) {
      intervalId = setInterval(() => setTime(time + 1), 1000);
    }

    return () => clearInterval(intervalId);
  }, [time, hasStarted]);

  const hours = Math.floor(time / 3600);
  const minutes = Math.floor((time % 3600) / 60);
  const seconds = Math.floor((time % 3600) % 60);
  
  return (
    <div className="bg-primary flex justify-center text-secondary absolute right-0 rounded-3xl w-35 py-2">
      <p className="text-2xl font-bold text-shadow-background/70 text-shadow-md">
        {hours}:{minutes.toString().padStart(2, "0")}:
        {seconds.toString().padStart(2, "0")}
      </p>
    </div>
  );
}

export default React.memo(Stopwatch);