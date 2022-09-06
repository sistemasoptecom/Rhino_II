using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rhino.Service
{
    public class report_devoluciones
    {

        private int id;
        private string usuario;
        private DateTime fecha;
        private string hora;
        private string empresa;

        public int Id { get => id; set => id = value; }
        public string Usuario { get => usuario; set => usuario = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public string Hora { get => hora; set => hora = value; }
        public string Empresa { get => empresa; set => empresa = value; }
    }
}