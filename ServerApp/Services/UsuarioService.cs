using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ServerApp.DAO;
using ServerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services
{


    public interface IUsuarioService
    {
        int Add(Usuario usuario);
        List<Usuario> GetAllUsuarios(int filtroAtivo, string filtroNome);
        Usuario GetUsuario(int idUsuario);
        int EditUsuario(Usuario usuario);
    }


    public class UsuarioService : IUsuarioService
    {
        IConfiguration _configuration;
        public UsuarioService(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").
                GetSection("DefaultConnection").Value;
            return connection;
        }

        public int Add(Usuario usuario)
        {
            var connectionString = this.GetConnection();
            int count = 0;
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "insert into Usuarios(Nome, Email, DataNascimento, Password, IdSexo) values (@Nome, @Email, @DataNascimento, @Password, @IdSexo); SELECT CAST(SCOPE_IDENTITY() as INT); ";
                    count = con.Execute(query, usuario);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return count;
            }
        }

        public int EditUsuario(Usuario usuario)
        {
            var connectionString = this.GetConnection();
            var count = 0;
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    var n = usuario.Ativo ? "true" : "false";
                    con.Open();
                    var query = "UPDATE Usuarios SET Nome = @Nome, Email = @Email, Password = @Password, IdSexo = @IdSexo, Ativo = '"+n+"'  WHERE Id = " + usuario.Id;
                    count = con.Execute(query, usuario);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return count;
            }
        }
        public List<Usuario> GetAllUsuarios(int filtroAtivo, string filtroNome)
        {
            var connectionString = this.GetConnection();
            List<Usuario> usuarios = new List<Usuario>();
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
     
                    
                    var query = "select u.Id, u.Nome, U.Email, U.DataNascimento, U.Password, U.Ativo, S.Descricao as Sexo , s.Id as IdSexo from Usuarios as u inner join Sexos S on u.IdSexo = S.Id";

                    if (filtroAtivo > 0)
                    {
                        var f = filtroAtivo == 1 ? "true" : "false";
                        query = "select u.Id, u.Nome, U.Email, u.DataNascimento, u.Password, u.Ativo, S.Descricao as Sexo, s.Id as IdSexo from Usuarios as u inner join Sexos S on u.IdSexo = S.Id where u.Ativo = '" + f + "';";
                    }

                    if (!string.IsNullOrEmpty(filtroNome))
                    {
                        query = "select u.Id, u.Nome, U.Email, u.DataNascimento, u.Password, u.Ativo, S.Descricao as Sexo, s.Id as IdSexo from Usuarios as u inner join Sexos S on u.IdSexo = S.Id where u.Nome like '%" + filtroNome.Trim() + "%';";
                    }

                    if (filtroAtivo > 0 && !string.IsNullOrEmpty(filtroNome))
                    {
                         var f = filtroAtivo == 1 ? "true" : "false";
                        query = "select u.Id, u.Nome, U.Email, u.DataNascimento, u.Password, u.Ativo, S.Descricao as Sexo, s.Id as IdSexo from Usuarios as u inner join Sexos S on u.IdSexo = S.Id where u.Ativo = '" + f + "' and u.Nome like '%" + filtroNome.Trim() + "%'";
                    }


                    con.Open();

                    usuarios = con.Query<Usuario>(query).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return usuarios;
            }
        }

        public Usuario GetUsuario(int idUsuario)
        {
            var connectionString = this.GetConnection();
            Usuario usuario = new Usuario();
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "select u.Id, u.Nome, U.Email, U.DataNascimento, U.Password, U.Ativo, S.Descricao as Sexo , s.Id as IdSexo from Usuarios as u inner join Sexos S on u.IdSexo = S.Id where u.Id = " + idUsuario;
                    System.Console.WriteLine(query);
                    usuario = con.Query<Usuario>(query).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return usuario;
            }
        }
    }
}