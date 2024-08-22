import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  private apiurl = "https://localhost:7066/api/Customer"
  constructor(private http: HttpClient) { }

  getData(): Observable<any> {
    return this.http.get(this.apiurl+"")
  }

  postData(data: any): Observable<any> {
    return this.http.post(this.apiurl, data)

  }
  updateData(id: number, data: any): Observable<any> {
    return this.http.put(this.apiurl,data)
  }
  deleteData(id: number): Observable<any> {
    const url = `${this.apiurl}/${id}`; // Include the id in the URL for DELETE request
    return this.http.delete(url);
  }
 

 
}
