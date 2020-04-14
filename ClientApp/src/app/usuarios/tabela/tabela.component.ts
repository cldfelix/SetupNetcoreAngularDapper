import { Component, OnInit, TemplateRef } from "@angular/core";
import { UsuariosService } from "src/app/_service/usuarios.service";
import { Usuario } from "src/app/_models/Usuario";
import { BsModalService, BsModalRef } from "ngx-bootstrap/modal";
import { Sexo } from "src/app/_models/Sexo";
import { SexoService } from "src/app/_service/sexo.service";
import * as alertify from 'alertifyjs';



import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder,
} from "@angular/forms";
import { IfStmt } from '@angular/compiler';

@Component({
  selector: "app-tabela",
  templateUrl: "./tabela.component.html",
  styleUrls: ["./tabela.component.css"],
})
export class TabelaComponent implements OnInit {
  usuarios: Usuario[];
  sexos: Sexo[];
  usuariosFiltrados: Usuario[];
  usuario: Usuario;
  filtroUsuario: string;
  _filtroNomeUsuario: string;
  modalRef: BsModalRef;
  formUsuario: FormGroup;
  formUsuarioUpdate: FormGroup;

  constructor(
    private service: UsuariosService,
    private modalService: BsModalService,
    private serviceSexo: SexoService,
    private fb: FormBuilder,
  ) {}

  openModal(temp: TemplateRef<any>, usuario: Usuario =null) {

    if(usuario != null){
      this.formUsuarioUpdate.patchValue(usuario);
      this.usuario = usuario;
    }
    this.modalRef = this.modalService.show(temp);
  }

  closeModal(temp: TemplateRef<any>) {
    this.modalService.hide(1);
  }

  ngOnInit() {
    this.getUsuarios();
    this.getSexos();
    this.validarUsuario();
    this.validarUsuarioUpdate();
  }

  get filtroNomeUsuario(): string {
    return this._filtroNomeUsuario;
  }

  set filtroNomeUsuario(v: string) {
    this._filtroNomeUsuario = v;
    this.usuariosFiltrados = this.filtroNomeUsuario
      ? this.filtrarUsuarios(this.filtroNomeUsuario)
      : this.usuarios;
  }

  filtrarUsuarios(f: string): any {
    var filtrarPor = f.toLocaleLowerCase();
    return this.usuarios.filter((x) => {
      return x.nome.toLocaleLowerCase().indexOf(filtrarPor) !== -1;
    });
  }

  getUsuarios() {
    this.service.getUsuarios().subscribe(
      (users: Usuario[]) => {
        this.usuarios = users;
        this.usuariosFiltrados = users;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  getSexos() {
    this.serviceSexo.getSexos().subscribe(
      (s: Sexo[]) => {
        this.sexos = s;
      },
      (err) => {
        console.log(err);
      }
    );
  }

  getUsuariosById(id: number) {
    this.service.getUsuariosById(id).subscribe(
      (user: Usuario) => {
        this.usuario = user;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  validarUsuario() {
    this.formUsuario = this.fb.group({
      nome: ["", [Validators.required, Validators.minLength(3)]],
      email: ["", [Validators.required, Validators.email]],
      password: ["", Validators.required],
      dataNascimento: ["", Validators.required],
      idSexo: ["", Validators.required],
    });
  }

  validarUsuarioUpdate() {
    this.formUsuarioUpdate = this.fb.group({
      id: ["", [Validators.required]],
      nome: ["", [Validators.required, Validators.minLength(3)]],
      email: ["", [Validators.required, Validators.email]],
      password: ["", Validators.required],
      ativo: ["", Validators.required],
      dataNascimento: ["", Validators.required],
      idSexo: ["", Validators.required],
    });
  }

  createUsuario(temp: TemplateRef<any>) {
    if (this.formUsuario.valid) {
      this.usuario = Object.assign({}, this.formUsuario.value);

      this.service.createUsuario(this.usuario).subscribe(
        (ret: number) => {
          this.getUsuarios();
          this.modalService.hide(1);
        },
        (error) => {
          if(error.status == 200){
            this.getUsuarios();
            this.modalService.hide(1);
            alertify.success('Usuario criado com sucesso!');
          }
          else{
            alertify.error('Erro interno!');

          }
        }
      );
    }
  }

  updateUsuario() {

    if (this.formUsuarioUpdate.valid) {
      this.usuario = Object.assign({}, this.formUsuarioUpdate.value);
      var id = this.usuario.idSexo;
      console.log(id);
      this.service.updateUsuario(this.usuario).subscribe(
        (ret: number) => {
          this.getUsuarios();
          this.modalService.hide(1);
        },
        (error) => {
          if(error.status == 200){
            this.getUsuarios();
            this.modalService.hide(1);
            alertify.success('Usuario atualizado com sucesso!');
          }
          else{
            alertify.error('Erro interno!');

          }
        }
      );
    }
  }
}
