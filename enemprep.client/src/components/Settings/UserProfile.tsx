import { useEffect, useState } from "react";
import { IUserProfileResponse } from "@/lib/types/user-profile.types";
import { AxiosError } from "axios";
import { userProfileService } from "@/lib/api/userProfileService";
import { toast } from "sonner";
import Loading from "@/components/Loading";
import Error from "next/error";
import Subtitle from "@/components/ui/Subtitle";
import UpdateUserProfile from "@/components/Settings/UpdateUserProfile";

const UserProfile = () => {
  const [profile, setProfile] = useState<IUserProfileResponse | null>(null);
  const [isLoadingProfile, setIsLoadingProfile] = useState<boolean>(true);
  const [userProfileError, setUserProfileError] =
    useState<AxiosError<ProblemDetails> | null>(null);
  const [isEditingProfile, setIsEditingProfile] = useState<boolean>(false);

  useEffect(() => {
    const getUserProfile = async () => {
      try {
        setUserProfileError(null);
        setIsLoadingProfile(true);

        const userProfile = await userProfileService.getProfile();
        setProfile(userProfile);
      } catch (error) {
        const axiosError = error as AxiosError<ProblemDetails>;
        const problem = axiosError.response?.data;
        toast.error(
          problem?.description || "Erro ao carregar perfil do usuário.",
        );
        setUserProfileError(axiosError);
      } finally {
        setIsLoadingProfile(false);
      }
    };

    getUserProfile();
  }, []);

  if (isLoadingProfile)
    return <Loading message="Carregando perfil do usuário..." />;

  if (userProfileError?.response?.data || !profile)
    return (
      <Error
        statusCode={userProfileError?.response?.data.code || 500}
        title={userProfileError?.response?.data.description}
      />
    );

  return (
    <>
      <Subtitle text="Perfil do Usuário" className="my-4" />
      <UpdateUserProfile
        userProfile={profile}
        isEditing={isEditingProfile}
        setIsEditing={setIsEditingProfile}
        onSaveUserBio={() => {}}
      />
    </>
  );
};

export default UserProfile;
