using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HistorialArqueos.Modelos;
using System.IO;

namespace HistorialArqueos
{
    class ProcesarHistoriales
    {
        public static string logEventos;
        public static FileStream archivoLog;
        public static StreamWriter swLog;
        public static DataTable tablaSucursales = new DataTable();
        public static DataTable tablaArqueo = new DataTable();
        public static DataTable historalArqueosTabla = new DataTable();
        public static List<Sucursal> idLocalesLista = new List<Sucursal>();

        public static List<Arqueo> listaArqueos = new List<Arqueo>();


  

        public static void iniciar(string anho="", string mes="", string dia="")
        {

            string fecha = "";
            
            if (!anho.Equals("") && !mes.Equals("") && !dia.Equals(""))
            {
                Console.WriteLine("Se pasaron parametros: "+anho+"-"+mes+"-"+dia);
                fecha = anho + "-" + mes + "-" + dia;
            }
            else
            {
                fecha = DateTime.Now.ToString("yyyy-MM-dd");
            }
            
            IniciarLog();
            EscribirLog("Iniciando proceso.");
            
            //fecha = "2021-08-01";
            ConexionSqlServer conectarCentral = new ConexionSqlServer(Credenciales.servidorCentralGbk, Credenciales.usuarioCentralGbk, Credenciales.passwordCentralGbk);
          //  ConexionSqlServer conectarLocal= new ConexionSqlServer(Credenciales.servidorCentralGbk, Credenciales.usuarioCentralGbk, Credenciales.passwordCentralGbk);
            EscribirLog("SQL: " + Sentencias.listarSucursales());
            try
            {
                tablaSucursales = conectarCentral.EjecutarConsultaSql(Sentencias.listarSucursales());

                if (tablaSucursales != null)
                {
                    idLocalesLista = obtenerIdLocales(tablaSucursales);
                    if (idLocalesLista.Count > 0)
                    {
                        foreach (Sucursal item in idLocalesLista)
                        {
                            EscribirLog("**********Iniciando extracción de datos**************");
                            EscribirLog("ID: " + item.idSucursal + " Sucursal: " + item.nombreLocal);
                            listaArqueos = traerHistorial(item.idSucursal, item.ipServidor, fecha);
                            if (listaArqueos.Count > 0)
                            {
                                
     
                                    insertarHistorial(listaArqueos, item.idSucursal, fecha);

                            }
                            else
                            {
                                EscribirLog("No existen datos en esta fehca "+fecha);
                            }
                        }
                        
                    }
                    else
                    {
                        EscribirLog("Tabla sucursales vacia");
                    }
                }

            }
            catch (Exception e)
            {
                EscribirLog("Error al intentar obtener datos de los locales. " + e);
            }
            EscribirLog("Fin del proceso...");
        }

        private static bool comprobarExisteArqueo(String id, string fecha, string usuario, double ventaTotal)
        {
            ConexionSqlServer conectarCentral = new ConexionSqlServer(Credenciales.servidorCentralGbk, Credenciales.usuarioCentralGbk, Credenciales.passwordCentralGbk);
            EscribirLog("SQL: " + Sentencias.comprobarExisteArqueo(id, fecha, usuario, ventaTotal));
            historalArqueosTabla = conectarCentral.EjecutarConsultaSql(Sentencias.comprobarExisteArqueo(id, fecha, usuario, ventaTotal));
            bool respuesta = false;
            try
            {
                 if (historalArqueosTabla.Rows.Count > 0)
                {
                    respuesta = true;
                }
                
            }
            catch (Exception e)
            {

                EscribirLog("Error al intentar comprobar si ya existe el arqueo en la tabla HistorialArqueos " + e);
            }
            return respuesta;
        }

        private static void IniciarLog()
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                path = path + @"\Log\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                logEventos = path + @"\LogDev_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + ".txt";
                archivoLog = System.IO.File.Create(logEventos);
                swLog = new StreamWriter(archivoLog);
                EscribirLog("Procesar ventas ...");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void EscribirLog(string evento)
        {
            try
            {
                swLog.WriteLine(DateTime.Now + " " + evento);
                swLog.Flush();
                Console.WriteLine(evento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void insertarHistorial(List<Arqueo> listaArqueos, string id, string fecha)
        {
            EscribirLog("Cantidad de registros obtenidos: " + listaArqueos.Count);
            int contador = 0;
            foreach (Arqueo item in listaArqueos)
            {
                string sql = Sentencias.InsertarArqueo(item);
                try
                {
                    if (comprobarExisteArqueo(id, fecha, item.usuarioArqueo, item.ventaTotal))
                    {
                        EscribirLog("El arqueo ya existe en la tabla historial arqueos");
                    }
                    else
                    {
                        EscribirLog("SQL: " + sql);
                        ConexionSqlServer conectar = new ConexionSqlServer(Credenciales.servidorCentralGbk, Credenciales.usuarioCentralGbk, Credenciales.passwordCentralGbk);
                        Console.WriteLine(sql);
                        conectar.EjecutarComandoSql(sql);
                        EscribirLog("Se ejecuto correctamente la senrencia: " + sql);
                        contador++;
                    }

                }
                catch (Exception e)
                {
                  
                   EscribirLog("Error al intentar insertar: "+e);
                }
            }
            EscribirLog("Cantidad de registros insertados: " + contador);
        }

        private static List<Arqueo> traerHistorial(string idSucursal, string ip, string fecha)
        {
            List<Arqueo> miListaArqueo = new List<Arqueo>();
            try
            {

                Arqueo arqueo = null;
                ConexionSqlServer conectarLocal = new ConexionSqlServer(ip, Credenciales.Usuario, Credenciales.password);
                EscribirLog("SQL: " + Sentencias.traerHistorialArqueo(fecha));
                tablaArqueo = conectarLocal.EjecutarConsultaSql(Sentencias.traerHistorialArqueo(fecha));
                if (tablaArqueo!=null)
                {
                    
                    foreach (DataRow item in tablaArqueo.Rows)
                    {
                        arqueo = new Arqueo();
                        arqueo.id_local = System.Convert.ToInt32(idSucursal);
                        arqueo.fechaArqueo= item["fecha_arqueo"].ToString();
                        arqueo.usuarioArqueo = item["usuario_arqueo"].ToString();
                        arqueo.migrado = System.Convert.ToInt32(item["migrado"].ToString());
                        arqueo.estado = System.Convert.ToInt32(item["id_estado"].ToString());
                        arqueo.fechaModificacion = item["fecha_modificacion"].ToString();
                        arqueo.ventaEfectivo = System.Convert.ToDouble(item["VentaEfectivo"].ToString());
                        arqueo.DepositoBancario = item["DepositoBancario"].ToString()!=""?System.Convert.ToDouble(item["DepositoBancario"].ToString()):0;
                        arqueo.ventasTarjetas = System.Convert.ToDouble(item["VentaTarjetas"].ToString());
                        arqueo.ventasVales = System.Convert.ToDouble(item["VentasVales"].ToString());
                        arqueo.moDolares = System.Convert.ToDouble(item["moDolares"].ToString());
                        arqueo.moReales = System.Convert.ToDouble(item["moReales"].ToString());
                        arqueo.moArgentino = System.Convert.ToDouble(item["moArgentino"].ToString());
                        arqueo.moColones = System.Convert.ToDouble(item["moColones"].ToString());
                        arqueo.ventaTotal = System.Convert.ToDouble(item["VentaTotal"].ToString());
                        miListaArqueo.Add(arqueo);
                    }
                    
                }
                else
                {
                    EscribirLog("La consulta no genero resultados.");
                }
            }
            catch (Exception e)
            {
                EscribirLog("Error al intentar traer historiales del local "+idSucursal+" : "+e);
            }
            
            return miListaArqueo;
        }

        private static List<Sucursal> obtenerIdLocales(DataTable tablaSucursales)
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
            List<Sucursal> miLista = new List<Sucursal>();
            Sucursal sucursal = null;
            foreach (DataRow item in tablaSucursales.Rows)
            {
                sucursal = new Sucursal();
                sucursal.idSucursal = item["id_sucursal"].ToString();
                sucursal.nombreLocal = item["nombre_local"].ToString();
                sucursal.codigoMicros = item["codigo_micros"].ToString()!=""? System.Convert.ToInt32(item["codigo_micros"].ToString()):0;
                sucursal.ipServidor = item["ip_servidor"].ToString();
                miLista.Add(sucursal);
            }

            return miLista;
        }
    }
}
