import * as fromAuthor from './author.actions';

describe('loadAuthors', () => {
  it('should return an action', () => {
    expect(fromAuthor.loadAuthors().type).toBe('[Author] Load Authors');
  });
});
