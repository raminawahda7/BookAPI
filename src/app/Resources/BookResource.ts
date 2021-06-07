export interface BookResource{
    id:number;
    title:string;
    description:string;
    isAvailable:boolean;
    publishedDate:Date;   
    publisher: string;
    authorNames:any[];
    authorsFullNames: string;

}