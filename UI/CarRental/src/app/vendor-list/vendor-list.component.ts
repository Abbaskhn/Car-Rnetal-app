import { Component, OnInit } from '@angular/core';
import { VendorService } from '../vendor.service';
import { Vendor } from '../vendor';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-vendor-list',
  standalone: true,
  imports: [],
  templateUrl: './vendor-list.component.html',
  styleUrl: './vendor-list.component.css'
})
export class VendorListComponent implements OnInit {
  vindor: Vendor[] = [];
  isAdmin: boolean = false;
  isVendor: boolean = false;
  isCustomer: boolean = false;
  constructor(private router: Router, private vendorService: VendorService,private authService:AuthService ) {}

  ngOnInit(): void {
    this.GetAll(); 
  }

  GetAll(): void {
    this.vendorService.getData()
      .subscribe(vendors => this.vindor = vendors);
  }
  setUserRoles(): void {
    const userRole = this.authService.getCurrentUserRole(); // Method to get the user's role
    this.isAdmin = userRole === 'Admin';
    this.isVendor = userRole === 'Vendor';
    this.isCustomer = userRole === 'Customer';
  }
  delete(id: number): void {
    if (this.isAdmin) {
    this.vendorService.deleteData(id).subscribe(
      response => {
        this.GetAll(); // Refresh the list after deletion
      },
      error => {
        console.error('Error deleting vendor:', error); // Log error for debugging
      }
    );
   
  }else {
    alert('You do not have permission to delete customers.');
  }
}}