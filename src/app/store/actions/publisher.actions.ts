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

export const updatePublisher = createAction(
  '[Publisher] Update Publisher',
  props<{ id: number; publisherToUpdate: Publisher }>()
);

export const updatePublisherSuccess = createAction(
  '[Publisher] Update Publisher Success',
  props<{ updatedPublisher: PublisherResource }>()
);

export const updatePublisherFailure = createAction(
  '[Publisher] Update Publisher Failure ',
  props<{ error: string }>()
);

export const deletePublisher = createAction(
  '[Publisher] Delete Publisher',
  props<{ id: number }>()
);

export const deletePublisherSuccess = createAction(
  '[Publisher] Delete Publisher Success',
  props<{ id: number,deletedPublishers: PublisherResource }>()
);

export const deletePublisherFailure = createAction(
  '[Publisher] Delete Publisher Failure ',
  props<{ error: string }>()
);
