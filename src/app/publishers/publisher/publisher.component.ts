import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PublisherService } from 'src/app/services/publisher.service';
import { ToastrService } from 'ngx-toastr';
import { Publisher } from './../../models/Publisher';
import { FormGroup, NgForm } from '@angular/forms';

@Component({
  selector: 'app-publisher',
  templateUrl: './publisher.component.html',
  styleUrls: ['./publisher.component.css'],
})
export class PublisherComponent implements OnInit {
  public formData: Publisher;
  constructor(
    public service: PublisherService,
    private router: Router,
    private route: ActivatedRoute,
    private toaster: ToastrService
  ) {}

  ngOnInit(): void {
    // reset form
    this.resetFrom();
    // getPublisher by id if it's not null
    let id;
    this.route.params.subscribe((params) => {
      id = params['id'];
    });
    // SO > use router.params
    if (id != null) {
      this.service.getPublisherById(id).subscribe(
        (publisher) => {
          this.formData = publisher;
        },
        (error) => {
          this.toaster.error('An error occurred on get record.');
        }
      );
    } else {
      this.resetFrom();
    }
  }
  //ResetForm-OnSubmit - insertRecord - updateRecord - Cancel
  private resetFrom(form?: NgForm) {
    if (form != null) {
      form.form.reset();
    }
    this.formData = {
      id: 0,
      name: '',
    };
  }

  public onSubmit(form: NgForm) {
    if(form.value.id===0){
      this.insertRecord(form);
    }else{
      this.updateRecord(form);
    }

  }

  public insertRecord(form: NgForm) {
    this.service.addPublisher(form.form.value).subscribe(
      () => {
        console.log(form.form.value.id);
        this.toaster.success('Registration Success');

        this.resetFrom(form);
        this.router.navigate(['/publishers']);
      },
      () => {
        this.toaster.error('An error occurred on insert the record');
      }
    );
  }
  public updateRecord(form: NgForm) {
    this.service.updatePublisher(form.form.value.id, form.form.value).subscribe(
      () => {
        this.toaster.success('Updated Successfully');
        this.resetFrom(form);
        this.router.navigate(['/publishers']);
      },
      () => {
        this.toaster.error('An error occurred on insert the record');
      }
    );
  }

  public cancel() {
    this.router.navigate(['/publishers']);
  }
}
