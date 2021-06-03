import { NgModule} from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthorListComponent } from './authors/author-list/author-list.component';
import { BookListComponent } from './books/book-list/book-list.component';
import { BookComponent } from './books/book/book.component';
import { HomeComponent } from './home/home.component';
import { PublisherListComponent } from './publishers/publisher-list/publisher-list.component';
import { PublisherComponent } from './publishers/publisher/publisher.component';

const routes: Routes = [
  {path:'home',component:HomeComponent},
  {path:'publishers',component:PublisherListComponent},
  {path:'publisher',component:PublisherComponent},
  {path:'publisher/:id',component:PublisherComponent},
  {path:'books',component:BookListComponent},
  {path:'book',component:BookComponent},
  {path:'authors',component:AuthorListComponent},
  {path:'**',redirectTo:'home',pathMatch:'full'}
];


@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })


export class AppRoutingModule {}
