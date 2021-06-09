import {
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer
} from '@ngrx/store';
import { environment } from '../../environments/environment';
import { PublisherState } from './interfaces/publisher.interface';
import { publisherReducer } from './reducers/publisher.reducer';

export const reducers: ActionReducerMap<PublisherState> = {
  publisherState: publisherReducer,

};


export const metaReducers: MetaReducer<PublisherState>[] = !environment.production ? [] : [];
