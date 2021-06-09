import { createReducer, on, Action } from '@ngrx/store';
import { loadPublishers, loadPublishersSuccess } from '../actions/publisher.actions';
import { IApp, initialAppState } from '../interfaces/publisher.interface';

export const publisherFeatureKey = 'AppState';

export const reducer = createReducer(
  initialAppState as IApp,
  on(loadPublishersSuccess, (state,action) => ({ ...state,publishers: action.publishers}))
);

export function AppReducer(state: IApp | undefined, action: Action) {
  return reducer(state as IApp, action as Action);
}
