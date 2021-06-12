import { PublisherResource } from 'src/app/Resources/PublisherResource';
import { AuthorResourceCreated } from '../Resources/AuthorResourceCreated';
import { Publisher } from './../models/Publisher';

export interface PublisherState {
  publishers:PublisherResource[],
}

export const initialPublisherState: PublisherState = {
  publishers:[],

};
// =====================================

export interface AuthorState {
  authors:AuthorResourceCreated[],
}

export const initialAuthorState: AuthorState = {
  authors:[],
};