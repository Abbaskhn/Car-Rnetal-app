import { Component, OnInit } from '@angular/core';

import { Router } from '@angular/router';
import { CarbookingService } from '../carbooking.service';
import { AuthService } from '../auth.service';
import { BookingCarDto } from '../Booking';

@Component({
  selector: 'app-bookinglist',
  standalone: true,
  imports: [],
  templateUrl: './bookinglist.component.html',
  styleUrl: './bookinglist.component.css'
})
export class BookinglistComponentt implements OnInit {
  bookings: BookingCarDto[] = [];
  isAdmin: boolean = false;
  isVendor: boolean = false;
  isCustomer: boolean = false;

  constructor(
    private router: Router,
    private bookingService: CarbookingService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.getData();
    this.setUserRoles();
  }

  getData(): void {
    this.bookingService.getBookings().subscribe((data) => {
      this.bookings = data;
    });
  }

  setUserRoles(): void {
    const userRole = this.authService.getCurrentUserRole();
    this.isAdmin = userRole === 'Admin';
    this.isVendor = userRole === 'Vendor';
    this.isCustomer = userRole === 'Customer';
  }

  delete(id: number): void {
    if (this.isAdmin) {
      this.bookingService.deleteBooking(id).subscribe(() => {
        this.getData();
      });
    } else {
      alert('You do not have permission to delete bookings.');
    }
  }
}