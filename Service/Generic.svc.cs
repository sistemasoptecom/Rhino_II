using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Script.Serialization;
using GemBox.Spreadsheet;
using System.Data.SqlClient;
using System.ServiceModel.Activation;
using System.Data;
using Rhino.Sql;
using System.IO;
using SpreadsheetLight;
using System.Data.Entity.Validation;

namespace Rhino.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehaviorAttribute(IncludeExceptionDetailInFaults = true)]
    public class Generic : IGeneric
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        SqlConnection conexion;
        rhinoEntities entiti = new rhinoEntities();
        public string buscador(string json)
        {
            string stringCon = System.Configuration.ConfigurationManager.ConnectionStrings["rhinoEntities"].ConnectionString;
            string[] vec = stringCon.Split(';');
            string preStrCon = vec[2].Substring(28) + ";" + vec[3] + ";" + vec[4] + ";" + vec[5] + ";" + vec[6];
            conexion = new SqlConnection(preStrCon);
            conexion.Open();
            dynamic data = jss.DeserializeObject(json);
            string sqls = data["sqls"];
            SqlCommand sql = new SqlCommand(sqls, conexion);
            SqlDataReader read = sql.ExecuteReader();
            string str = WriteReaderToJSON(read);
            conexion.Close();
            return str;
        }
        private string WriteReaderToJSON(IDataReader reader)
        {
            StringBuilder sb = new StringBuilder();

            if (reader == null || reader.FieldCount == 0)
            {
                sb.Append("null");
                return "";
            }

            int rowCount = 0;

            sb.Append("[");

            while (reader.Read())
            {
                sb.Append("{");
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    sb.Append("\"" + reader.GetName(i) + "\":");
                    sb.Append("\"" + reader[i] + "\"");
                    sb.Append(i == (reader.FieldCount - 1) ? "" : ",");
                }
                sb.Append("},");
                rowCount++;
            }
            if (rowCount > 0)
            {
                int index = sb.ToString().LastIndexOf(",");
                sb.Remove(index, 1);
            }
            sb.Append("]");
            return sb.ToString();
        }

        public string SaveImg(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string dfile = data["file"];
            string name_ = data["name"];
            string name = name_.ToString();
            string ruta = data["ruta"];

            string file = dfile.Split(',')[1];
            string ext = (dfile.Split(';')[0]).Split('/')[1];
            byte[] toDecodeByte = Convert.FromBase64String(file);

            string filename = DateTime.Now.ToString("ddMMyyHHmmss");

            if (name.Trim().Length > 0)
            {
                filename = name;
            }


            if (!File.Exists("C:\\inetpub\\wwwroot\\plantilla5\\" + ruta + "\\" + filename + "." + ext))
            {
                var fs = new BinaryWriter(new FileStream(@"C:\inetpub\wwwroot\plantilla5\" + ruta + "\\" + filename + "." + ext, FileMode.Append, FileAccess.Write));
                fs.Write(toDecodeByte);
                fs.Close();
            }

            return filename + "." + ext;
        }

        public string CargarAuto(string json)
        {
            try
            {
                jss.MaxJsonLength = 999999999;
                dynamic data = jss.DeserializeObject(json);
                string name = data["name"];
                string path = @"h:/root/home/sistemasop-001/www/siscoint/rhinowcf/multimediaRepository/" + name + ".xlsx";
                string usu = data["usu"];
                DateTime fecha = DateTime.Today;
                int rs = 0;
                List<object> arr = new List<object>();
                List<object> arr2 = new List<object>();
                List<Excel> arr1 = new List<Excel>();
                List<Excel> arr12 = new List<Excel>();
                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                string estado = "PENDIENTE";
                ExcelFile workbook =  ExcelFile.Load(path);
                var sb = new StringBuilder();

                planillas_nomina pn = new planillas_nomina();

                // Iterate through all worksheets in an Excel workbook.
                foreach (var worksheet in workbook.Worksheets)
                {
                    sb.AppendLine();
                    sb.AppendFormat("{0} {1} {0}", new string('-', 25), worksheet.Name);

                    // Iterate through all rows in an Excel worksheet.
                    int counter = 0;

                    foreach (var row in worksheet.Rows)
                    {
                        if(counter >= 1) { 
                            Excel e = new Excel();
                            sb.AppendLine();
                            e.Comp = row.AllocatedCells[0].StringValue;
                            e.Cia = row.AllocatedCells[1].StringValue;
                            e.Vigencia = row.AllocatedCells[2].StringValue;
                            e.Mes = row.AllocatedCells[3].StringValue;
                            e.Periodo = row.AllocatedCells[4].StringValue;
                            e.Area = row.AllocatedCells[5].StringValue;
                            e.Impu = row.AllocatedCells[6].StringValue;
                            e.Valor = Convert.ToInt32(row.AllocatedCells[7].StringValue);
                            arr.Add(e);

                            presupuesto p = entiti.presupuesto.Where(z => z.ccosto.Equals(e.Area) && z.cuenta.Equals(e.Impu) && z.ano.Equals("2020")).FirstOrDefault();

                            if( p != null){
                                if(p.disponibilidad < e.Valor)
                                {
                                    estado = "RETENIDO";
                                }
                            }else{
                                estado = "RETENIDO";
                            }


                        }

                        counter = counter + 1;
                    }
                    

                    planillas_nomina reg = entiti.planillas_nomina.OrderByDescending(y => y.consecutivo).FirstOrDefault();
                    if (reg != null)
                    {
                        pn.id = reg.consecutivo + 1;
                        pn.consecutivo = reg.consecutivo + 1;
                    }
                    else
                    {
                        pn.consecutivo = 1;
                        pn.id = 1;
                    }
                    pn.log_user = usu;
                    pn.estado = estado;
                    pn.ruta = path;
                    pn.fecha = fecha;


                }

                entiti.planillas_nomina.Add(pn);
                int rs2 = entiti.SaveChanges();

                return "[" + jss.Serialize(arr) + "," + jss.Serialize(estado) +","+ jss.Serialize(pn.consecutivo) + "]";
            }
            #region catch
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize("?" + err);
            }

            catch (Exception e)
            {

                string ret = "";
                if (e.InnerException != null)
                {
                    if (e.InnerException.Message != null)
                    {
                        ret = (e.InnerException.Message);
                        if (e.InnerException.InnerException != null)
                        {
                            ret += e.InnerException.InnerException.Message;
                        }
                    }
                }
                else
                {
                    ret = (e.Message);
                }
                return jss.Serialize("?" + ret);
            }
            #endregion
        }

        public string SaveFile2(string json)
        {
            try
            {
                jss.MaxJsonLength = 999999999;
                dynamic data = jss.DeserializeObject(json);
                string dfile = data["file"];
                string name = data["name"];

                string file = dfile.Split(',')[1];
                string ext = (dfile.Split(';')[0]).Split('/')[1];
                byte[] toDecodeByte = Convert.FromBase64String(file);

                string filename = name + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
                if (ext.Equals("vnd.openxmlformats-officedocument.wordprocessingml.document"))
                {
                    ext = "docx";
                }
                if (ext.Equals("vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
                {
                    ext = "xlsx";
                }

                if (ext.Equals("pdf"))
                {

                    ext = "pdf";      //
                }

                if (!Directory.Exists("h:/root/home/sistemasop-001/www/siscoint/rhinowcf/multimediaRepository\\"))
                {
                    Directory.CreateDirectory("h:/root/home/sistemasop-001/www/siscoint/rhinowcf/multimediaRepository\\");
                }

                if (!File.Exists("h:/root/home/sistemasop-001/www/siscoint/rhinowcf/multimediaRepository/" + filename + "." + ext))
                {
                    var fs = new BinaryWriter(new FileStream(@"h:/root/home/sistemasop-001/www/siscoint/rhinowcf/multimediaRepository/" + name + "." + ext, FileMode.Append, FileAccess.Write));
                    fs.Write(toDecodeByte);
                    fs.Close();
                }
                else
                {
                    File.Delete("h:\root/home/sistemasop-001/www/siscoint\rhinowcf/multimediaRepository\\" + filename + "." + ext);
                    var fs = new BinaryWriter(new FileStream(@"h:\root\home\sistemasop-001\www\siscoint\rhinowcf\multimediarepository\" + filename + "." + ext, FileMode.Append, FileAccess.Write));
                    fs.Write(toDecodeByte);
                    fs.Close();
                }

                return jss.Serialize(name + "." + ext);

            }
            #region catch
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize("?" + err);
            }

            catch (Exception e)
            {

                string ret = "";
                if (e.InnerException != null)
                {
                    if (e.InnerException.Message != null)
                    {
                        ret = (e.InnerException.Message);
                        if (e.InnerException.InnerException != null)
                        {
                            ret += e.InnerException.InnerException.Message;
                        }
                    }
                }
                else
                {
                    ret = (e.Message);
                }
                return jss.Serialize("?" + ret);
            }
            #endregion

        }

        public string Read_planilla(string json)
        {
            try
            {
                jss.MaxJsonLength = 999999999;
                dynamic data = jss.DeserializeObject(json);
                string ruta = data["ruta"];
                DateTime fecha = DateTime.Today;
                List<object> arr = new List<object>();
                List<object> arr2 = new List<object>();
                List<Excel> arr1 = new List<Excel>();
                List<Excel> arr12 = new List<Excel>();
                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                ExcelFile workbook = ExcelFile.Load(ruta);
                var sb = new StringBuilder();

                // Iterate through all worksheets in an Excel workbook.
                foreach (var worksheet in workbook.Worksheets)
                {
                    sb.AppendLine();
                    sb.AppendFormat("{0} {1} {0}", new string('-', 25), worksheet.Name);

                    // Iterate through all rows in an Excel worksheet.
                    int counter = 0;

                    foreach (var row in worksheet.Rows)
                    {
                        if (counter >= 1)
                        {
                            Excel e = new Excel();
                            sb.AppendLine();
                            e.Comp = row.AllocatedCells[0].StringValue;
                            e.Cia = row.AllocatedCells[1].StringValue;
                            e.Vigencia = row.AllocatedCells[2].StringValue;
                            e.Mes = row.AllocatedCells[3].StringValue;
                            e.Periodo = row.AllocatedCells[4].StringValue;
                            e.Area = row.AllocatedCells[5].StringValue;
                            e.Impu = row.AllocatedCells[6].StringValue;
                            e.Valor = Convert.ToInt32(row.AllocatedCells[7].StringValue);
                            arr.Add(e);

                        }

                        counter = counter + 1;
                    }



                }

                return jss.Serialize(arr);
            }
            #region catch
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize("?" + err);
            }

            catch (Exception e)
            {

                string ret = "";
                if (e.InnerException != null)
                {
                    if (e.InnerException.Message != null)
                    {
                        ret = (e.InnerException.Message);
                        if (e.InnerException.InnerException != null)
                        {
                            ret += e.InnerException.InnerException.Message;
                        }
                    }
                }
                else
                {
                    ret = (e.Message);
                }
                return jss.Serialize("?" + ret);
            }
            #endregion
        }


        public string Aprobar_planilla(string json)
        {
            try
            {
                jss.MaxJsonLength = 999999999;
                dynamic data = jss.DeserializeObject(json);
                string ruta = data["ruta"];
                int consecutivo = data["consecutivo"];
                DateTime fecha = DateTime.Today;
                string anio = fecha.Year.ToString();
                List<object> arr = new List<object>();
                List<object> arr2 = new List<object>();
                List<Excel> arr1 = new List<Excel>();
                List<Excel> arr12 = new List<Excel>();
                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                string estado = "APROBADO";
                ExcelFile workbook = ExcelFile.Load(ruta);
                var sb = new StringBuilder();

                // Iterate through all worksheets in an Excel workbook.
                foreach (var worksheet in workbook.Worksheets)
                {
                    sb.AppendLine();
                    sb.AppendFormat("{0} {1} {0}", new string('-', 25), worksheet.Name);

                    // Iterate through all rows in an Excel worksheet.
                    int counter = 0;

                    foreach (var row in worksheet.Rows)
                    {
                        if (counter >= 1)
                        {
                            Excel e = new Excel();
                            sb.AppendLine();
                            e.Comp = row.AllocatedCells[0].StringValue;
                            e.Cia = row.AllocatedCells[1].StringValue;
                            e.Vigencia = row.AllocatedCells[2].StringValue;
                            e.Mes = row.AllocatedCells[3].StringValue;
                            e.Periodo = row.AllocatedCells[4].StringValue;
                            e.Area = row.AllocatedCells[5].StringValue;
                            e.Impu = row.AllocatedCells[6].StringValue;
                            e.Valor = Convert.ToInt32(row.AllocatedCells[7].StringValue);
                           

                            presupuesto p = entiti.presupuesto.Where(z => z.ccosto.Equals(e.Area) && z.cuenta.Equals(e.Impu) && z.ano.Equals(anio)).FirstOrDefault();

                            if (p != null)
                            {
                                if(p.disponibilidad - e.Valor >= 0){
                                    p.disponibilidad = (Convert.ToDouble(p.disponibilidad - e.Valor));
                                    p.total_ejecucion = (Convert.ToDouble(p.total_ejecucion + e.Valor));
                                    estado = "APROBADO";
                                    entiti.Entry(p).State = System.Data.EntityState.Modified;
                                }else{
                                    estado = "RETENIDO";
                                    arr.Add(e);
                                }
                            }else{
                                estado = "RETENIDO";
                                arr.Add(e);
                            }
                        }

                        counter = counter + 1;
                    }
                }
                if (estado.Equals("APROBADO"))
                {
                    planillas_nomina pn = entiti.planillas_nomina.Where(z => z.consecutivo == consecutivo).FirstOrDefault();
                    pn.estado = estado;
                    int rs2 = entiti.SaveChanges();
                }
                
                return "[" + jss.Serialize(arr) + "," + jss.Serialize(estado) + "]";
            }
            #region catch
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize("?" + err);
            }

            catch (Exception e)
            {

                string ret = "";
                if (e.InnerException != null)
                {
                    if (e.InnerException.Message != null)
                    {
                        ret = (e.InnerException.Message);
                        if (e.InnerException.InnerException != null)
                        {
                            ret += e.InnerException.InnerException.Message;
                        }
                    }
                }
                else
                {
                    ret = (e.Message);
                }
                return jss.Serialize("?" + ret);
            }
            #endregion
        }

        public string Verificar_planilla(string json)
        {
            try
            {
                jss.MaxJsonLength = 999999999;
                dynamic data = jss.DeserializeObject(json);
                string ruta = data["ruta"];
                int consecutivo = data["consecutivo"];
                DateTime fecha = DateTime.Today;
                string anio = fecha.Year.ToString();
                List<object> arr = new List<object>();
                List<object> arr2 = new List<object>();
                List<Excel> arr1 = new List<Excel>();
                List<Excel> arr12 = new List<Excel>();
                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                string estado = "PENDIENTE";
                ExcelFile workbook = ExcelFile.Load(ruta);
                var sb = new StringBuilder();

                // Iterate through all worksheets in an Excel workbook.
                foreach (var worksheet in workbook.Worksheets)
                {
                    sb.AppendLine();
                    sb.AppendFormat("{0} {1} {0}", new string('-', 25), worksheet.Name);

                    // Iterate through all rows in an Excel worksheet.
                    int counter = 0;

                    foreach (var row in worksheet.Rows)
                    {
                        if (counter >= 1)
                        {
                            Excel e = new Excel();
                            sb.AppendLine();
                            e.Comp = row.AllocatedCells[0].StringValue;
                            e.Cia = row.AllocatedCells[1].StringValue;
                            e.Vigencia = row.AllocatedCells[2].StringValue;
                            e.Mes = row.AllocatedCells[3].StringValue;
                            e.Periodo = row.AllocatedCells[4].StringValue;
                            e.Area = row.AllocatedCells[5].StringValue;
                            e.Impu = row.AllocatedCells[6].StringValue;
                            e.Valor = Convert.ToInt32(row.AllocatedCells[7].StringValue);


                            presupuesto p = entiti.presupuesto.Where(z => z.ccosto.Equals(e.Area) && z.cuenta.Equals(e.Impu) && z.ano.Equals(anio)).FirstOrDefault();

                            if (p != null)
                            {
                                if (p.disponibilidad - e.Valor >= 0)
                                {
                                    estado = "PENDIENTE";
                                }
                                else
                                {
                                    estado = "RETENIDO";
                                    arr.Add(e);
                                }
                            }
                            else
                            {
                                estado = "RETENIDO";
                                arr.Add(e);
                            }
                        }

                        counter = counter + 1;
                    }
                }
                if (estado.Equals("PENDIENTE"))
                {
                    planillas_nomina pn = entiti.planillas_nomina.Where(z => z.consecutivo == consecutivo).FirstOrDefault();
                    pn.estado = estado;
                    int rs2 = entiti.SaveChanges();
                }

                return "[" + jss.Serialize(arr) + "," + jss.Serialize(estado) + "]";
            }
            #region catch
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize("?" + err);
            }

            catch (Exception e)
            {

                string ret = "";
                if (e.InnerException != null)
                {
                    if (e.InnerException.Message != null)
                    {
                        ret = (e.InnerException.Message);
                        if (e.InnerException.InnerException != null)
                        {
                            ret += e.InnerException.InnerException.Message;
                        }
                    }
                }
                else
                {
                    ret = (e.Message);
                }
                return jss.Serialize("?" + ret);
            }
            #endregion
        }
    }

   
}
