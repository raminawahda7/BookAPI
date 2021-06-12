import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PublisherService } from 'src/app/services/publisher.service';
import { ToastrService } from 'ngx-toastr';
import { Publisher } from './../../models/Publisher';
import { NgForm } from '@angular/forms';
import { Store } from '@ngrx/store';
import { State } from 'src/app/store';
import { createPublisher,updatePublisher } from 'src/app/store/actions/publisher.actions';

@Component({
  selector: 'app-publisher',
  templateUrl: './publisher.component.html',
  styleUrls: ['./publisher.component.css'],
})
export class PublisherComponent implements OnInit {
  public formData: Publisher;
  constructor(
    private store:Store<State>,
    public service: PublisherService,
    private router: Router,
    private route: ActivatedRoute,
    private toaster: ToastrService
  ) {}
  private id = 0; // This global id will be used to check if save button should insert publisher or update the publisher.

  ngOnInit(): void {
    // reset form
    this.resetFrom();
    // getPublisher by id if it's not null
    // ..../publishers/1 
    this.route.params.subscribe((params) => {
      this.id = params['id'];
    });
    // SO > use router.params
    if (this.id != null) {
      this.service.getPublisherById(this.id).subscribe(
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
      name: '',
    };
  }

  public onSubmit(form: NgForm) {
    
    if (this.id === undefined) {
      this.insertRecord(form);
    } else {
      this.updateRecord(form);
    }
  }

  public insertRecord(form: NgForm) {
    let publisher: Publisher = {
      name: form.form.value.name,
    };
    this.store.dispatch(createPublisher({newPublisher:publisher}));
    this.router.navigate(['/publishers']);
  }
  public updateRecord(form: NgForm) {

    this.store.dispatch(updatePublisher({id:this.id,publisherToUpdate:form.form.value}))
    this.router.navigate(['/publishers']);

    // this.service.updatePublisher(this.id, form.form.value).subscribe(
    //   () => {
    //     this.toaster.success('Updated Successfully');
    //     this.resetFrom(form);
    //     this.router.navigate(['/publishers']);
    //   },
    //   () => {
    //     this.toaster.error('An error occurred on insert the record');
    //   }
    // );
  }

  public cancel() {
    this.router.navigate(['/publishers']);
  }
}
