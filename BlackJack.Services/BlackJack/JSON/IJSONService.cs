namespace BlackJack.Services.BlackJack.JSON
{
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Namespace="http://DotNetByExample/JSONDemoService")]
    public interface IJSONService
    {
        [OperationContract]
        SampleDataObject GetSingleObject(int id, string name);

        [OperationContract]
        ICollection<SampleDataObject> GetMultipleObjects(int number);
    }
}