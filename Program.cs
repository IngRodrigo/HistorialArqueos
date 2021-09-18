using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorialArqueos
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length>0)
            {
                string anho = args[0];
                string mes = args[1];
                string dia = args[2];
                ProcesarHistoriales.iniciar(anho, mes, dia);
            }
            else
            {
   
                ProcesarHistoriales.iniciar();
            }

        }
    }
}
