import { PublisherResource } from 'src/app/Resources/PublisherResource';
import { AuthorResourceCreated } from '../Resources/AuthorResourceCreated';

export interface Publisher {
  publishers:PublisherResource[],
}

export const initialPublisherState: Publisher = {
  publishers:[],
};

export interface PublisherState {
  publisherState: Publisher,
}


