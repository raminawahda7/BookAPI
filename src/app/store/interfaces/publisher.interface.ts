import { PublisherResource } from 'src/app/Resources/PublisherResource';

export interface IApp {
  publishers:PublisherResource[]
}

export interface IAppState {
  AppState: IApp;
}

export const initialAppState: IApp = {
  publishers:[]  
};
