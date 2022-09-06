using Rhino.Sql;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web.Script.Serialization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Rhino.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehaviorAttribute(IncludeExceptionDetailInFaults = true)]
    public class Archivo : IArchivo
    {


        JavaScriptSerializer jss = new JavaScriptSerializer();

        rhinoEntities conn = new rhinoEntities();

        #region USUARIO        
    
        public string Auth(string json)
        {
            usuario usuario = new usuario();
            List<object> arr = new List<object>();
            try
            {
                dynamic data = jss.DeserializeObject(json);
                if (conn.usuario.Count() == 0)
                {
                    usuario au = new usuario();
                    au.username = "ADMIN";
                    au.codigo = "11111111";
                    au.nombre_usuario = "SYSTEM ADMINISTRATOR";
                    au.password="ADMIN";
                    conn.usuario.Add(au);
                    conn.SaveChanges();
                }
                string username = "";
                string password = "";
                username = data["username"];
                password = encriptar(data["password"]);
                int consecutivo = Convert.ToInt32(data["consecutivo"]);


                usuario = conn.usuario.Where(a => a.username.Equals(username) && a.password.Equals(password)).FirstOrDefault();

                int r = conn.consecutivos.Where(a => a.consecutivo_bd == consecutivo).Count();
                if (usuario == null)
                {
                    usuario = new usuario();
                }
                arr.Add(usuario);
                arr.Add(r);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            
            
            return jss.Serialize(arr);
        }
        #endregion
      
       

        public string encriptar(string text)
        {
            string enc = text.Trim().ToUpper();
            int lon = enc.Length;
            string j = "";
            int k = 0;
            string res = "";

            if (lon > 0)
            {
                for (int i = 0; i < lon; i++)
                {
                    j = enc.Substring(i, 1);
                    k = ((((Encoding.ASCII.GetBytes(j)[0]) + i) * lon) % 255);
                    res += "" + (char)k;
                }
            }

            return res;
        }
        
        public class combo
        {
            public string key { set; get; }
            public string values { set; get; }
        }

    }
}
