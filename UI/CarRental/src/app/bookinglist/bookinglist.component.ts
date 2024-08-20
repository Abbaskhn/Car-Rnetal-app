import { Component, OnInit } from '@angular/core';
import { Booking } from '../Booking';
import { Router } from '@angular/router';
import { CarbookingService } from '../carbooking.service';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-bookinglist',
  standalone: true,
  imports: [],
  templateUrl: './bookinglist.component.html',
  styleUrl: './bookinglist.component.css'
})
export class BookinglistComponentt implements OnInit{
  booking: Booking[] = [];
  isAdmin: boolean = false;
  isVendor: boolean = false;
  isCustomer: boolean = false;

  constructor(
    private router: Router, 
    private bookingService: CarbookingService,
    private authService: AuthService // Inject AuthService to manage roles
  ) {}

  ngOnInit(): void {
    this.GetData();
    this.setUserRoles(); // Set roles based on the authenticated user
  }

  GetData(): void {
    this.bookingService.getAllBookings().subscribe((data) => {
      this.booking = data;
    });
  }

  setUserRoles(): void {
    const userRole = this.authService.getCurrentUserRole(); // Method to get the user's role
    this.isAdmin = userRole === 'Admin';
    this.isVendor = userRole === 'Vendor';
    this.isCustomer = userRole === 'Customer';
  }

  delete(id: number): void {
    if (this.isAdmin) {
      this.bookingService.deleteBooking(id).subscribe(() => {
        this.GetData(); // Refresh the list after deletion
      });
    } else {
      alert('You do not have permission to delete bookings.');
    }
  }
}