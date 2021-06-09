import { createReducer, on, Action } from '@ngrx/store';
import { loadAuthors, loadAuthorsSuccess, loadAuthorsFailure } from '../actions/author.actions';
import { initialAuthorState,AuthorState } from './../states';


export function AuthorReducer(_state: AuthorState | undefined, _action: Action) {
  return createReducer(
    initialAuthorState,
    on(loadAuthors, (state) => ({
          ...state,
          isLoading: true,
      })
    ),
    on(loadAuthorsFailure, (state, action) => (
      {
        ...state,
        isLoading: false,
        isLoaded: false,
        errorMessage: action.error,
      })),
  
    on(loadAuthorsSuccess, (state, action) => ({
      ...state,
      authors: action.authors,
      isLoading: false,
      isLoaded: true,
      totalResults: action.authors.length
    }))
  )(_state, _action);
  }