import { createAction, props } from '@ngrx/store';

export const loadPublishers = createAction(
  '[Publisher] Load Publishers'
);

export const loadPublishersSuccess = createAction(
  '[Publisher] Load Publishers Success',
  props<{ data: any }>()
);

export const loadPublishersFailure = createAction(
  '[Publisher] Load Publishers Failure',
  props<{ error: any }>()
);
