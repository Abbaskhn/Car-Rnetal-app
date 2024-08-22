import { Component, OnInit } from '@angular/core';

import { CarService } from '../car.service';
import { CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../auth.service';
import { MatDialog } from '@angular/material/dialog';
import { Car } from '../car';

@Component({
  selector: 'app-car',
  standalone: true,
  imports: [NgFor,RouterLink,CurrencyPipe,NgIf],
  templateUrl: './car.component.html',
  styleUrl: './car.component.css'
})
export class CarComponent implements OnInit {
  cars: Car[] = [];
  isAdmin: boolean = false;
  isVendor: boolean = false;
  isCustomer: boolean = false;


  constructor(
    private carService: CarService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.checkRole();
    this.loadCars();
  }

  checkRole(): void {
    this.isAdmin = this.authService.isAdmin();
    this.isVendor = this.authService.isVendor();
    this.isCustomer = this.authService.isCustomer();
  }

  loadCars(): void {
    this.carService.getCars().subscribe((data: Car[]) => {
      console.log('Received car data:', data); // For debugging
      this.cars = data;
    }, error => {
      console.error('Error loading cars:', error); // For debugging
    });
  }

  addNewCar(): void {
    if (this.isAdmin) {
      this.router.navigate(['/createcar']);
    } else {
      alert('Access denied. Admins only.');
    }
  }

  manageSuppliers(): void {
    if (this.isAdmin) {
      this.router.navigate(['/manage-suppliers']);
    } else {
      alert('Access denied. Admins only.');
    }
  }

  addCarListing(): void {
    if (this.isVendor) {
      this.router.navigate(['/createcar']);
    } else {
      alert('Access denied. Vendors only.');
    }
  }

  bookCar(carId: number): void {
    if (this.isCustomer) {
      this.router.navigate(['/book', carId]);
    } else {
      alert('Access denied. Customers only.');
    }
  }

  editCar(id: number): void {
    if (this.isAdmin || this.isVendor) {
      this.router.navigate(['/createcar', id]); // Adjust the route as needed
    } else {
      alert('Access denied. Admins and Vendors only.');
    }
  }

  deleteCar(carId: number): void {
    if (this.isAdmin || this.isVendor) {
      if (confirm('Are you sure you want to delete this car?')) {
        this.carService.deleteCar(carId).subscribe(() => {
          this.loadCars();
        });
      }
    } else {
      alert('Access denied. Admins and Vendors only.');
    }
  }
}