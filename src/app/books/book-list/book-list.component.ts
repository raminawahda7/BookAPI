import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BookService } from 'src/app/services/book.service';
import { Book } from './../../models/Book';
import { BookResource } from './../../Resources/BookResource';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  public books:BookResource[];
  constructor(
    private router: Router,
    private service: BookService,
    private toaster: ToastrService
  ) { }

  ngOnInit(): void {
    this.getBooks();
  }
  private getBooks() {
    this.service.getBooks().subscribe((books) => {
      this.books = books.map(x => {
        x.authorsFullNames = x.authorNames.map(x=>x.fullName).join(",")
        return x;
      });
      console.log(this.books)
    });
  }
  public addBook() {
    this.router.navigate(['/book']);
  }
  public editBook(bookId: number) {
    this.router.navigate(['/book/' + bookId]);
    console.log('...............Id: ');
    console.log(bookId);
    //TODO Delete console.logs
  }
  public deleteBook(bookId: number) {
    this.service.deleteBook(bookId).subscribe(
      () => {
        this.toaster.success('The book has been deleted');
        this.getBooks();
      },
      (error) => {
        this.toaster.error('Failed to delete book');
      }
    );

    // Later: add confirmation dialog implementation with toaster.success and toaster.error.
  }
}

