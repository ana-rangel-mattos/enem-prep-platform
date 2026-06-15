"use client";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Field,
  FieldDescription,
  FieldError,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field";
import { Calendar } from "@/components/ui/calendar";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { cn } from "@/lib/utils";
import React, { useState } from "react";
import { z } from "zod";
import Loading from "@/components/Loading";
import { authService } from "@/lib/api/authService";
import { toast } from "sonner";
import { Controller, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Checkbox } from "@/components/ui/checkbox";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { CalendarIcon } from "lucide-react";
import { format } from "date-fns";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { IRegisterUser } from "@/lib/types/user.interface";
import { fileToBase64 } from "@/lib/helpers";

const MAX_FILE_SIZE = 5 * 1024 * 1024;
const ACCEPTED_IMAGES_TYPES = [
  "image/jpeg",
  "image/png",
  "image/jpg",
  "image/webp",
];

const registerSchema = z
  .object({
    fullName: z
      .string({ error: "Nome Completo é obrigatório." })
      .min(3, "Nome deve ter ao menos 3 caracteres.")
      .max(255, "Nome completo não deve possuir mais de que 255 caracteres."),
    username: z
      .string({ error: "Nome de usuário é obrigatório." })
      .min(8, "Nome de usuário deve ter ao menos 8 caracteres.")
      .max(20, "Nome de usuário não deve possuir mais de que 20 caracteres."),
    email: z
      .email({ error: "E-mail é obrigatório." })
      .min(6, "E-mail de usuário deve ter ao menos 6 caracteres.")
      .max(320, "E-mail não deve possuir mais de que 20 caracteres."),
    password: z
      .string({ error: "Senha é obrigatório." })
      .min(8, "Senha deve ter ao menos 8 caracteres.")
      .regex(/[A-Z]/, "Must contain one uppercase letter")
      .regex(/[a-z]/, "Must contain one lowercase letter")
      .regex(/[0-9]/, "Must contain one number"),
    confirmPassword: z.string({ error: "Confirme a senha." }).optional(),
    isPrivate: z.boolean().optional(),
    dateOfBirth: z.coerce
      .date({ error: "Data de nascimento é obrigatória." })
      .transform((date) => date.toISOString()),
    profileImage: z
      .custom<File>((file) => file instanceof File, "Não é um arquivo válido.")
      .optional(),
    code: z
      .string()
      .max(50, "Código de convite não deve possuir mais de que 50 characteres.")
      .optional(),
  })
  .superRefine((data, context) => {
    if (data.confirmPassword !== data.password) {
      context.issues.push({
        code: "custom",
        message: "As senhas diferem.",
        path: ["password"],
        input: data.password,
      });

      context.issues.push({
        code: "custom",
        message: "As senhas diferem.",
        path: ["confirmPassword"],
        input: data.confirmPassword,
      });
    }

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

type FormData = z.infer<typeof registerSchema>;

const SignupForm: React.FC<React.ComponentProps<"div">> = ({
  className,
  ...props
}) => {
  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);
  const { control, handleSubmit, reset } = useForm<FormData>({
    resolver: zodResolver(registerSchema),
    defaultValues: {
      isPrivate: false,
    },
  });

  if (isLoading) return <Loading message="Processando cadastro..." />;

  const onSubmit = async (data: FormData) => {
    try {
      if (data.confirmPassword) {
        delete data.confirmPassword;
      }

      if (!data.profileImage) {
        delete data.profileImage;
      }

      let profileImage: string | undefined;

      if (data.profileImage) {
        profileImage = await fileToBase64(data.profileImage);
      }

      const payload: IRegisterUser = {
        fullName: data.fullName,
        username: data.username,
        email: data.email,
        password: data.password,
        isPrivate: data.isPrivate,
        dateOfBirth: data.dateOfBirth,
        code: data.code,
        profileImage,
      };

      setIsLoading(true);
      await authService.registerUser(payload);
      reset();
      router.push("/login");
    } catch (error: any) {
      const backendError =
        error.response?.data?.details || "Ocorreu um erro inesperado.";
      toast.error(backendError);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader className="text-center">
          <CardTitle className="text-xl">Crie a sua conta</CardTitle>
          <CardDescription>
            Preencha o formulário abaixo para se registrar.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit(onSubmit)}>
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
                    <FieldLabel htmlFor={field.name}>
                      Nome de Usuário
                    </FieldLabel>
                    <Input
                      {...field}
                      id={field.name}
                      aria-placeholder="Nome de Usuário"
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
                    />
                    {fieldState.error && (
                      <FieldError errors={[fieldState.error]} />
                    )}
                  </Field>
                )}
              />
              <Field className="grid grid-cols-2 gap-4">
                <Controller
                  name="password"
                  control={control}
                  render={({ field, fieldState }) => (
                    <Field data-invalid={fieldState.invalid}>
                      <FieldLabel htmlFor={field.name}>Senha</FieldLabel>
                      <Input
                        {...field}
                        id={field.name}
                        type="password"
                        aria-placeholder="Senha"
                      />
                      <FieldDescription id={field.name}>
                        Deve conter ao menos 8 caracteres.
                      </FieldDescription>
                      {fieldState.error && (
                        <FieldError errors={[fieldState.error]} />
                      )}
                    </Field>
                  )}
                />
                <Controller
                  name="confirmPassword"
                  control={control}
                  render={({ field, fieldState }) => (
                    <Field data-invalid={fieldState.invalid}>
                      <FieldLabel htmlFor={field.name}>
                        Confirme a Senha
                      </FieldLabel>
                      <Input
                        {...field}
                        id={field.name}
                        type="password"
                        aria-placeholder="Confirme a Senha"
                      />
                      {fieldState.error && (
                        <FieldError errors={[fieldState.error]} />
                      )}
                    </Field>
                  )}
                />
              </Field>
              <Controller
                name="dateOfBirth"
                control={control}
                render={({ field, fieldState }) => (
                  <Field data-invalid={fieldState.invalid}>
                    <FieldLabel htmlFor={field.name}>
                      Data de Nascimento
                    </FieldLabel>
                    <Popover>
                      <PopoverTrigger asChild>
                        <Button
                          variant="outline"
                          data-empty={!field.value}
                          className="data-[empty=true]:text-muted-foreground w-70 justify-start text-left font-normal"
                        >
                          <CalendarIcon />
                          {field.value ? (
                            format(field.value, "PPP")
                          ) : (
                            <span>Pick a date</span>
                          )}
                        </Button>
                      </PopoverTrigger>
                      <PopoverContent className="w-auto p-0">
                        <Calendar
                          {...field}
                          mode="single"
                          id={field.name}
                          captionLayout="dropdown"
                          selected={field.value}
                          onSelect={field.onChange}
                        />
                      </PopoverContent>
                    </Popover>
                  </Field>
                )}
              />
              <Controller
                name="code"
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
                      aria-placeholder="Código de Convite de um Administrador"
                    />
                    {fieldState.error && (
                      <FieldError errors={[fieldState.error]} />
                    )}
                  </Field>
                )}
              />
              <Controller
                name="isPrivate"
                control={control}
                render={({ field, fieldState }) => (
                  <Field
                    data-invalid={fieldState.invalid}
                    orientation="horizontal"
                  >
                    <FieldLabel htmlFor={field.name}>Perfil Privado</FieldLabel>
                    <Checkbox id={field.name} name={field.name} />
                    {fieldState.error && (
                      <FieldError errors={[fieldState.error]} />
                    )}
                  </Field>
                )}
              />
              <Controller
                name="profileImage"
                control={control}
                render={({ field, fieldState }) => (
                  <Field data-invalid={fieldState.invalid}>
                    <FieldLabel htmlFor={field.name}>
                      Imagem de Perfil
                    </FieldLabel>
                    <Input
                      id={field.name}
                      type="file"
                      accept="image/*"
                      onChange={(event) => {
                        field.onChange(event.target.files?.item(0));
                      }}
                      aria-placeholder="Imagem de Perfil"
                    />
                    {fieldState.error && (
                      <FieldError errors={[fieldState.error]} />
                    )}
                  </Field>
                )}
              />
              <Field>
                <Button type="submit">Criar Conta</Button>
                <FieldDescription className="text-center">
                  Já tem conta? <Link href="/login">Login</Link>
                </FieldDescription>
              </Field>
            </FieldGroup>
          </form>
        </CardContent>
      </Card>
    </div>
  );
};

export default SignupForm;
