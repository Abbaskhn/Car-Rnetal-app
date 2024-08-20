import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StripeService {
  private apiUrl = 'https://localhost:7066/api/Payment/charge'; // Adjust this URL based on your API settings

  constructor(private http: HttpClient) { }

  // Adjust method to accept parameters separately
  charge(stripeToken: string, name: string, email: string, amount: number, description: string): Observable<any> {
    // Create a payload object
    const payload = {
      stripeToken,
      name,
      email,
      amount,
      description
    };

    return this.http.post(this.apiUrl, payload, {
      headers: new HttpHeaders({
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      })
    });
  }
}