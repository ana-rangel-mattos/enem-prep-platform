import Subtitle from "@/components/ui/Subtitle";
import { useEffect, useState } from "react";
import { AxiosError } from "axios";
import { toast } from "sonner";
import Loading from "@/components/Loading";
import Error from "next/error";
import UpdateUserAccount from "@/components/Settings/UpdateUserAccount";
import { IUserAccountResponse } from "@/lib/types/user.types";
import { userService } from "@/lib/api/userService";

const UserAccount = () => {
  const [userAccount, setUserAccount] = useState<IUserAccountResponse | null>(
    null,
  );
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<AxiosError<ProblemDetails> | null>(null);
  const [isEditing, setIsEditing] = useState<boolean>(false);

  useEffect(() => {
    const getUserAccount = async () => {
      try {
        setError(null);
        setIsLoading(true);

        const userAcc = await userService.getLoggedUserAccount();
        setUserAccount(userAcc);
      } catch (error) {
        const axiosError = error as AxiosError<ProblemDetails>;
        const problem = axiosError.response?.data;
        toast.error(
          problem?.description || "Erro ao carregar conta do usuário.",
        );
        setError(axiosError);
      } finally {
        setIsLoading(false);
      }
    };

    getUserAccount();
  }, []);

  const handleSaveUserAccount = () => {
    setIsEditing(false);
  };

  if (isLoading) return <Loading message="Carregando conta do usuário..." />;

  if (error?.response?.data || !userAccount)
    return (
      <Error
        statusCode={error?.response?.data.code || 500}
        title={error?.response?.data.description}
      />
    );

  return (
    <>
      <Subtitle text="Conta do Usuário" className="my-4" />
      <UpdateUserAccount
        userAccount={userAccount}
        isEditing={isEditing}
        setIsEditing={setIsEditing}
        onSaveUserAccount={handleSaveUserAccount}
      />
    </>
  );
};

export default UserAccount;
