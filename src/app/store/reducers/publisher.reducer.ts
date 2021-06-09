import { createReducer, on, Action } from '@ngrx/store';
import { loadPublishersSuccess,loadPublishers,loadPublishersFailure, createPublisher } from '../actions/publisher.actions';
import { PublisherState, initialPublisherState } from '../states';
import { createPublisherSuccess } from './../actions/publisher.actions';


export function PublisherReducer(_state: PublisherState|undefined, _action: Action) {
  return createReducer(
    initialPublisherState,
    on(loadPublishers, (state) => ({
          ...state,
          isLoading: true,
      })
    ),
    on(loadPublishersFailure, (state, action) => (
      {
        ...state,
        isLoading: false,
        isLoaded: false,
        errorMessage: action.error,
      })),
  
    on(loadPublishersSuccess, (state, action) => ({
      ...state,
      publishers: action.publishers,
      isLoading: false,
      isLoaded: true,
      totalResults: action.publishers.length
    })),
    on(createPublisher,(state,action)=>({
      ...state,
      isLoaded:true
    }))
  )(_state, _action);
  }