using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorialArqueos.Modelos
{
    class Arqueo
    {
        public int id_local { get; set; }
        public string fechaArqueo { get; set; }
        public string usuarioArqueo { get; set; }
        public int migrado { get; set; }
        public int estado { get; set; }
        public string fechaModificacion { get; set; }
        public double ventaTotal { get; set; }

        public double ventaEfectivo { get; set; }
        public double DepositoBancario { get; set; }

        public double ventasTarjetas { get; set; }
        public double ventasVales { get; set; }

        public double moDolares { get; set; }

        public double moReales { get; set; }
        public double moArgentino { get; set; }
        public double moColones { get; set; }

        public int idArqueo { get; set; }
    }
}
