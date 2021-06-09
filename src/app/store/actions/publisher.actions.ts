import { createAction, props } from '@ngrx/store';
import { PublisherResource } from 'src/app/Resources/PublisherResource';
import { Publisher } from './../../models/Publisher';

export const loadPublishers = createAction(
  '[Publisher] Load Publishers'
  // props<{ name: Publisher }>()
);

export const loadPublishersSuccess = createAction(
  '[Publisher] Load Publishers Success',
  props<{ publishers: PublisherResource[] }>()
);

export const loadPublishersFailure = createAction(
  '[Publisher] Load Publishers Failure',
  props<{ error: any }>()
);

export const createPublisher = createAction(
  '[Publisher] Create Publisher',
  props<{ newPublisher: Publisher }>()

);

export const createPublisherSuccess = createAction(
  '[Publisher] Create Publisher Success',
  props<{ newPublisher: PublisherResource }>()
);

export const createPublisherFailure = createAction(
  '[Publisher] Create Publisher Failure',
  props<{ error: string }>()
);
