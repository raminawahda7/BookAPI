import { NgModule,NO_ERRORS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { PublisherService } from './services/publisher.service';
import { ConfirmationDialogService } from './services/confirmation-dialog.service';
import { BookComponent } from './books/book/book.component';
import { BookListComponent } from './books/book-list/book-list.component';
import { AuthorComponent } from './authors/author/author.component';
import { AuthorListComponent } from './authors/author-list/author-list.component';
import { PublisherComponent } from './publishers/publisher/publisher.component';
import { PublisherListComponent } from './publishers/publisher-list/publisher-list.component';
import { HomeComponent } from './home/home.component';
import { NavComponent } from './nav/nav.component';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
import { AppRoutingModule } from './app-routing.module';
import { AuthorService } from './services/author.service';
import { NgbdDatepickerPopup } from './datepicker/datepicker-popup';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { StoreModule } from '@ngrx/store';
import { reducers, metaReducers } from './reducers';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from '../environments/environment';

@NgModule({
  declarations: [
    AppComponent,
    BookComponent,
    BookListComponent,
    AuthorComponent,
    AuthorListComponent,
    PublisherComponent,
    PublisherListComponent,
    HomeComponent,
    NavComponent,
    ConfirmationDialogComponent,
    NgbdDatepickerPopup,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    ToastrModule.forRoot(),
    StoreModule.forRoot(reducers, { metaReducers }),
    !environment.production ? StoreDevtoolsModule.instrument() : [],
  ],
  providers: [PublisherService, AuthorService, ConfirmationDialogService],
  bootstrap: [AppComponent],
  schemas: [NO_ERRORS_SCHEMA],
})
export class AppModule {}
