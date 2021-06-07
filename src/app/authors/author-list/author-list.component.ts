import { Component, OnInit } from '@angular/core';
import { Author } from './../../models/Author';
import { Router } from '@angular/router';
import { AuthorService } from './../../services/author.service';
import { ToastrService } from 'ngx-toastr';
import { AuthorResourceCreated } from './../../Resources/AuthorResourceCreated';

@Component({
  selector: 'app-author-list',
  templateUrl: './author-list.component.html',
  styleUrls: ['./author-list.component.css'],
})
export class AuthorListComponent implements OnInit {
  public authors: AuthorResourceCreated[];
  constructor(
    private route: Router,
    private service: AuthorService,
    private toaster: ToastrService
  ) {}

  ngOnInit(): void {
    this.getAuthors();
  }

  private getAuthors() {
    this.service.getAuthors().subscribe((authors) => {
      this.authors = authors;
    });
  }
  public addAuthor() {
    this.route.navigate(['/author']);
  }
  public editAuthor(authorId: number) {
    this.route.navigate(['/author/' + authorId]);
  }
  public deleteAuthor(authorId: number) {
    this.service.deleteAuthor(authorId).subscribe(
      () => {
        this.toaster.success('The author has been deleted');
        this.getAuthors();
      },
      (error) => {
        this.toaster.error('Failed to delete publisher');
      }
    );
  }
}
