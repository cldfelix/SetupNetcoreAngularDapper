using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;


namespace ServerApp.Services
{
   public class SqlServerServices 
    {
        private static SqlServerServices _defaultInstance;
        private readonly string _connectionString;

        public static void Init(string connectionString)
        {
            _defaultInstance = new SqlServerServices(connectionString);
        }

        public SqlServerServices(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static SqlServerServices DefaultInstance
        {
          get { return _defaultInstance; }
        }

        public static object Combine<T1, T2>(T1 t1, T2 t2)
        {
            var expando = new System.Dynamic.ExpandoObject();
            var merged = expando as IDictionary<string, object>;
            foreach (System.Reflection.PropertyInfo fi in typeof(T1).GetProperties())
            {
                merged[fi.Name] = fi.GetValue(t1, null);
            }
            foreach (System.Reflection.PropertyInfo fi in typeof(T2).GetProperties())
            {
                merged[fi.Name] = fi.GetValue(t2, null);
            }
            return merged;
        }


   

        public IList<T> ListWithParams<T>(string tableName, int? topLimit = null, string whereQuery = null, dynamic extraParams = null, string extraJoins = null, string extraSelect = null, string customOrderBy = null)
        {
            string selectPart;
            if (topLimit.HasValue)
                selectPart = "top("+topLimit.Value+") " + tableName + ".*";
            else
                selectPart = tableName + ".*";

            if (!String.IsNullOrWhiteSpace(extraSelect))
                selectPart += ", " + extraSelect;

            string joins = "";
            if (!String.IsNullOrWhiteSpace(extraJoins))
                joins = extraJoins;

            string query = @"select " + selectPart + " from " + tableName + " " + joins;

            if (!String.IsNullOrEmpty(whereQuery))
                query = query + " " + whereQuery;

            if (!String.IsNullOrEmpty(customOrderBy))
                query += " order by " + customOrderBy;

            var queryParams = new { };

            var finalParams = SqlServerServices.Combine(queryParams, extraParams != null ? extraParams : new { });
            return this.List<T>(query, (object)finalParams).ToList();
        }

        public IEnumerable<T> List<T>(string query, object args = null)
        {
          SqlConnection conexao = new SqlConnection(_connectionString);
          try 
          {
            
            conexao.Open();

            if (args != null)
            {
              return conexao.Query<T>(query, (object)args);
            }
            else
            {
              return conexao.Query<T>(query);
            }
          }
          finally
          {
            conexao.Close();
          }
        }

        public T FirstOrDefault<T>(string query, object args = null)
        {
          
          SqlConnection conexao = new SqlConnection(_connectionString);
          try 
          {
            
            conexao.Open();

            if (args != null)
            {
              return conexao.Query<T>(query, (object)args).FirstOrDefault();
            }
            else
            {
              return conexao.Query<T>(query).FirstOrDefault();
            }
          }
          finally
          {
            conexao.Close();
          }
        }

        public T FirstOrDefaultWithParams<T>(string tableName, string whereQuery = null, dynamic extraParams = null, string extraJoins = null, string extraSelect = null, string customOrderBy = null)
        {
            string selectPart = "top(1) " + tableName + ".*";

            if (!String.IsNullOrWhiteSpace(extraSelect))
                selectPart += ", " + extraSelect;

            string joins = "";
            if (!String.IsNullOrWhiteSpace(extraJoins))
                joins = extraJoins;

            string query = @"select " + selectPart + " from " + tableName + " " + joins;

            if (!String.IsNullOrEmpty(whereQuery))
                query = query + " " + whereQuery;

            if (!String.IsNullOrEmpty(customOrderBy))
                query += " order by " + customOrderBy;

            var queryParams = new { };

            var finalParams = SqlServerServices.Combine(queryParams, extraParams != null ? extraParams : new { });
            return this.FirstOrDefault<T>(query, (object)finalParams);
        }

        public T SingleOrDefault<T>(string query, object args = null)
        {
          SqlConnection conexao = new SqlConnection(_connectionString);
          try 
          {
            
            conexao.Open();

            if (args != null)
            {
              return conexao.Query<T>(query, (object)args).SingleOrDefault();
            }
            else
            {
              return conexao.Query<T>(query).SingleOrDefault();
            }
          }
          finally
          {
            conexao.Close();
          }
        }
        
        public T SingleOrDefaultWithParams<T>(string tableName, string whereQuery = null, dynamic extraParams = null, string extraJoins = null, string extraSelect = null, string customOrderBy = null)
        {
            string selectPart = "top(1) " + tableName + ".*";

            if (!String.IsNullOrWhiteSpace(extraSelect))
                selectPart += ", " + extraSelect;

            string joins = "";
            if (!String.IsNullOrWhiteSpace(extraJoins))
                joins = extraJoins;

            string query = @"select " + selectPart + " from " + tableName + " " + joins;

            if (!String.IsNullOrEmpty(whereQuery))
                query = query + " " + whereQuery;

            if (!String.IsNullOrEmpty(customOrderBy))
                query += " order by " + customOrderBy;

            var queryParams = new { };

            var finalParams = SqlServerServices.Combine(queryParams, extraParams != null ? extraParams : new { });
            return this.SingleOrDefault<T>(query, (object)finalParams);
        }
        
        public dynamic Insert(string query, object args)
        {
          SqlConnection conexao = new SqlConnection(_connectionString);
          try 
          {
            conexao.Open();
            return conexao.Query<dynamic>(query, (object)args).FirstOrDefault();
          }
          finally
          {
            conexao.Close();
          }
        }

        public int InsertWithIntId(string query, object args)
        {
          SqlConnection conexao = new SqlConnection(_connectionString);
          try 
          {
            conexao.Open();
            
            string finalQuery;
            if (query[query.Length - 1] != ';')
              finalQuery = query + "; SELECT CAST(SCOPE_IDENTITY() as int)";
            else
              finalQuery = query + "SELECT CAST(SCOPE_IDENTITY() as int)";
            
            return conexao.Query<int>(finalQuery, args).Single();
          }
          finally
          {
            conexao.Close();
          }
        }
        public dynamic Update(string query, object args)
        {
          SqlConnection conexao = new SqlConnection(_connectionString);
          try 
          {
            
            conexao.Open();

            return conexao.Query<dynamic>(query, (object)args).FirstOrDefault();
          }
          finally
          {
            conexao.Close();
          }
        }
    }
}