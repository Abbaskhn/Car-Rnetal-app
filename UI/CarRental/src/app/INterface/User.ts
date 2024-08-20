
    export interface User {
      id: number;
      firstName: string;
      lastName: string;
      username: string;
      email: string;
      passwordHash: string;
      token?: string;
      role?: string;
      refreshToken?: string;
      refreshTokenExpiryTime?: Date;
     
    }
  