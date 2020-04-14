using System.Collections.Generic;
using System.Linq;
using ServerApp.Models;
using ServerApp.Services;

namespace ServerApp.DAO
{
    internal static class UsuarioDAO
    {
        public static IList<T> GetAllUsuarios<T>(string filtro = null, bool filtroAtivo = true) where T : IUsuario
        {
            if(!filtroAtivo && !string.IsNullOrEmpty(filtro)){
                var sqlString = "WHERE Name LIKE @FiltroNome'%' AND Ativo = @Ativo inner join Sexo S on u.IdSexo = S.Id;";
                return SqlServerServices.DefaultInstance.ListWithParams<T>("Usuarios", null, sqlString, new { FiltroNome = filtro, Ativo = filtroAtivo });
            }

              if(!filtroAtivo){
                var sqlString = "WHERE Ativo = @Ativo inner join Sexo S on u.IdSexo = S.Id;";
                return SqlServerServices.DefaultInstance.ListWithParams<T>("Usuarios", null, sqlString, new { FiltroNome = filtroAtivo});
            }

              if(!string.IsNullOrEmpty(filtro)){
                var sqlString = "WHERE Name LIKE '%'@FiltroNome'%' inner join Sexo S on u.IdSexo = S.Id;";
                return SqlServerServices.DefaultInstance.ListWithParams<T>("Usuarios", null, sqlString, new { FiltroNome = filtro });
            }



            return SqlServerServices.DefaultInstance.List<T>( string.Format(@"select u.Id, u.Nome, U.Email, U.DataNascimento, U.Password, U.Ativo, S.Descricao from Usuario as u inner join Sexo S on u.IdSexo = S.Id;"), null).ToList();




            //return SqlServerServices.DefaultInstance.ListWithParams<T>("Usuarios", null, "WHERE PointsRequired <= @Points AND Ativo = @Ativo", new { Points = points });
        }
    }
}