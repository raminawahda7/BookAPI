import * as fromPublisher from './publisher.actions';

describe('loadPublishers', () => {
  it('should return an action', () => {
    expect(fromPublisher.loadPublishers().type).toBe('[Publisher] Load Publishers');
  });
});
