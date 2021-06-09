import {
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer
} from '@ngrx/store';
import { environment } from '../../environments/environment';
import { PublisherState } from './interfaces/publisher.interface';
import { PublisherReducer } from './reducers/publisher.reducer';

export const reducers: ActionReducerMap<PublisherState> = {
  PublisherState: PublisherReducer,

};


export const metaReducers: MetaReducer<PublisherState>[] = !environment.production ? [] : [];
