using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rhino.Service
{
    public class Informe_ped_srp
    {
        private string id;
        private string nro_ped;
        private string nro_srp;
        private string nro_rp;
        private string nro_contrato;
        private int ceco;
        private string nombre_ceco;
        private string usuario;
        private string asignado;
        private double valor_srp;
        private string apro_finan;
        private DateTime fecha;
        private string estado;

        public string Id { get => id; set => id = value; }
        public string Nro_ped { get => nro_ped; set => nro_ped = value; }

        public string Nro_srp { get => nro_srp; set => nro_srp = value; }

        public string Nro_rp { get => nro_rp; set => nro_rp = value; }

        public string Nro_contrato { get => nro_contrato; set => nro_contrato = value; }
        public int CECO { get => ceco; set => ceco = value; }

        public string Nombre_ceco { get => nombre_ceco; set => nombre_ceco = value; }

        public string Usuario { get => usuario; set => usuario = value; }

        public string Asignado { get => asignado; set => asignado = value; }

        public Double Valor_srp { get => valor_srp; set => valor_srp = value; }

        public string Apro_finan { get => apro_finan; set => apro_finan = value; }

        public DateTime Fecha { get => fecha; set => fecha = value; }

        public string Estado { get => estado; set => estado = value; }
    }
}