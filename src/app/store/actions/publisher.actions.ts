import { createAction, props } from '@ngrx/store';
import { PublisherResource } from 'src/app/Resources/PublisherResource';

export const loadPublishers = createAction(
  '[Publisher] Load Publishers'
  // props<{ name: Publisher }>()
);

export const loadPublishersSuccess = createAction(
  '[Publisher] Load Publishers Success',
  props<{publishers : PublisherResource[]}>()
);

export const loadPublishersFailure = createAction(
  '[Publisher] Load Publishers Failure',
  props<{ error: any }>()
);
