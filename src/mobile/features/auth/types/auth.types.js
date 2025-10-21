export const AuthTypes = {
  LoginRequest: {
    email: "",
    password: "",
  },
  LoginResponse: {
    token: "",
    user: {
      id: "",
      email: "",
      name: "",
    },
  },
};
