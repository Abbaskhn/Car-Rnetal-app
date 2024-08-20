import { Component, OnInit } from '@angular/core';
import { Car } from '../car';
import { CarService } from '../car.service';
import { CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../auth.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-car',
  standalone: true,
  imports: [NgFor,RouterLink,CurrencyPipe,NgIf],
  templateUrl: './car.component.html',
  styleUrl: './car.component.css'
})
export class CarComponent implements OnInit {
  cars: Car[] = [];
  isAdmin: boolean = true;
  isVendor: boolean = false;
  isCustomer: boolean = false;

  constructor(
    private carService: CarService,
    private authService: AuthService,
    private router: Router,
   
  ) {}

  ngOnInit(): void {
    this.checkRole();
    this.loadCars();
  }
  checkRole(): void {
    this.isAdmin = this.authService.isAdmin();
    this.isVendor = this.authService.isVendor();
    this.isCustomer = this.authService.isCustomer();
    console.log('Admin:', this.isAdmin);
    console.log('Vendor:', this.isVendor);
    console.log('Customer:', this.isCustomer);
  }
  loadCars() {
    this.carService.getCars().subscribe((data) => {
      this.cars = data;
    });
  }

  deleteCar(id: number) {
    if (this.isAdmin || this.isVendor) {
      if (confirm('Are you sure you want to delete this car?')) {
        this.carService.deleteCar(id).subscribe(() => {
          this.loadCars();
        });
      }
    } else {
      alert('Access denied.');
    }
  }

  addNewCar() {
    if (this.isAdmin) {
      this.router.navigate(['/createcar']);
    } else {
      alert('Access denied. Admins only.');
    }
  }

  manageSuppliers() {
    if (this.isAdmin) {
      this.router.navigate(['/manage-suppliers']);
    } else {
      alert('Access denied. Admins only.');
    }
  }

  addCarListing() {
    if (this.isVendor) {
      this.router.navigate(['/createcar']);
    } else {
      alert('Access denied. Vendors only.');
    }
  }

  bookCar() {
    if (this.isCustomer) {
      this.router.navigate(['/book-car']);
    } else {
      alert('Access denied. Customers only.');
    }
  }
}