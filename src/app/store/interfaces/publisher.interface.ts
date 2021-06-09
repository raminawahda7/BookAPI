import { PublisherResource } from 'src/app/Resources/PublisherResource';
import { AuthorResourceCreated } from './../../Resources/AuthorResourceCreated';

export interface Publisher {
  publishers:PublisherResource[],
}

export interface PublisherState {
  publisherState: Publisher,
}

export const initialPublisherState: Publisher = {
  publishers:[],
};

