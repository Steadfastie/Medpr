export interface User {
  [x: string]: string | undefined;
  userId?: string;
  login: string;
  password: string;
  role?: string;
  accessToken?: string;
  newPassword?: string;
  fullName?: string;
  dateOfBirth?: string;
}
