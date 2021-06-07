import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthorResourceCreated } from '../Resources/AuthorResourceCreated';
import { Injectable } from '@angular/core';
import { Author } from './../models/Author';

@Injectable
({
    providedIn:'root'
})

export class AuthorService {
  private baseUrl: string = environment.baseUrl+'api/';
  constructor(private http:HttpClient) {}

  public getAuthors():Observable<AuthorResourceCreated[]>{
      return this.http.get<AuthorResourceCreated[]>(this.baseUrl+'authors');
  }
  public getAuthorById(id:number):Observable<AuthorResourceCreated>{
      return this.http.get<AuthorResourceCreated>(this.baseUrl+'authors/'+id);
  }
  public addAuthor(author:Author){
    return this.http.post(this.baseUrl+'authors',author)
  }
  public updateAuthor(id:number,author:Author){
    return this.http.put(this.baseUrl+'authors/'+id,author)
  }
  public deleteAuthor(id:number){
      return this.http.delete(this.baseUrl+'authors/'+id);
  }

}
