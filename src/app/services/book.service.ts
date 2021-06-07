import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { Book } from './../models/Book';
import { BookResource } from './../Resources/BookResource';

@Injectable
({
    providedIn:'root'
})

export class BookService {
  private baseUrl: string = environment.baseUrl+'api/';
  constructor(private http:HttpClient) {}

  public getBooks():Observable<BookResource[]>{
      return this.http.get<BookResource[]>(this.baseUrl+'Books');
  }
  public getBookById(id:number):Observable<BookResource>{
      return this.http.get<BookResource>(this.baseUrl+'Books/'+id);
  }
  public addBook(book:Book){
    console.log(book)
    return this.http.post(this.baseUrl+'Books', book)
  }
  public updateBook(id:number,book:Book){
    return this.http.put(this.baseUrl+'Books/'+id,book)
  }
  public deleteBook(id:number){
      return this.http.delete(this.baseUrl+'Books/'+id);
  }

}
