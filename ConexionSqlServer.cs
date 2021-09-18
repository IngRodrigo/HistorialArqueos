using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorialArqueos
{
    public class ConexionSqlServer
    {

        public static string connectionString;

        string servidor, usuario, password;
        public ConexionSqlServer(string ipServidor, string usuario, string password)
        {

                this.servidor =ipServidor;
                this.usuario = usuario;
                this.password = password;

                connectionString = @"Data Source=" + servidor + ";Persist Security Info=True;User ID=" + usuario + ";Password=" + password + ";";
        }

        /// <summary>
        /// Ejecutar una consulta SQL en la BD.
        /// </summary>
        /// <param name="SentenciaSql"></param>
        /// <returns></returns>
        public DataTable EjecutarConsultaSql(string SentenciaSql)
        {
            try
            {
                using (var conexionSql = new SqlConnection(connectionString))
                {
                    conexionSql.Open();
                    var comando = new SqlCommand(SentenciaSql, conexionSql);
                    comando.CommandType = CommandType.Text;
                    comando.CommandTimeout = 60;   // expresado en segundos.
                    var ds = new DataSet();
                    var da = new SqlDataAdapter();
                    da.SelectCommand = comando;
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Ejecutar un comando Insert, Update o Delete en la BD.
        /// </summary>
        /// <param name="SentenciaSql"></param>
        public void EjecutarComandoSql(string SentenciaSql)
        {
            try
            {
                using (var conexionSql = new SqlConnection(connectionString))
                {
                    conexionSql.Open();
                    var comando = new SqlCommand(SentenciaSql, conexionSql);
                    comando.CommandType = CommandType.Text;
                    comando.CommandTimeout = 60;   // expresado en segundos.
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /* public void EjecutarComandoSql(string SentenciaSql, SqlConnection conexionSql, SqlTransaction trans)
         {
             try
             {
                 var comando = new SqlCommand(SentenciaSql, conexionSql);
                 comando.CommandType = CommandType.Text;
                 comando.CommandTimeout = 60;   // expresado en segundos.
                 comando.Transaction = trans;
                 comando.ExecuteNonQuery();
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
        */
    }
}
