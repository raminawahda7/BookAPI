import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Author } from './../models/Author';
import { Observable, Observer } from 'rxjs';

export class AuthorService {
  private baseUrl: string = environment.baseUrl+'api/';
  constructor(private http:HttpClient) {}

  public getAuthors():Observable<Author[]>{
      return this.http.get<Author[]>(this.baseUrl+'authors');
  }
  public getAuthorById(authorId:number):Observable<Author>{
      return this.http.get<Author>(this.baseUrl+'authors'+authorId);
  }
}
