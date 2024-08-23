import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Car } from './car';
import { environment } from '../environment';
import { AppResponseModel, AppResponseModelExt } from './auth.service';



@Injectable({
  providedIn: 'root'
})
export class CarService {
  private baseUrl =  environment.apiUrl+'/api/Car';
 
  constructor(private http: HttpClient) {}

  createCar(carData: Car): Observable<AppResponseModel<Car>> {
    return this.http.post<AppResponseModel<Car>>(this.baseUrl+'/Add', carData);
  }

  getCars(): Observable<AppResponseModelExt<Car[]>> {
    return this.http.get<AppResponseModelExt<Car[]>>(this.baseUrl+'/GetAll');
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