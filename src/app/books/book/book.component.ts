import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PublisherResource } from 'src/app/Resources/PublisherResource';
import { BookService } from 'src/app/services/book.service';
import { PublisherService } from 'src/app/services/publisher.service';
import { Book } from 'src/app/models/Book';
import { AuthorResourceCreated } from 'src/app/Resources/AuthorResourceCreated';
import { AuthorService } from './../../services/author.service';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css'],
})
export class BookComponent implements OnInit {
  public publisherList: PublisherResource[];
  public authorList: AuthorResourceCreated[];

  public id: number = 0;
  public testvalue: boolean = false;
  // TODO Edit Book Form
  formData: FormGroup;
  constructor(
    public service: BookService,
    private pubservice: PublisherService,
    private authorService:AuthorService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private toaster: ToastrService
  ) {
    this.formData = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      isAvailable: false,
      publishedDate: [''],
      publisherId: ['', Validators.required],
      authorIds: [],
    });

    console.log('isAvailable', this.formData.controls['isAvailable'].value);
  }

  ngOnInit(): void {
    this.formData.reset();
    this.pubservice.getPublishers().subscribe((publisher) => {
      this.publisherList = publisher;
    });
    this.authorService.getAuthors().subscribe((author) => {
      this.authorList = author;
    });
    this.route.params.subscribe((params) => {
      this.id = params['id'];
    });
    if (this.id != null) {
      this.service.getBookById(this.id).subscribe(
        (book) => {
          this.formData.patchValue(book);
        },
        (error) => {
          this.toaster.error('An error occurred on get record.');
        }
      );
    } else {
      this.formData.reset();
    }
  }
  public onSubmit(): void {
    console.log(
      'selected publisher',
      this.formData.controls['publisherId'].value
    );

    if (this.formData.invalid) {
      alert('Invalid input');
    }
    const formValue = this.formData.getRawValue();
    const book = {
      title: formValue.title,
      description: formValue.description,
      isAvailable: formValue.isAvailable === true ? true : false,
      publishedDate: new Date(`${formValue.publishedDate.year}-${formValue.publishedDate.month}-${formValue.publishedDate.day}`),
      publisherId:  formValue.publisherId,
      authorIds:  formValue.authorIds,
    } as Book;
    console.log('book', book)
    if (this.id === undefined) {
      this.insertRecord(book);
    } else {
      this.updateRecord(book);
    }
  }
  public insertRecord(book: Book) {
    this.change();
    this.service.addBook(book).subscribe(
      () => {
        this.toaster.success('Registration Success');
        this.formData.reset();
        this.router.navigate(['/books']);
      },
      () => {
        this.toaster.error('An error occurred on insert the record');
      }
    );
  }
  public updateRecord(book: Book) {
    this.change();
    this.service.updateBook(this.id, book).subscribe(
      () => {
        this.toaster.success('Updated Successfully');
        this.formData.reset();
        this.router.navigate(['/books']);
      },
      () => {
        this.toaster.error('An error occurred on insert the record');
      }
    );
  }
  public change() {
    if (this.formData.controls['isAvailable'].value !== true) {
      this.formData.controls['isAvailable'].setValue(false);
    }
  }
  private convertStringToDate(date:any) {
    return new Date(`${date.day}-${date.month}-${date.year}`);
  }
  public cancel() {
    this.router.navigate(['/books']);
  }
}
