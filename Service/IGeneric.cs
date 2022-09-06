using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Rhino.Service
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IGeneric" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IGeneric
    {
        [OperationContract]
        [WebInvoke(
           Method = "POST",
           UriTemplate = "/Buscador",
           ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.WrappedRequest
           )]
        string buscador(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/SaveImg",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string SaveImg(string json);


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/SaveFile2",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string SaveFile2(string json);


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/CargarAuto",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string CargarAuto(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Aprobar_planilla",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Aprobar_planilla(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Verificar_planilla",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Verificar_planilla(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Read_planilla",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Read_planilla(string json);


    }

}
