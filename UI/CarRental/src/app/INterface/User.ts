// models.ts

export interface IUser {
  id: number;
  firstName: string;
  lastName: string;
  userName: string;
  password: string;
  email: string;
  token: string;
  roleId: number;
  role?: IRole;  // Optional, as it might be included in a separate API call
}

export interface IRole {
  id: number;
  name: string;
}

export interface IUserVM {
  firstName: string;
  lastName: string;
  userName: string;
  password: string;
  email: string;
  roleId: number;
}

export interface ILogin {
  userName: string;
  password: string;
}
