namespace BlackJack.Services.BlackJack
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        String SayHello(String name);
    }
}
