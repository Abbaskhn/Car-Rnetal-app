import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Car } from './car';



@Injectable({
  providedIn: 'root'
})
export class CarService {
  private baseUrl = 'https://localhost:7066/api/Car'; // Adjust base URL as needed
 
  constructor(private http: HttpClient) {}

  createCar(carData: FormData): Observable<any> {
    return this.http.post<any>(this.baseUrl, carData);
  }

  getCars(): Observable<Car[]> {
    return this.http.get<Car[]>(this.baseUrl);
  }

  getCar(id: number): Observable<Car> {
    return this.http.get<any>(`${this.baseUrl}/${id}`);
  }

  
updateCar(id: number, carData: FormData): Observable<any> {
  return this.http.put<any>(`${this.baseUrl}/${id}`, carData);
}
  deleteCar(id: number): Observable<Car> {
    return this.http.delete<any>(`${this.baseUrl}/${id}`);
  }
}