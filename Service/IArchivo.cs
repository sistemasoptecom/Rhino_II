using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Rhino.Service
{

    [ServiceContract]
    public interface IArchivo
    {
        #region USUARIO        
        [OperationContract]
        [WebInvoke(
           Method = "POST",
           UriTemplate = "/Auth",
           ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.WrappedRequest
           )]
        string Auth(string json);
       
        #endregion
    

    }
}
