import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PublisherService } from 'src/app/services/publisher.service';

@Component({
  selector: 'app-publisher-list',
  templateUrl: './publisher-list.component.html',
  styleUrls: ['./publisher-list.component.css'],
})
export class PublisherListComponent implements OnInit {
  public publishers: any;
  constructor(
    private router: Router,
    private service: PublisherService,
    private toaster: ToastrService,
    // don't inject component ... later inject confirmation dialog service here if yopu want.
  ) {}

  ngOnInit(): void {
    this.getPublishers();
  }
  private getPublishers() {
    this.service.getPublishers().subscribe((publishers) => {
      this.publishers = publishers;
    });
  }
  public addPublisher() {
    this.router.navigate(['/Publishers']);
  }
  public editPublisher(publisherId: number) {
    this.router.navigate(['/Publishers/' + publisherId]);
  }
  public deletePublisher(publisherId:number) {
    this.service.deletePublisher(publisherId);
    // Later: add confirmation dialog implementation with toaster.success and toaster.error.
  }
}
