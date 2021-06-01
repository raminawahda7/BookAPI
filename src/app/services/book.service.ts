import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Book } from '../models/Book';
import { Observable } from 'rxjs';
import { environment } from './../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BookService {
  private baseUrl: string = environment.baseUrl + 'api/';

  constructor(private http: HttpClient) {}
  public addBook(book: Book) {
    return this.http.post(this.baseUrl + 'Books', book);
  }
  public updateBook(id:number,book:Book){
    return this.http.put(this.baseUrl+'Books/'+id,book);
  }
  public getBooks():Observable<Book[]>{
    return this.http.get<Book[]>(`${this.baseUrl}Books`);
  }
  public getBookById(id:number):Observable<Book>{
    return this.http.get<Book>(`${this.baseUrl}Books/${id}`);
  }
  public deleteBook(id:number){
    return this.http.delete(`${this.baseUrl}Books/${id}`);
  }
  
}
