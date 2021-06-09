import {
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer
} from '@ngrx/store';
import { environment } from '../../environments/environment';
import { PublisherReducer } from './reducers/publisher.reducer';
import { AuthorReducer } from './reducers/author.reducer';
import { AuthorState,PublisherState } from './states';

export interface State {
    publishers: PublisherState;
    authors: AuthorState;
}

export const reducers: ActionReducerMap<State> = {
  authors: AuthorReducer,
  publishers:PublisherReducer

};


