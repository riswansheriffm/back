
namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface IGetAnActiveTenantRepository
    {
        Task<Guid> GetAnActiveTenantAsync();
    }
}
