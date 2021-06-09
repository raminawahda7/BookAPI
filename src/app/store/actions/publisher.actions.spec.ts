import * as fromPublisher from './publisher.actions';

describe('loadPublishers', () => {
  it('should return an action', () => {
    expect(fromPublisher.loadPublishers().type).toBe('[Publisher] Load Publishers');
  });
});

export const signupSuccess = createAction(
  USER_SIGNUP_SUCCESS,
  props<any>()
)