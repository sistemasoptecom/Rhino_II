using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rhino.Service
{
    public class reporte_bodegasst
    {
        private int id;
        private string codigo;
        private string marca;
        private string referencia;
        private string serial;
        private string lote;
        private int mes_fabricacion;
        private int ano_fabricacion;
        private DateTime f_compra;
        private string proveedor;
        private int tipo;
        private string observaciones;
        private int estado;
        private string ccosto;
        private string cedula_asig;
        private string estado_asig;
        private string empleado;
        private DateTime f_inspeccion;
        private string tipo_equipo;
        private DateTime f_prox_inspec;
        private DateTime f_seguimiento;
        private string res_inspeccion;

        public int Id { get => id; set => id = value; }
        public string Codigo { get => codigo; set => codigo = value; }
        public string Marca { get => marca; set => marca = value; }
        public string Referencia { get => referencia; set => referencia = value; }
        public string Serial { get => serial; set => serial = value; }
        public string Lote { get => lote; set => lote = value; }
        public int Mes_fabricacion { get => mes_fabricacion; set => mes_fabricacion = value; }
        public int Ano_fabricacion { get => ano_fabricacion; set => ano_fabricacion = value; }
        public DateTime F_compra { get => f_compra; set => f_compra = value; }
        public string Proveedor { get => proveedor; set => proveedor = value; }
        public int Tipo { get => tipo; set => tipo = value; }
        public string Observaciones { get => observaciones; set => observaciones = value; }
        public int Estado { get => estado; set => estado = value; }
        public string Cedula_asig { get => cedula_asig; set => cedula_asig = value; }
        public string Ccosto { get => ccosto; set => ccosto = value; }
        public string Estado_asig { get => estado_asig; set => estado_asig = value; }
        public string Empleado { get => empleado; set => empleado = value; }
        public DateTime F_inspeccion { get => f_inspeccion; set => f_inspeccion = value; }
        public string Tipo_equipo { get => tipo_equipo; set => tipo_equipo = value; }
        public DateTime F_prox_inspec { get => f_prox_inspec; set => f_prox_inspec = value; }
        public DateTime F_seguimiento { get => f_seguimiento; set => f_seguimiento = value; }

        public string Res_inspeccion { get => res_inspeccion; set => res_inspeccion = value; }
    }
}