namespace BlackJack.Services.BlackJack.JSON
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SampleDataObject
    {
        public SampleDataObject(int NewId, string NewName)
        {
            this.Id = NewId;
            this.Name = NewName;
        }
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}