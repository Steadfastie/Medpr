export interface User {
  userId?: string;
  login: string;
  password: string;
  role?: string;
  accessToken?: string;
  newPassword?: string;
  fullName?: string;
  dateOfBirth?: string;
}
