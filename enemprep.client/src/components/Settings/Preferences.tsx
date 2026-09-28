import { useEffect, useState } from "react";
import { IUserPreferencesResponse } from "@/lib/types/user-preferences.types";
import { AxiosError } from "axios";
import { useRouter } from "next/navigation";
import { userPreferencesService } from "@/lib/api/userPreferencesService";
import { toast } from "sonner";
import Loading from "@/components/Loading";
import Error from "next/error";
import Subtitle from "@/components/ui/Subtitle";
import UpdateResetPreferences from "@/components/Settings/UpdateResetPreferences";

const Preferences = () => {
  const [preferences, setPreferences] =
    useState<IUserPreferencesResponse | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<AxiosError<ProblemDetails> | null>(null);
  const [isEditingPreferences, setIsEditingPreferences] =
    useState<boolean>(false);

  const router = useRouter();

  useEffect(() => {
    const getUserPreferences = async () => {
      try {
        setError(null);
        setIsLoading(true);
        const userPreferences =
          await userPreferencesService.getLoggedUserPreferences();
        setPreferences(userPreferences);
      } catch (error) {
        const axiosError = error as AxiosError<ProblemDetails>;
        const problem = axiosError.response?.data;
        toast.error(
          problem?.description || "Erro ao carregar preferências do usuário.",
        );
        setError(axiosError);
      } finally {
        setIsLoading(false);
      }
    };

    getUserPreferences();
  }, []);

  const handleUpdateUserPreferences = () => {
    setIsEditingPreferences(false);
    router.refresh();
  };

  if (isLoading)
    return <Loading message="Carregando preferências do usuário..." />;

  if (error?.response?.data || !preferences)
    return (
      <Error
        statusCode={error?.response?.data.code || 500}
        title={error?.response?.data.description}
      />
    );

  return (
    <>
      <Subtitle text="Preferências do Usuário" className="my-4" />
      <UpdateResetPreferences
        userPreferences={preferences}
        isEditing={isEditingPreferences}
        setIsEditing={setIsEditingPreferences}
        onSavePreferences={handleUpdateUserPreferences}
      />
    </>
  );
};

export default Preferences;
