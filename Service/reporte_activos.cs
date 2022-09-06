using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rhino.Service
{
    public class reporte_activos
    {

        private string cod_articulo;
        private string estado;
        private string placa;
        private string serial;
        private string usuario;
        private string cedula;
        private string descripcion;
        private string factura;
        private string valor;
        private string centro_costo;
        private string observacion;
        private DateTime causacion;
        private DateTime fecha_estado;
        private string tipo;

        public string Cod_articulo { get => cod_articulo; set => cod_articulo = value; }
        public string Estado { get => estado; set => estado = value; }
        public string Placa { get => placa; set => placa = value; }

        public string Serial { get => serial; set => serial = value; }
        public string Usuario { get => usuario; set => usuario = value; }
        public string Cedula { get => cedula; set => cedula = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public string Factura { get => factura; set => factura = value; }
        public string Valor { get => valor; set => valor = value; }
        public string Centro_costo { get => centro_costo; set => centro_costo = value; }
        public string Observacion { get => observacion; set => observacion = value; }

        public string Tipo { get => tipo; set => tipo = value; }
        public DateTime Causacion { get => causacion; set => causacion = value; }
        public DateTime Fecha_estado { get => fecha_estado; set => fecha_estado = value; }
    }
}