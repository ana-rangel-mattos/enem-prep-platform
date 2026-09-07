import { useCallback, useEffect, useRef, useState } from "react";

interface CountdownOptions {
  countStart: number;
  intervalMs?: number;
  isIncrement?: boolean;
  countStop?: number;
}

interface CountdownControllers {
  startCountdown: () => void;
  stopCountdown: () => void;
}

export type UseCountdownReturn = [number, CountdownControllers];

export function useCountdown({
  countStart,
  countStop = 0,
  intervalMs = 1000,
  isIncrement = false,
}: CountdownOptions): UseCountdownReturn {
  const [count, setCount] = useState(countStart);
  const [isRunning, setIsRunning] = useState(false);
  const savedCallback = useRef<() => void>(undefined);

  const increment = useCallback(() => setCount((x) => x + 1), []);
  const decrement = useCallback(() => setCount((x) => x - 1), []);

  const startCountdown = useCallback(() => setIsRunning(true), []);
  const stopCountdown = useCallback(() => setIsRunning(false), []);

  const countdownCallback = useCallback(() => {
    if (count === countStop) {
      setIsRunning(false);
      return;
    }
    if (isIncrement) {
      increment();
    } else {
      decrement();
    }
  }, [count, countStop, decrement, increment, isIncrement]);

  // Store callback in ref
  useEffect(() => {
    savedCallback.current = countdownCallback;
  }, [countdownCallback]);

  // Set up interval
  useEffect(() => {
    if (!isRunning) {
      return;
    }

    const tick = () => savedCallback.current?.();
    const id = setInterval(tick, intervalMs);
    return () => clearInterval(id);
  }, [isRunning, intervalMs]);

  return [count, { startCountdown, stopCountdown }];
}

export type { CountdownOptions, CountdownControllers };
