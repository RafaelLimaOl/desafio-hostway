import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OperatorDash } from './operator-dash';

describe('OperatorDash', () => {
  let component: OperatorDash;
  let fixture: ComponentFixture<OperatorDash>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OperatorDash]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OperatorDash);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
