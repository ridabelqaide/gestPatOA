export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  username: string;
  role: string;
  email: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  roleId: number;
}
