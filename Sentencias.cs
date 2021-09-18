using HistorialArqueos.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorialArqueos
{
    class Sentencias
    {
        public static string listarSucursales()
        {
            string sql = "select * from CentralGBK.dbo.sucursal order by id_sucursal asc;";
            return sql;
        }

        public static string traerHistorialArqueo(string fecha)
        {
            

            string sql = "select (VentaEfectivo+DepositoBancario+VentaTarjetas+VentasVales+moDolares+moReales+moArgentino+moColones)as VentaTotal , *  from (select DISTINCT a.fecha_arqueo, a.usuario_ingreso as 'usuario_arqueo', 1 as 'migrado',a.id_estado, a.fecha_modificacion, case when efectivo.VentaEfectivo is null then 0 else efectivo.VentaEfectivo end as 'VentaEfectivo',  case when bancario.DepositoBancario is null then 0 else bancario.DepositoBancario end as 'DepositoBancario', case when tarjeta.VentaTarjetas is null then 0 else tarjeta.VentaTarjetas end as 'VentaTarjetas', case when vales.VentasVales is null then 0 else vales.VentasVales end as 'VentasVales', case when dolares.moDolares is null then 0 else dolares.moDolares end as 'moDolares', case when reales.moReales is null then 0 else reales.moReales end as 'moReales', case when argentino.moArgentino is null then 0 else argentino.moArgentino end as 'moArgentino',  0 AS 'moColones' from gestionBK.dbo.arqueo  as a  left JOIN ( select ArqueoId, sum(Cantidad) as VentaEfectivo from (select distinct  ArqueoId, v.GrupoValorId, gv.GrupoValorDescripcion, sum(Cantidad) as cantidad from gestionBK.dbo.ArqueoDetalle as ad  INNER JOIN gestionBK.dbo.Valor as v on ad.ValorId=v.ValorId INNER JOIN gestionBK.dbo.GrupoValor as gv on v.GrupoValorId=gv.GrupoValorId and v.GrupoValorId in (2) and Cantidad>0 GROUP BY ArqueoDetalleId, ArqueoId,  v.GrupoValorId, gv.GrupoValorDescripcion) as a GROUP BY ArqueoId ) as efectivo on a.id_arqueo=efectivo.ArqueoId left JOIN ( select ArqueoId, sum(Cantidad) as DepositoBancario from (select distinct  ArqueoId, v.GrupoValorId, gv.GrupoValorDescripcion, sum(Cantidad) as cantidad from gestionBK.dbo.ArqueoDetalle as ad  INNER JOIN gestionBK.dbo.Valor as v on ad.ValorId=v.ValorId INNER JOIN gestionBK.dbo.GrupoValor as gv on v.GrupoValorId=gv.GrupoValorId and v.GrupoValorId in (6) and Cantidad>0 GROUP BY ArqueoDetalleId, ArqueoId,  v.GrupoValorId, gv.GrupoValorDescripcion) as a GROUP BY ArqueoId ) as bancario on a.id_arqueo=bancario.ArqueoId   left JOIN ( select ArqueoId, sum(Cantidad) as VentaTarjetas from (select distinct  ArqueoId, v.GrupoValorId, gv.GrupoValorDescripcion, sum(Cantidad) as cantidad from gestionBK.dbo.ArqueoDetalle as ad  INNER JOIN gestionBK.dbo.Valor as v on ad.ValorId=v.ValorId INNER JOIN gestionBK.dbo.GrupoValor as gv on v.GrupoValorId=gv.GrupoValorId and v.GrupoValorId in (5) and Cantidad>0 GROUP BY ArqueoDetalleId, ArqueoId,  v.GrupoValorId, gv.GrupoValorDescripcion) as a GROUP BY ArqueoId ) as tarjeta on a.id_arqueo=tarjeta.ArqueoId  left JOIN ( select ArqueoId, sum(Cantidad) as VentasVales from (select distinct  ArqueoId, v.GrupoValorId, gv.GrupoValorDescripcion, sum(Cantidad) as cantidad from gestionBK.dbo.ArqueoDetalle as ad  INNER JOIN gestionBK.dbo.Valor as v on ad.ValorId=v.ValorId INNER JOIN gestionBK.dbo.GrupoValor as gv on v.GrupoValorId=gv.GrupoValorId and v.GrupoValorId in (7,8) and Cantidad>0 GROUP BY ArqueoDetalleId, ArqueoId,  v.GrupoValorId, gv.GrupoValorDescripcion) as a GROUP BY ArqueoId ) as vales on a.id_arqueo=vales.ArqueoId  left JOIN ( select ArqueoId, sum(Cantidad) as moDolares from (select distinct  ArqueoId, v.GrupoValorId, gv.GrupoValorDescripcion, sum(Cantidad) as cantidad from gestionBK.dbo.ArqueoDetalle as ad  INNER JOIN gestionBK.dbo.Valor as v on ad.ValorId=v.ValorId INNER JOIN gestionBK.dbo.GrupoValor as gv on v.GrupoValorId=gv.GrupoValorId and v.ValorId in (28) and Cantidad>0 GROUP BY ArqueoDetalleId, ArqueoId,  v.GrupoValorId, gv.GrupoValorDescripcion) as a GROUP BY ArqueoId ) as dolares on a.id_arqueo=dolares.ArqueoId  left JOIN ( select ArqueoId, sum(Cantidad) as moReales from (select distinct  ArqueoId, v.GrupoValorId, gv.GrupoValorDescripcion, sum(Cantidad) as cantidad from gestionBK.dbo.ArqueoDetalle as ad  INNER JOIN gestionBK.dbo.Valor as v on ad.ValorId=v.ValorId INNER JOIN gestionBK.dbo.GrupoValor as gv on v.GrupoValorId=gv.GrupoValorId and v.ValorId in (29) and Cantidad>0 GROUP BY ArqueoDetalleId, ArqueoId,  v.GrupoValorId, gv.GrupoValorDescripcion) as a GROUP BY ArqueoId ) as reales on a.id_arqueo=reales.ArqueoId  left JOIN ( select ArqueoId, sum(Cantidad) as moArgentino from (select distinct  ArqueoId, v.GrupoValorId, gv.GrupoValorDescripcion, sum(Cantidad) as cantidad from gestionBK.dbo.ArqueoDetalle as ad  INNER JOIN gestionBK.dbo.Valor as v on ad.ValorId=v.ValorId INNER JOIN gestionBK.dbo.GrupoValor as gv on v.GrupoValorId=gv.GrupoValorId and v.ValorId in (30) and Cantidad>0 GROUP BY ArqueoDetalleId, ArqueoId,  v.GrupoValorId, gv.GrupoValorDescripcion) as a GROUP BY ArqueoId ) as argentino on a.id_arqueo=argentino.ArqueoId   where cast(fecha_arqueo as DATE) ='"+fecha+"') as a";
            return sql;
            
        }

        public static string comprobarExisteArqueo(String idLocal, string fecha, string usuario, double ventaTotal)
        {
            string sql = "SELECT * FROM CentralGBK.[dbo].[HistorialArqueos] where id_local="+idLocal+" and usuario_arqueo='"+usuario+"' " +
                "and  CAST(fecha_arqueo as DATE)='" + fecha+ "' or CAST(fecha_modificacion as DATE)='"+fecha+"' and VentaTotal="+ventaTotal+";";
            return sql;
        }
        public static string InsertarArqueo(Arqueo item)
        {
            string sql = "insert into CentralGBK.dbo.HistorialArqueos " +
                "(id_local, fecha_arqueo, usuario_arqueo, migrado, estado, " +
                "fecha_modificacion, VentaTotal, VentaEfectivo, DepositoBancario, " +
                "VentaTarjetas, VentaVales, moDolares, moReal, moArgentino, moColones) " +
                "values " +
                "('"+item.id_local+"', " +
                "'" + Convert.ToDateTime(item.fechaArqueo).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                "'" + item.usuarioArqueo + "', " +
                "'" + item.migrado + "', " +
                "'" + item.estado + "', " +
                "'" + Convert.ToDateTime(item.fechaModificacion).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                item.ventaTotal + ", " +
                item.ventaEfectivo + ", " +
                item.DepositoBancario + ", " +
                item.ventasTarjetas + ", " +
                item.ventasVales + ", " +
                item.moDolares + ", " +
                item.moReales + ", " +
                item.moArgentino + ", " +
                item.moColones + ");";
             return sql;
        }
    }
}
