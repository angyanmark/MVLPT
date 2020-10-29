export interface Token {
  email: string;
  exp: number;
  iat: number;
  iss: string;
  nameid: string;
  nbf: number;
  role: string;
  unique_name: string;
}
