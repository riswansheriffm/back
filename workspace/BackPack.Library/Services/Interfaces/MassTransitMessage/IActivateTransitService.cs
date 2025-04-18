
namespace BackPack.Library.Services.Interfaces.MassTransitMessage
{
    public interface IActivateTransitService
    {
        Task<bool> ActivateTransitAsync(string transitURL);
    }
}
