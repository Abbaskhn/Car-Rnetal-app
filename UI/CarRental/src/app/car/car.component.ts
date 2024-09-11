import { Component, OnInit } from '@angular/core';
import { CarService } from '../car.service';
import { CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AppResponseModelExt, AuthService } from '../auth.service';
import { MatDialog } from '@angular/material/dialog';
import { Car } from '../car';
import { ImageViewerComponent } from '../components/imageViewer.component';

@Component({
  selector: 'app-car',
  standalone: true,
  imports: [NgFor, RouterLink, CurrencyPipe, NgIf, ImageViewerComponent],
  templateUrl: './car.component.html',
  styleUrl: './car.component.css'
})export class CarComponent implements OnInit {
  cars: Car[] = [];

  constructor(
    private carService: CarService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadCars();
  }

  // Method to check if the current user has a specific role
  hasRole(role: string): boolean {
    return this.authService.hasUserRole(role);
  }

  loadCars(): void {
    this.carService.getCars().subscribe(
      (res: AppResponseModelExt<Car[]>) => {
        this.cars = res.data.$values;
      },
      error => {
        console.error('Error loading cars:', error);
      }
    );
  }

  getImageData(car: Car): string | null {
    // Ensure the carFiles and carAppFiles exist and are properly formatted
    if (car.carFiles?.$values[0]?.carAppFiles?.data) {
      return car.carFiles.$values[0].carAppFiles.data;
    }
    return null;  // Return null if no valid image data is found
  }

  addNewCar(): void {
    if (this.hasRole('Admin')) {
      this.router.navigate(['/createcar']);
    } else {
      alert('Access denied. Admins only.');
    }
  }

  manageSuppliers(): void {
    if (this.hasRole('Admin')) {
      this.router.navigate(['/manage-suppliers']);
    } else {
      alert('Access denied. Admins only.');
    }
  }

  addCarListing(): void {
    if (this.hasRole('Vendor')) {
      this.router.navigate(['/createcar']);
    } else {
      alert('Access denied. Vendors only.');
    }
  }

  bookCar(carId: number): void {
    if (this.hasRole('Customer')) {
      this.router.navigate(['/book', carId]);
    } else {
      alert('Access denied. Customers only.');
    }
  }

  editCar(id: number): void {
    if (this.hasRole('Admin') || this.hasRole('Vendor')) {
      this.router.navigate(['/createcar', id]); // Adjust the route as needed
    } else {
      alert('Access denied. Admins and Vendors only.');
    }
  }

  deleteCar(carId: number): void {
    if (this.hasRole('Admin') || this.hasRole('Vendor')) {
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
