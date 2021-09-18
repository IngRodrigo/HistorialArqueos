using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorialArqueos
{
    class Credenciales
    {
        public static string FileToReadCenralGbk = @"C:\HistorialArqueos\CredencialesCentralGBK.txt";

        public static string[] linesC = File.ReadAllLines(FileToReadCenralGbk);
        public static string servidorCentralGbk= linesC[0];
        public static string usuarioCentralGbk = linesC[1];
        public static string passwordCentralGbk = linesC[2];

        //Credenciales bases sucursales
        public static string FileToRead = @"C:\HistorialArqueos\CredencialesLocales.txt";

        public static string[] lines = File.ReadAllLines(FileToRead);

        public static string Usuario = lines[0];
        public static string password = lines[1];


    }
}
