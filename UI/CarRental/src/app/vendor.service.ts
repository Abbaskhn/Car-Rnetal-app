import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Vendor } from './vendor';

@Injectable({
  providedIn: 'root'
})
export class VendorService {
 
  private apiUrl = "https://localhost:7066/api/Vendor"; // Replace with your API endpoint
  

  constructor(private http: HttpClient) { }
  getData(): Observable<any> {
    return this.http.get(this.apiUrl+"")
  }


  saveFormData(data: any): Observable<any> {
    // If you have a backend API, you can post the data to the server
    return this.http.post<any>(this.apiUrl, data);
  }
  
  updateVendor(id: number, data: Vendor): Observable<Vendor[]> {
    const url = `${this.apiUrl}/${id}`; // Construct the URL correctly
    return this.http.put<Vendor[]>(url, data); // Use Vendor instead of Vendor[]
  }
  
  

  // New DELETE method for deleting data
  deleteData(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`; // Include the id in the URL
    return this.http.delete(url); // Perform the DELETE request
  }
 
}

