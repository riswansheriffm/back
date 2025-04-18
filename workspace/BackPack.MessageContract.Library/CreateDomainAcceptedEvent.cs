namespace BackPack.MessageContract.Library
{
    public class CreateDomainAcceptedEvent : ConsumerBaseResponse
    {
        public int DomainID { get; set; }
    }
}
