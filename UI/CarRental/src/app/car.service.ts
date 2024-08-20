import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Car } from './car';

@Injectable({
  providedIn: 'root'
})
export class CarService {
  private apiUrl="https://localhost:7066/api/Car";
  constructor(private http: HttpClient) {}

  getCars(): Observable<Car[]> {
    return this.http.get<Car[]>(this.apiUrl);
  }

  getCarById(id: number): Observable<Car> {
    return this.http.get<Car>(`${this.apiUrl}/${id}`);
  }

  createCar(car: Car, imageFile: File): Observable<Car> {
    const formData: FormData = new FormData();
    formData.append('carName', car.carName);
    formData.append('model', car.model);
    formData.append('rentalprice', car.rentalprice.toString());
    formData.append('imageFile', imageFile);

    return this.http.post<Car>(this.apiUrl, formData);
  }

  updateCar(id: number, car: Car, imageFile: File): Observable<Car> {
    const formData: FormData = new FormData();
    formData.append('carName', car.carName);
    formData.append('model', car.model);
    formData.append('rentalprice', car.rentalprice.toString());
    if (imageFile) {
      formData.append('imageFile', imageFile);
    }

    return this.http.put<Car>(`${this.apiUrl}/${id}`, formData);
  }

  deleteCar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}