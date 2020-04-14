import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Sexo } from '../_models/Sexo';

@Injectable({
  providedIn: 'root'
})
export class SexoService {
  url= "http://localhost:5000/v1/sexos";

constructor( private http: HttpClient) { }
  getSexos(): Observable<Sexo[]>{
    return this.http.get<Sexo[]>(this.url);
  }
}
