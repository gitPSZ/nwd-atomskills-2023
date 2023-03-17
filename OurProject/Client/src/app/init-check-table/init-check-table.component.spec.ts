import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitCheckTableComponent } from './init-check-table.component';

describe('InitCheckTableComponent', () => {
  let component: InitCheckTableComponent;
  let fixture: ComponentFixture<InitCheckTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitCheckTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InitCheckTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
