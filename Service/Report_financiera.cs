using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rhino.Service
{
    public class Report_financiera
    {


        private int nro_pedido;
        private string fecha;
        private string ceco;
        private string usuario;
        private string valor;
        private string proveedor;
        private string estado;
        private string justificacion;




        public int Nro_pedido { get => nro_pedido; set => nro_pedido = value; }
        public string Fecha { get => fecha; set => fecha = value; }

        public string Ceco { get => ceco; set => ceco = value; }

        public string Usuario { get => usuario; set => usuario = value; }

        public string Valor { get => valor; set => valor = value; }

        public string Proveedor { get => proveedor; set => proveedor = value; }

        public string Estado { get => estado; set => estado = value; }

        public string Justificacion { get => justificacion; set => justificacion = value; }

    }
}