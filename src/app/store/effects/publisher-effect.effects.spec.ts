import { TestBed } from '@angular/core/testing';
import { provideMockActions } from '@ngrx/effects/testing';
import { Observable } from 'rxjs';

import { PublisherEffectEffects } from './publisher-effect.effects';

describe('PublisherEffectEffects', () => {
  let actions$: Observable<any>;
  let effects: PublisherEffectEffects;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        PublisherEffectEffects,
        provideMockActions(() => actions$)
      ]
    });

    effects = TestBed.inject(PublisherEffectEffects);
  });

  it('should be created', () => {
    expect(effects).toBeTruthy();
  });
});
