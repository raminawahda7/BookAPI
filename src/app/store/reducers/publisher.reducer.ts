import { createReducer, on, Action } from '@ngrx/store';
import { loadPublishers, loadPublishersSuccess } from '../actions/publisher.actions';
import { Publisher, initialPublisherState } from '../interfaces/publisher.interface';

export const publisherFeatureKey = 'PublisherState';

export const reducer = createReducer(
  initialPublisherState as Publisher,
  on(loadPublishersSuccess, (state,action) => ({ ...state,publishers: action.publishers}))
);

export function publisherReducer(state: Publisher | undefined, action: Action) {
  return reducer(state as Publisher, action as Action);
}
