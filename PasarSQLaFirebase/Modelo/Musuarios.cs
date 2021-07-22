using System;
using System.Collections.Generic;
using System.Text;

namespace PasarSQLaFirebase.Modelo
{
    public class Musuarios
    {
        //en Firebase los ID´s se manejan como String y no como entero, por eso el ID será string
        public string Id_usuario { get; set; }
        public string Usuario { get; set; }
        public string Pass { get; set; }
        public string Icono { get; set; }
        public string Estado { get; set; }

    }
}
