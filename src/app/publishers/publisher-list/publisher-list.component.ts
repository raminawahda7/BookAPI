import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PublisherResource } from 'src/app/Resources/PublisherResource';
import { PublisherService } from 'src/app/services/publisher.service';
import { PublisherState } from 'src/app/store/states';
import { Store } from '@ngrx/store';
import { loadPublishers, deletePublisher } from 'src/app/store/actions/publisher.actions';
import { Publisher } from './../../models/Publisher';
import { State } from 'src/app/store';

@Component({
  selector: 'app-publisher-list',
  templateUrl: './publisher-list.component.html',
  styleUrls: ['./publisher-list.component.css'],
})
export class PublisherListComponent implements OnInit {
  public publishers: PublisherResource[];
  constructor(
    private store: Store<State>,
    private router: Router,
    private service: PublisherService,
    private toaster: ToastrService
  ) //TODO don't inject component ... later inject confirmation dialog service here if you want.
  {
    this.store
      .select((appState) => appState.publishers.publishers)
      .subscribe((data) => {
        this.publishers = data;
        console.log('Publishers :', data);
      });
  }

  ngOnInit(): void {
    if (this.publishers.length == 0) {
      this.getPublishers();
    }
  }
  private getPublishers() {
    this.store.dispatch(loadPublishers());
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
    this.store.dispatch(deletePublisher({id:publisherId}))

    //TODO add confirmation dialog implementation with toaster.success and toaster.error.
  }
}
