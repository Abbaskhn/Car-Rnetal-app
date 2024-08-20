import { Component } from '@angular/core';
import { Customer } from '../customer';
import { Router } from '@angular/router';
import { CustomerService } from '../customer.service';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-customerlist',
  standalone: true,
  imports: [],
  templateUrl: './customerlist.component.html',
  styleUrl: './customerlist.component.css'
})
export class CustomerlistComponent {
  constructor(private router: Router,  private customerService:CustomerService, private authService: AuthService ){}
  item: any;
  customer:Customer[]=[];
  isAdmin: boolean = false;
isVendor: boolean = false;
isCustomer: boolean = false;
  
  ngOnInit(): void {
    this.GetAll();
    this.setUserRoles() 
  }
  GetAll(): void {
    this.customerService.getData()
    .subscribe(heroes => this.customer = heroes);
  }
  delete(id: number): void {
    if (this.isAdmin) {
      this.customerService.deleteData(id).subscribe(() => {
        this.GetAll(); // Refresh the list after deletion
      });
    } else {
      alert('You do not have permission to delete customers.');
    }
  }

  setUserRoles(): void {
    const userRole = this.authService.getCurrentUserRole(); // Method to get the user's role
    this.isAdmin = userRole === 'Admin';
    this.isVendor = userRole === 'Vendor';
    this.isCustomer = userRole === 'Customer';
  }
}
