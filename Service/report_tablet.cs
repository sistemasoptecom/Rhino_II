using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rhino.Service
{
    public class report_tablet
    {
        private int id;
        private int item;
        private string cod_articulo;
        private string estado;
        private string placa;
        private string imei;
        private string usuario;
        private string cedula;
        private string descripcion;
        private string linea;
        private string linea_activa;
        private string nuevo_imei;
        private string factura;
        private string valor;
        private string centro_costo;
        private string observacion;
        private string tipo;
        private int descuento;
        private string motivo_desc;
        private string forma_desc;
        private DateTime causacion;
        private DateTime fecha_estado;
        private string observacion_entrega;

        public string Cod_articulo{ get => cod_articulo; set => cod_articulo = value; }
        public string Estado { get => estado; set => estado = value; }
        public string Placa { get => placa; set => placa = value; }
        public string Usuario { get => usuario; set => usuario = value; }
        public string Cedula { get => cedula; set => cedula = value; }
        public string Imei { get => imei; set => imei = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public string Linea { get => linea; set => linea = value; }
        public string Linea_activa { get => linea_activa; set => linea_activa = value; }
        public string Nuevo_imei { get => nuevo_imei; set => nuevo_imei = value; }
        public string Factura { get => factura; set => factura = value; }
        public string Valor { get => valor; set => valor = value; }
        public string Centro_costo { get => centro_costo; set => centro_costo = value; }
        public string Observacion { get => observacion; set => observacion = value; }
        public string Observacion_entrega { get => observacion_entrega; set => observacion_entrega = value; }
        public string Motivo_desc { get => motivo_desc; set => motivo_desc = value; }
        public string Forma_desc { get => forma_desc; set => forma_desc = value; }
        public DateTime Causacion { get => causacion; set => causacion = value; }
        public DateTime Fecha_estado { get => fecha_estado; set => fecha_estado = value; }

        public int Id { get => id; set => id = value; }
        public int Item { get => item; set => item = value; }
        public int Descuento { get => descuento; set => descuento = value; }
        public string Tipo { get => tipo; set => tipo = value; }
    }
}