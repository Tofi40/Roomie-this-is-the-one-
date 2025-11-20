export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  bio: string;
  birthDate: string;
  gender: string;
  isSmoker: boolean;
  hasPets: boolean;
  createdAt: string;
}
