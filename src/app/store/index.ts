import {
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer
} from '@ngrx/store';
import { environment } from '../../environments/environment';
import { IAppState } from './interfaces/publisher.interface';
import { AppReducer } from './reducers/publisher.reducer';

export const reducers: ActionReducerMap<IAppState> = {
  AppState: AppReducer,
};


export const metaReducers: MetaReducer<IAppState>[] = !environment.production ? [] : [];
