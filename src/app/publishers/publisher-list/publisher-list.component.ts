import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PublisherResource } from 'src/app/Resources/PublisherResource';
import { PublisherService } from 'src/app/services/publisher.service';
import { IAppState } from 'src/app/store/interfaces/publisher.interface';
import { Store } from '@ngrx/store';
import { loadPublishers } from 'src/app/store/actions/publisher.actions';
import { Publisher } from './../../models/Publisher';

@Component({
  selector: 'app-publisher-list',
  templateUrl: './publisher-list.component.html',
  styleUrls: ['./publisher-list.component.css'],
})
export class PublisherListComponent implements OnInit {
  public publishers: PublisherResource[];
  constructor(
    private store:Store<IAppState>,
    private router: Router,
    private service: PublisherService,
    private toaster: ToastrService
  ) // don't inject component ... later inject confirmation dialog service here if yopu want.
  {}

  ngOnInit(): void {
    this.getPublishers();
    this.store.subscribe(data => { this.publishers = data.AppState.publishers ;console.log('Publishers :', this.publishers) })
  }
  private getPublishers() {
    // this.service.getPublishers().subscribe((publishers) => {
    //   this.publishers = publishers;
    // });
    this.store.dispatch(loadPublishers())

  }
  public addPublisher() {
    this.router.navigate(['/publisher']);
  }
  public editPublisher(publisherId: number) {
    this.router.navigate(['/publisher/' + publisherId]);
    console.log('...............Id: ');
    console.log(publisherId);
    //TODO Delete console.logs
  }
  public deletePublisher(publisherId: number) {
    this.service.deletePublisher(publisherId).subscribe(
      () => {
        this.toaster.success('The publisher has been deleted');
        this.getPublishers();
      },
      (error) => {
        this.toaster.error('Failed to delete publisher');
      }
    );

    //TODO add confirmation dialog implementation with toaster.success and toaster.error.
  }

}
