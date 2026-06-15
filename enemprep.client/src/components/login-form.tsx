"use client";

import { z } from "zod";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import {
  Field,
  FieldDescription,
  FieldError,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import Link from "next/link";
import React, { useState } from "react";
import { authService } from "@/lib/api/authService";
import { useAppDispatch } from "@/hooks/use-store";
import { setUser } from "@/lib/features/auth/authSlice";
import { useRouter } from "next/navigation";
import { toast } from "sonner";
import { Controller, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import Loading from "@/components/Loading";

const schema = z
  .object({
    username: z.string().optional(),
    email: z.string().email().optional().or(z.literal("")),
    password: z.string(),
  })
  .superRefine((data, context) => {
    if (!data.email && !data.username) {
      context.issues.push({
        code: "custom",
        message: "Email ou username devem ser providos.",
        path: ["username"],
        input: data.username,
      });

      context.issues.push({
        code: "custom",
        message: "Email ou username devem ser providos.",
        path: ["email"],
        input: data.email,
      });
    }
  });

type FormData = z.infer<typeof schema>;

export const LoginForm: React.FC<React.ComponentProps<"div">> = ({
  className,
  ...props
}) => {
  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);
  const dispatch = useAppDispatch();
  const { control, handleSubmit, reset } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: {
      username: "",
      email: "",
      password: "",
    },
  });

  const onSubmit = async (data: FormData) => {
    try {
      setIsLoading(true);

      await authService.loginUser(data);
      const userProfile = await authService.getLoggedUser();
      dispatch(setUser(userProfile));

      reset();
      router.push("/dashboard");
    } catch (error: any) {
      const backendError =
        error.response?.data || "Ocorreu um erro inesperado.";
      toast.error(backendError);
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) return <Loading message="Processando Login..." />;

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader className="text-center">
          <CardTitle className="text-xl">Bem-vindo(a) de volta!</CardTitle>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit(onSubmit)}>
            <FieldGroup>
              <Controller
                name="username"
                control={control}
                render={({ field, fieldState }) => (
                  <Field data-invalid={fieldState.invalid}>
                    <FieldLabel htmlFor={field.name}>Username</FieldLabel>
                    <Input
                      {...field}
                      id={field.name}
                      aria-placeholder="usr-name05"
                      autoComplete="off"
                    />
                    <FieldDescription>
                      O nome de usuário usado para criar a conta.
                    </FieldDescription>
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
                      type="email"
                      aria-placeholder="user@example.com"
                      autoComplete="off"
                    />
                    <FieldDescription>
                      O e-mail usado para criar a conta.
                    </FieldDescription>
                    {fieldState.error && (
                      <FieldError errors={[fieldState.error]} />
                    )}
                  </Field>
                )}
              />
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
                      autoComplete="off"
                    />
                    {fieldState.error && (
                      <FieldError errors={[fieldState.error]} />
                    )}
                  </Field>
                )}
              />
              <Field>
                <Button type="submit">Login</Button>
                <FieldDescription className="text-center">
                  Não tem conta? <Link href="/register">Cadastre-se</Link>
                </FieldDescription>
              </Field>
            </FieldGroup>
          </form>
        </CardContent>
      </Card>
    </div>
  );
};
