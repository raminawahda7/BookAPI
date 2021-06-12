import { createReducer, on, Action } from '@ngrx/store';
import * as lodash from 'lodash';
import {
  loadPublishersSuccess,
  loadPublishers,
  loadPublishersFailure,
  createPublisher,
  updatePublisherSuccess,
  createPublisherFailure,
  deletePublisherSuccess,
  deletePublisher,
  deletePublisherFailure
} from '../actions/publisher.actions';
import { PublisherState, initialPublisherState } from '../states';
import {
  createPublisherSuccess,
  updatePublisher,
} from './../actions/publisher.actions';

export function PublisherReducer(
  _state: PublisherState | undefined,
  _action: Action
) {
  return createReducer(
    initialPublisherState,
    on(loadPublishers, (state) => ({
      ...state,
      isLoading: true,
    })),
    on(loadPublishersFailure, (state, action) => ({
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
      totalResults: action.publishers.length,
    })),
    on(createPublisher, (state, action) => ({
      ...state,
      isLoading: true,
    })),
    on(createPublisherSuccess, (state, action) => {
      const createdPublisher = action.newPublisher;
      const publishersList = [...state.publishers];
      // const copyOfState = Object.assign([], state.publishers);
      console.log(
        'compare if original state and copy of state are equals: ',
        lodash.isEqual(publishersList, state.publishers)
      );
      publishersList.push(createdPublisher);
      return {
        ...state,
        isLoading: false,
        isLoaded: true,
        publishers: publishersList,
        totalResults: publishersList.length,
      };
    }),
    on(createPublisherFailure, (state, action) => ({
      ...state,
      isLoading: false,
      isLoaded: false,
    })),
    on(updatePublisher, (state, action) => ({
      ...state,
      isLoading: true,
    })),
    on(updatePublisherSuccess, (state, action) => {
      const updatedPublisher = action.updatedPublisher;
      const publishersList = [...state.publishers];
      const index = publishersList.findIndex(x=>x.id==updatedPublisher.id);
      publishersList[index]=updatedPublisher;
      return {
        ...state,
        isLoading: false,
        isLoaded: true,
        publishers: publishersList,
        totalResults: publishersList.length,
      };
    }),
    on(deletePublisher,(state,action)=>{
      return{
        ...state,
        isLoading: true,
      }
    }),
    on(deletePublisherSuccess,(state,action)=>{
      const index = state.publishers.findIndex(x=>x.id==action.id);
      return{
        ...state,
        publishers:state.publishers.slice(0,index)        ,
        isLoading: false,
        isLoaded: true,        
      }
    }),
    on(deletePublisherFailure,(state,action)=>{
      return{
        ...state,
        isLoading: false,
        isLoaded: false,        
      }
    })
  )(_state, _action);
}
