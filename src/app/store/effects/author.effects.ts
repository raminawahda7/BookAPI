import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { AuthorService } from './../../services/author.service';
import * as authorActions from '../actions/author.actions';
import { switchMap, catchError, map } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable()
export class AuthorEffects {
  constructor(
    private actions$: Actions,
    private authorService: AuthorService
  ) {}

  loadAuthors = createEffect(() =>
    this.actions$.pipe(
      // filter out the actions, except for `[Author Page] Opened`
      ofType(authorActions.loadAuthors),
      switchMap(() =>
        // call the service
        this.authorService.getAuthors().pipe(
          // return a Success action when the HTTP request was successfull (`[Authors Api] Load Sucess`)
          map((response) =>
            authorActions.loadAuthorsSuccess({ authors: response })
          ),
          // return a Failed action when something went wrong during the HTTP request (`[Authors Api] Load Failed`)
          catchError((error) => of(authorActions.loadAuthorsFailure(error)))
        )
      )
    )
  );
}
