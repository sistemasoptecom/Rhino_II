using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rhino.Service
{
    public class Report_compras
    {


        private int nro_pedido;
        private string fecha;
        private string ceco;
        private string usuario;
        private string valor;
        private string proveedor;
        private string estado;
        private string justificacion;
        private string nro_srp;
        private string nro_rp;
        private string nro_contrato;
        private string obs_compras;




        public int Nro_pedido { get => nro_pedido; set => nro_pedido = value; }
        public string Fecha { get => fecha; set => fecha = value; }

        public string Ceco { get => ceco; set => ceco = value; }

        public string Usuario { get => usuario; set => usuario = value; }

        public string Valor { get => valor; set => valor = value; }

        public string Proveedor { get => proveedor; set => proveedor = value; }

        public string Estado { get => estado; set => estado = value; }

        public string Justificacion { get => justificacion; set => justificacion = value; }

        public string Nro_srp { get => nro_srp; set => nro_srp = value; }

        public string Nro_rp { get => nro_rp; set => nro_rp = value; }
        public string Nro_contrato { get => nro_contrato; set => nro_contrato = value; }

        public string Obs_compras { get => obs_compras; set => obs_compras = value; }

    }
}