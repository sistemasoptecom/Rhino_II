using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rhino.Service
{
    public class Excel
    {


        private string comp;
        private string cia;
        private string vigencia;
        private string mes;
        private string periodo;
        private string area;
        private string impu;
        private int valor;



        public string Comp { get => comp; set => comp = value; }
        public string Cia { get => cia; set => cia = value; }

        public string Vigencia { get => vigencia; set => vigencia = value; }

        public string Mes { get => mes; set => mes = value; }

        public string Periodo { get => periodo; set => periodo = value; }

        public string Area { get => area; set => area = value; }

        public string Impu { get => impu; set => impu = value; }

        public int Valor { get => valor; set => valor = value; }

    }
}