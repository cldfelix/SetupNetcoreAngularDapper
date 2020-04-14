create table Sexos
(
    Id        int identity
        constraint Sexo_pk
            primary key nonclustered,
    Descricao nvarchar(100) not null
)
go


create table Usuarios
(
    Id             int identity
        constraint Usuario_pk
            primary key nonclustered,
    Nome           nvarchar(100) not null,
    Email          nvarchar(100) not null,
    DataNascimento date          not null,
    Password       nvarchar(100) not null,
    Ativo          bit default 1 not null,
    IdSexo         int           not null
        constraint Usuarios_Sexos_Id_fk
            references Sexos
)
go

create unique index Usuarios_Id_uindex
    on Usuarios (Id)
go


insert into Sexos(Descricao) values ('Masculino'),('Feminino'),('Outros')
                                                           
