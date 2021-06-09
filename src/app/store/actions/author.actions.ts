import { createAction, props } from '@ngrx/store';
import { AuthorResourceCreated } from './../../Resources/AuthorResourceCreated';

export const loadAuthors = createAction(
  '[Author] Load Authors'
);

export const loadAuthorsSuccess = createAction(
  '[Author] Load Authors Success',
  props<{ authors: AuthorResourceCreated [] }>()
);

export const loadAuthorsFailure = createAction(
  '[Author] Load Authors Failure',
  props<{ error: any }>()
);
