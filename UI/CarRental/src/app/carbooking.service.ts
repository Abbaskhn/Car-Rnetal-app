import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { BookingCarDto } from './Booking';


@Injectable({
  providedIn: 'root'
})
export class CarbookingService {
  private baseUrl = "https://localhost:7066/api/BookingCar";
  constructor(private http: HttpClient) {}

  getBookings(): Observable<BookingCarDto[]> {
    return this.http.get<BookingCarDto[]>(this.baseUrl).pipe(
      catchError(this.handleError)
    );
  }

  getBookingById(id: number): Observable<BookingCarDto> {
    return this.http.get<BookingCarDto>(`${this.baseUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  createBooking(booking: BookingCarDto): Observable<BookingCarDto> {
    return this.http.post<BookingCarDto>(this.baseUrl, booking).pipe(
      catchError(this.handleError)
    );
  }

  updateBooking(id: number, booking: BookingCarDto): Observable<BookingCarDto> {
    return this.http.put<BookingCarDto>(`${this.baseUrl}/${id}`, booking).pipe(
      catchError(this.handleError)
    );
  }

  deleteBooking(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  checkCarAvailability(carId: number, startDate: Date, endDate: Date): Observable<any> {
    const params = new HttpParams()
      .set('carId', carId.toString())
      .set('startDate', startDate.toISOString())
      .set('endDate', endDate.toISOString());
  
    return this.http.get<any>(`${this.baseUrl}/check-availability`, { params });
  }
  private handleError(error: any) {
    console.error('An error occurred', error);
    return throwError('Something bad happened; please try again later.');
  }
}