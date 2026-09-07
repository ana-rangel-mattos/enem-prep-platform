import React, { useEffect } from "react";
import { Badge } from "@/components/ui/badge";
import { useCountdown } from "@/hooks/use-countdown";

interface ITimeCountdownProps extends React.ComponentPropsWithoutRef<"div"> {
  countStartSeconds: number;
  hasStarted: boolean;
  onExpire?: () => void;
}
const TimerCountdown: React.FC<ITimeCountdownProps> = ({
  countStartSeconds,
  hasStarted,
  onExpire,
  ...props
}) => {
  const [count, { startCountdown }] = useCountdown({
    countStart: countStartSeconds,
  });

  const minutes = Math.floor(count / 60);
  const seconds = Math.floor(count % 60);

  useEffect(() => {
    if (hasStarted) {
      startCountdown();
    }
  }, [hasStarted, startCountdown]);

  useEffect(() => {
    if (count === 0 && hasStarted) {
      onExpire?.();
    }
  }, [count, hasStarted, onExpire]);

  return (
    <div {...props}>
      <div className="space-y-3 text-center">
        <div className="flex min-h-[60px] items-center justify-center">
          <Badge
            className="px-4 py-5 font-mono text-3xl font-bold"
            variant={
              count === 0
                ? "destructive"
                : count <= countStartSeconds * 0.1
                  ? "secondary"
                  : "default"
            }
          >
            {minutes.toString().padStart(2, "0")}:
            {seconds.toString().padStart(2, "0")}
          </Badge>
        </div>
      </div>
    </div>
  );
};

export default TimerCountdown;
