export interface IRolUser {
    id: number;
    email: string;
    personName: string;
    rolName: string;
    isDeleted: boolean;
    rolId: number; // Asegúrate de que este campo exista en tu API
    userId: number; // Asegúrate de que este campo exista en tu API
  }