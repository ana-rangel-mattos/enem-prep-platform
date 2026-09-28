import { IUserProfileResponse } from "@/lib/types/user-profile.types";
import React from "react";
import { z } from "zod";
import { Controller, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { toast } from "sonner";
import { AxiosError } from "axios";
import { userProfileService } from "@/lib/api/userProfileService";
import { Card, CardContent, CardFooter } from "@/components/ui/card";
import FormCardHeader from "@/components/FormCardHeader";
import {
  Field,
  FieldDescription,
  FieldError,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field";
import { Button } from "@/components/ui/button";
import { Textarea } from "@/components/ui/textarea";
import EditButton from "@/components/EditButton";

interface IUpdateUserBioProps {
  userProfile: IUserProfileResponse;
  isEditing: boolean;
  setIsEditing: (isEditing: boolean) => void;
  onSaveUserBio: () => void;
}

const formSchema = z.object({
  userBio: z
    .string()
    .min(10, "A bio do usuário deve ter ao menos 10 caracteres.")
    .max(400, "A bio do usuário não deve passar de 400 caracteres.")
    .optional(),
});

type FormData = z.infer<typeof formSchema>;

const UpdateUserProfile: React.FC<IUpdateUserBioProps> = ({
  onSaveUserBio,
  userProfile,
  setIsEditing,
  isEditing,
}) => {
  const { control, handleSubmit } = useForm<FormData>({
    resolver: zodResolver(formSchema),
    defaultValues: { userBio: userProfile?.userBio },
  });

  const onSubmit = async (data: FormData) => {
    try {
      await userProfileService.updateUserBio(data);
      onSaveUserBio();
      toast.success("Perfil atualizado com sucesso!");
    } catch (error) {
      const axiosError = error as AxiosError<ProblemDetails>;
      const problem = axiosError.response?.data;
      toast.error(
        problem?.description || "Erro ao atualizar o Perfil do usuário.",
      );
    }
  };

  return (
    <Card className="mx-auto w-full sm:max-w-md md:max-w-lg lg:max-w-4xl">
      <FormCardHeader
        title="Perfil do Usuário"
        description="Gerencie informações sobre o perfil do usuário aqui. Ex: Bio."
      >
        <EditButton isEditing={isEditing} setIsEditing={setIsEditing} />
      </FormCardHeader>
      <CardContent>
        <form onSubmit={handleSubmit(onSubmit)} id="form-update-user-profile">
          <FieldGroup>
            <Controller
              name="userBio"
              control={control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor="user-bio">Bio do Usuário</FieldLabel>
                  <FieldDescription>
                    Escreva um breve texto sobre você.
                  </FieldDescription>
                  <Textarea
                    {...field}
                    id="user-bio"
                    placeholder="Digite a sua bio aqui."
                    disabled={!isEditing}
                  ></Textarea>
                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
          </FieldGroup>
        </form>
      </CardContent>
      {isEditing && (
        <CardFooter>
          <Field orientation="horizontal">
            <Button
              type="button"
              variant="outline"
              onClick={() => setIsEditing(false)}
            >
              Cancelar
            </Button>
            <Button type="submit" form="form-update-user-profile">
              Atualizar
            </Button>
          </Field>
        </CardFooter>
      )}
    </Card>
  );
};

export default UpdateUserProfile;
