import { IUserAccountResponse } from "@/lib/types/user.types";
import React from "react";
import { z } from "zod";
import { Card, CardContent, CardFooter } from "@/components/ui/card";
import FormCardHeader from "@/components/FormCardHeader";
import { Button } from "@/components/ui/button";
import EditButton from "@/components/EditButton";
import {
  Field,
  FieldDescription,
  FieldError,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field";
import { Controller, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { toast } from "sonner";
import { AxiosError } from "axios";
import { userService } from "@/lib/api/userService";
import { Input } from "@/components/ui/input";
import { fileToBase64 } from "@/lib/helpers";

const MAX_FILE_SIZE = 5 * 1024 * 1024;
const ACCEPTED_IMAGES_TYPES = [
  "image/jpeg",
  "image/png",
  "image/jpg",
  "image/webp",
];

interface IUpdateUserAccountProps {
  userAccount: IUserAccountResponse;
  isEditing: boolean;
  setIsEditing: (isEditing: boolean) => void;
  onSaveUserAccount: () => void;
}

const formSchema = z
  .object({
    fullName: z
      .string()
      .min(3, "Nome deve ter ao menos 3 caracteres.")
      .max(255, "Nome completo não deve possuir mais de que 255 caracteres.")
      .optional(),
    email: z
      .email()
      .min(6, "E-mail de usuário deve ter ao menos 6 caracteres.")
      .max(320, "E-mail não deve possuir mais de que 20 caracteres.")
      .optional(),
    username: z
      .string()
      .min(8, "Nome de usuário deve ter ao menos 8 caracteres.")
      .max(20, "Nome de usuário não deve possuir mais de que 20 caracteres.")
      .optional(),
    isPrivate: z.boolean().optional(),
    profileImage: z
      .custom<File>((file) => file instanceof File, "Não é um arquivo válido.")
      .optional(),
    roleInviteCode: z.string().optional(),
    confirmationPassword: z.string(),
  })
  .superRefine((data, context) => {
    if (data.profileImage?.size && data.profileImage.size > MAX_FILE_SIZE) {
      context.issues.push({
        code: "custom",
        message: "O tamanho da imagem deve ter até 5MB.",
        path: ["profileImage"],
        input: data.profileImage,
      });
    }

    if (
      data.profileImage?.type &&
      !ACCEPTED_IMAGES_TYPES.includes(data.profileImage.type)
    ) {
      context.issues.push({
        code: "custom",
        message: "Apenas .jpg, .jpeg, .png e .webp são suportados.",
        path: ["profileImage"],
        input: data.profileImage,
      });
    }
  });

type FormData = z.infer<typeof formSchema>;

const UpdateUserAccount: React.FC<IUpdateUserAccountProps> = ({
  userAccount,
  isEditing,
  setIsEditing,
  onSaveUserAccount,
}) => {
  const { control, handleSubmit } = useForm<FormData>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: userAccount.email,
      fullName: userAccount.fullName,
      username: userAccount.username,
      isPrivate: userAccount.isPrivate,
    },
  });

  const onSubmit = async (data: FormData) => {
    try {
      let profileImage: string | undefined;

      if (data.profileImage) {
        profileImage = await fileToBase64(data.profileImage);
      }

      await userService.updateLoggedUserAccount({ ...data, profileImage });
      onSaveUserAccount();
      toast.success("Conta atualizada com sucesso!");
    } catch (error) {
      const axiosError = error as AxiosError<ProblemDetails>;
      const problem = axiosError.response?.data;
      toast.error(problem?.description || "Erro ao atualizar a conta.");
    }
  };

  return (
    <Card className="mx-auto w-full sm:max-w-md md:max-w-lg lg:max-w-4xl">
      <FormCardHeader
        title="Conta do Usuário"
        description="Altere as informações referente à sua conta aqui!"
      >
        <EditButton isEditing={isEditing} setIsEditing={setIsEditing} />
      </FormCardHeader>
      <CardContent>
        <form onSubmit={handleSubmit(onSubmit)} id="form-update-user-account">
          <FieldGroup>
            <Controller
              name="fullName"
              control={control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor={field.name}>Nome Completo</FieldLabel>
                  <Input
                    {...field}
                    id={field.name}
                    aria-placeholder="Nome Completo"
                    disabled={!isEditing}
                  />
                  {fieldState.error && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
            <Controller
              name="username"
              control={control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor={field.name}>Nome de Usuário</FieldLabel>
                  <Input
                    {...field}
                    id={field.name}
                    aria-placeholder="Nome de Usuário"
                    disabled={!isEditing}
                  />
                  {fieldState.error && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
            <Controller
              name="email"
              control={control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor={field.name}>E-mail</FieldLabel>
                  <Input
                    {...field}
                    id={field.name}
                    aria-placeholder="E-mail"
                    disabled={!isEditing}
                  />
                  {fieldState.error && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
            <Controller
              name="roleInviteCode"
              control={control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor={field.name}>
                    Código de Convite
                  </FieldLabel>
                  <Input
                    {...field}
                    id={field.name}
                    type="text"
                    disabled={!isEditing}
                    placeholder="Código de Convite"
                  />
                  <FieldDescription id={field.name}>
                    Insira um código de convite para fazer parte de um cargos
                    Administrador ou Professor.
                  </FieldDescription>
                  {fieldState.error && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />
            <Controller
              name="confirmationPassword"
              control={control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor={field.name}>
                    Senha de Confirmação
                  </FieldLabel>
                  <Input
                    {...field}
                    id={field.name}
                    type="password"
                    disabled={!isEditing}
                    placeholder="Sua senha"
                  />
                  <FieldDescription id={field.name}>
                    Confirme a sua senha para modificar informações sobre a
                    conta.
                  </FieldDescription>
                  {fieldState.error && (
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
            <Button type="submit" form="form-update-user-account">
              Atualizar
            </Button>
          </Field>
        </CardFooter>
      )}
    </Card>
  );
};

export default UpdateUserAccount;
