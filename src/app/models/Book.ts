export interface Book{
    title:string;
    description:string;
    isAvailable:boolean;
    publishedDate:Date;   
    publisherId: number;
    authorIds: number[];
}