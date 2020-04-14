import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Usuario } from '../_models/Usuario';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UsuariosService {
  url = "http://localhost:5000/v1/usuarios"

constructor(private http: HttpClient) { }

  getUsuarios(filtroNumber: number =0, filtroNome: string = ""): Observable<Usuario[]> {
    return this.http.get<Usuario[]>(`${this.url}/?filtroAtivo=${filtroNumber}&filtroNome=${filtroNome}`);
  }

  createUsuario(usuario: Usuario): Observable<number> {
    return this.http.post<number>(this.url, usuario)
  }

  getUsuariosById(id: number): Observable<Usuario>{
    return this.http.get<Usuario>(`${this.url}/${id}`)
  }

  updateUsuario(usuario: Usuario): Observable<number>{
    return this.http.put<number>(this.url, usuario)
  }
}
