export const fileToBase64 = (file: File): Promise<string> => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();

    reader.onload = () => {
      const result = reader.result as string;

      const base64 = result.split(",")[1];

      resolve(base64);
    };

    reader.onerror = reject;
    reader.readAsDataURL(file);
  });
};

export const timeToString = (time: number): string => {
  return time.toString().slice(0, 8);
};

export const ISODateToString = (date: string): string => {
  return date.slice(0, 10).split("-").reverse().join("/");
};

export const unknownEnumToNumber = (value: unknown): number | undefined => {
  if (
    value === "" ||
    value === null ||
    value === undefined ||
    value === "none" ||
    value === "-1"
  ) {
    return undefined;
  }

  const parsed = Number(value);
  return isNaN(parsed) ? undefined : parsed;
};
