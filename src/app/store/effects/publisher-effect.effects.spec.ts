import { TestBed } from '@angular/core/testing';
import { provideMockActions } from '@ngrx/effects/testing';
import { Observable } from 'rxjs';

import { PublisherEffects } from './publisher.effects';

describe('PublisherEffects', () => {
  let actions$: Observable<any>;
  let effects: PublisherEffects;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        PublisherEffects,
        provideMockActions(() => actions$)
      ]
    });

    effects = TestBed.inject(PublisherEffects);
  });

  it('should be created', () => {
    expect(effects).toBeTruthy();
  });
});
