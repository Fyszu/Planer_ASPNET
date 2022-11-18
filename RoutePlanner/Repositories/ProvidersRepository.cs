using RoutePlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace RoutePlanner.Repositories
{
    public class ProvidersRepository : IProvidersRepository
    {
        private readonly MyDbContext context;
        public ProvidersRepository(MyDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Provider entity)
        {
            if (entity != null)
            {
                await context.Providers.AddAsync(entity);
                await context.SaveChangesAsync();
            }
            else
                throw new Exception("Błąd podczas dodawania operatora. Przekazany operator jest nullem.");
        }

        public async Task<List<Provider>> GetAllAsync()
        {
            return await context.Providers.ToListAsync();
        }

        public async Task<Provider> GetByIdAsync(long id)
        {
            return await context.Providers.Include(provider => provider.ChargingStations).Where(provider => provider.Id == id).SingleOrDefaultAsync();
        }

        public async Task RemoveAsync(Provider entity)
        {
            if (entity != null)
            {
                context.Providers.Remove(entity);
                await context.SaveChangesAsync();
            }
            else
                throw new Exception("Błąd podczas usuwania operatora. Przekazany operator jest nullem.");
        }

        // Removes all records from Providers table
        public async Task RemoveAllAsync()
        {
            await context.Providers
                .ForEachAsync(provider => {
                if (provider != null)
                    context.Providers.Remove(provider);
            });
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Provider entity)
        {
            if (entity != null)
            {
                Provider dbProvider = await GetByIdAsync(entity.Id);
                if (dbProvider != null)
                {
                    context.Entry(dbProvider).CurrentValues.SetValues(entity);
                    await context.SaveChangesAsync();
                }
                else
                    throw new Exception("Błąd podczas aktualizacji operatora. Nie znaleziono operatora o ID: " + entity.Id);
            }
            else
                throw new Exception("Błąd podczas aktualizacji operatora. Przekazany operator jest nullem.");
        }
    }
}
