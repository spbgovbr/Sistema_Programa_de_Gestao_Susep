import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class StorageService {

    constructor(private http: HttpClient) {}

    uploadFile(url: string, file: File): Observable<Object> {        
        let formData = new FormData();
        formData.append('upload', file)
        return this.http.post(url, formData);
    }
    
}