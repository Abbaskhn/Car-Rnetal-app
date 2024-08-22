import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { DateAdapter, MatNativeDateModule } from '@angular/material/core';
import { BookingCarDto } from '../Booking';
import { CarbookingService } from '../carbooking.service';
import { MaterialModule } from '../MaterailModule/MaterialModule';
import { NgIf } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { CheackoutComponent } from '../cheackout/cheackout.component';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { BookingStatusDialogComponent } from '../booking-status-dialog/booking-status-dialog.component';
import { debounceTime } from 'rxjs';

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [MaterialModule,FormsModule,ReactiveFormsModule,NgIf,    MatDatepickerModule,
    MatNativeDateModule],
  templateUrl: './booking.component.html',
  styleUrl: './booking.component.css',
 
})
export class BookingComponent implements OnInit {    bookingForm: FormGroup;
  loading = false;
  error: string | null = null;

  minDate: Date;
  maxDate: Date;

  constructor(
    private fb: FormBuilder,
    private bookingService: CarbookingService,
    private dateAdapter: DateAdapter<any>,
    private dialog: MatDialog,
    private route: ActivatedRoute
  ) {
    this.minDate = new Date();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() + 1);

    this.bookingForm = this.fb.group({
      carId: [{ value: null, disabled: true }, Validators.required],
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      address: ['', Validators.required],
      startDate: [null, Validators.required],
      endDate: [null, Validators.required],
      totalAmount: [{ value: 0, disabled: true }, [Validators.required, Validators.min(0)]]
    });

    this.dateAdapter.setLocale('en-US');
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      const carId = +params['carId'];
      this.bookingForm.get('carId')?.setValue(carId);
    });

    this.bookingForm.valueChanges
      .pipe(debounceTime(300)) // Adding debounce time to limit the number of events
      .subscribe(() => {
        this.calculateTotalAmount();
      });
  }

  calculateTotalAmount() {
    const startDate = this.bookingForm.get('startDate')?.value;
    const endDate = this.bookingForm.get('endDate')?.value;

    if (startDate && endDate) {
      const days = (endDate - startDate) / (1000 * 60 * 60 * 24);
      const dailyRate = 1000; // Adjust this as needed
      this.bookingForm.get('totalAmount')?.setValue(days * dailyRate, { emitEvent: false });
    }
  }

  checkAvailabilityAndSubmit() {
    const carId = this.bookingForm.get('carId')?.value;
    const startDate = this.bookingForm.get('startDate')?.value;
    const endDate = this.bookingForm.get('endDate')?.value;

    if (carId && startDate && endDate) {
      this.bookingService.checkCarAvailability(carId, startDate, endDate).subscribe({
        next: (response: { available: boolean, message?: string }) => {
          if (response.available) {
            this.submitBooking();
          } else {
            this.dialog.open(BookingStatusDialogComponent, {
              width: '400px',
              data: { message: response.message || 'Car is already booked for the selected dates.' }
            });
          }
        },
        error: () => {
          this.error = 'Failed to check car availability.';
        }
      });
    }
  }

  submitBooking() {
    if (this.bookingForm.valid) {
      this.loading = true;
      this.error = null;

      const booking: BookingCarDto = this.bookingForm.getRawValue();

      this.bookingService.createBooking(booking).subscribe({
        next: (response: any) => {
          this.loading = false;
          this.bookingForm.disable();
          this.dialog.open(CheackoutComponent, {
            width: '400px',
            data: { booking: response }
          });
        },
        error: () => {
          this.loading = false;
          this.dialog.open(BookingStatusDialogComponent, {
            width: '400px',
            data: { message: 'An error occurred while creating the booking.' }
          });
        }
      });
    }
  }

  onSubmit() {
    this.checkAvailabilityAndSubmit();
  }
}