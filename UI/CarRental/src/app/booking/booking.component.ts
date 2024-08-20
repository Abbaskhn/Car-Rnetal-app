import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { DateAdapter, MatNativeDateModule } from '@angular/material/core';
import { Booking } from '../Booking';
import { CarbookingService } from '../carbooking.service';
import { MaterialModule } from '../MaterailModule/MaterialModule';
import { NgIf } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { CheackoutComponent } from '../cheackout/cheackout.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [MaterialModule,FormsModule,ReactiveFormsModule,NgIf,    MatDatepickerModule,
    MatNativeDateModule],
  templateUrl: './booking.component.html',
  styleUrl: './booking.component.css',
 
})
export class BookingComponent {
  bookingForm: FormGroup;
  loading = false;
  error: string | null = null;

  // Date properties
  minDate: Date;
  maxDate: Date;

  constructor(
    private fb: FormBuilder,
    private bookingService: CarbookingService,
    private dateAdapter: DateAdapter<any>,
    private dialog: MatDialog
  ) {
    // Initialize date properties
    this.minDate = new Date();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() + 1); // Set max date to one year from now

    // Initialize form group
    this.bookingForm = this.fb.group({
   
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      address: ['', Validators.required],
      startDate: [null, Validators.required],
      endDate: [null, Validators.required],
      totalAmount: [0, [Validators.required, Validators.min(0)]], // Ensure to validate amount
    });
    this.dateAdapter.setLocale('en-US'); // Set locale if needed
  }

  onSubmit() {
    if (this.bookingForm.valid) {
      this.loading = true;
      this.error = null;
      const booking: Booking = this.bookingForm.value;

      this.bookingService.createBooking(booking).subscribe({
        next: (response: any) => {
          console.log('Booking successful', response);
          this.loading = false;

          // Open CheackoutComponent dialog
          this.dialog.open(CheackoutComponent, {
            width: '400px',
            data: {
              booking: response // Pass booking data to the checkout component
            }
          });
        },
        error: (err: any) => {
          console.error('Booking failed', err);
          this.error = 'Booking failed. Please try again later.';
          this.loading = false;
        },
      });
    }
  }
}