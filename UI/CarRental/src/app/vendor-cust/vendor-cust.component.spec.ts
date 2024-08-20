import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VendorCustComponent } from './vendor-cust.component';

describe('VendorCustComponent', () => {
  let component: VendorCustComponent;
  let fixture: ComponentFixture<VendorCustComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VendorCustComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VendorCustComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
