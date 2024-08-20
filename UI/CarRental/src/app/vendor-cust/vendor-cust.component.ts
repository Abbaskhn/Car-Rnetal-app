import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { VendorComponent } from "../vendor/vendor.component";
import { CustomerComponent } from "../customer/customer.component";
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-vendor-cust',
  standalone: true,
  imports: [NgIf,VendorComponent, CustomerComponent,ReactiveFormsModule,FormsModule],
  templateUrl: './vendor-cust.component.html',
  styleUrl: './vendor-cust.component.css'
})
export class VendorCustComponent 
  {
    selectedRole: string = '';
  roleForm: FormGroup; 
  vendor(){
    console.log("/app-vendor")
  }
  constructor(private fb: FormBuilder) {
    this.roleForm = this.fb.group({
      role: [''] // default value for the role selection
    });
  }
  
}
