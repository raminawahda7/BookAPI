import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { PublisherService } from 'src/app/services/publisher.service';
import * as publisherActions from '../actions/publisher.actions';
import { catchError, map, switchMap, exhaustMap, tap } from 'rxjs/operators';
import { of } from 'rxjs';
@Injectable()
export class PublisherEffectEffects {
  constructor(
    private actions$: Actions,
    private pubservice: PublisherService
  ) {}

  loadPublishers = createEffect(() => 
    this.actions$.pipe(
      // filter out the actions, except for `[Customers Page] Opened`
      ofType(publisherActions.loadPublishers),
      switchMap(() =>
        // call the service
        this.pubservice.getPublishers().pipe(
          // return a Success action when the HTTP request was successfull (`[Customers Api] Load Sucess`)
          map((response) => publisherActions.loadPublishersSuccess({ publishers: response })),
          // return a Failed action when something went wrong during the HTTP request (`[Customers Api] Load Failed`)
          catchError((error) => of(publisherActions.loadPublishersFailure(error)))
        )
      )
    )
  );
}
