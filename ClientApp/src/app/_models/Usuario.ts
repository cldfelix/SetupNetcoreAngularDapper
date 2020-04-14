export interface Usuario {
   id?: number;
   nome : string;
   email : string;
   password?: string;
   dataNascimento?: Date;
   ativo : boolean;
   sexo: string;
   idSexo: number;
}
