import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthorService } from './../../services/author.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-author',
  templateUrl: './author.component.html',
  styleUrls: ['./author.component.css'],
})
export class AuthorComponent implements OnInit {
  public id: number = 0;
  formData: FormGroup;
  constructor(
    public service: AuthorService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private toaster: ToastrService
  ) {
    this.formData = this.fb.group({
      firstName: ['', Validators.required],
      lastName: [''],
      age: ['', Validators.min(15)],
      email: ['', Validators.email],
    });

    // this.service.getAuthors().subscribe((res) => {
    //   this.formData.patchValue(res);
    // });
  }

  ngOnInit(): void {
    this.formData.reset();
    this.route.params.subscribe((params) => {
      this.id = params['id'];
    });
    if (this.id != null) {
      this.service.getAuthorById(this.id).subscribe((author) => {
        this.formData.patchValue(author);
      },(error) => {
        this.toaster.error('An error occurred on get record.');
      });
    }else{
      this.formData.reset();
    }
  }
  public onSubmit(): void {
    
    if (this.formData.invalid) {
      // stop here if it's invalid
      alert('Invalid input');
    } 
    if (this.id === undefined) {
      this.insertRecord();
    } else {
      this.updateRecord();
    }
  }
  public insertRecord() {
    this.service.addAuthor(this.formData.getRawValue()).subscribe(
      () => {
        this.toaster.success('Registration Success');
        this.formData.reset();
        this.router.navigate(['/authors']);
      },
      () => {
        this.toaster.error('An error occurred on insert the record');
      }
    );
  }
  public updateRecord() {
    this.service.updateAuthor(this.id, this.formData.getRawValue()).subscribe(
      () => {
        this.toaster.success('Updated Successfully');
        this.formData.reset();
        this.router.navigate(['/authors']);
      },
      () => {
        this.toaster.error('An error occurred on insert the record');
      }
    );
  }
  public cancel() {
    this.router.navigate(['/authors']);
  }
}
