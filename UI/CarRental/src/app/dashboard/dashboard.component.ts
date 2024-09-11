import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { VendorComponent } from "../vendor/vendor.component";
import { CustomerComponent } from "../customer/customer.component";
import { NgIf } from '@angular/common';
import { AuthService } from '../auth.service';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [NgIf, VendorComponent, CustomerComponent,RouterLinkActive,RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  userRole: string | null = null;

  constructor(private fb: FormBuilder, private authService: AuthService) {
  
   
  }

  ngOnInit(): void {
 
  }
 
 
}
