import { PublisherResource } from 'src/app/Resources/PublisherResource';
import { AuthorResourceCreated } from './../../Resources/AuthorResourceCreated';

export interface IApp {
  publishers:PublisherResource[],
  authors:AuthorResourceCreated[]
}

export interface IAppState {
  AppState: IApp;
}

export const initialAppState: IApp = {
  publishers:[],
  authors:[]
};
