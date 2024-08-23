import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment';
import { AppResponseModel } from '../auth.service';



@Injectable({
  providedIn: 'root'
})
export class FileService {
  private baseUrl =  environment.apiUrl+'/api/File';
 
  constructor(private http: HttpClient) {}

  save(fileData: FormData): Observable<AppResponseModel<number>> {
    return this.http.post<AppResponseModel<number>>(this.baseUrl+'/upload', fileData);
  }
}