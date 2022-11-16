﻿using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public class ProvidersRepository : IProvidersRepository
    {
        private readonly MyDbContext _context;
        public ProvidersRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Provider entity)
        {
            if (entity != null)
            {
                await _context.Providers.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("Błąd podczas dodawania operatora. Przekazany operator jest nullem.");
        }

        public async Task<List<Provider>> GetAllAsync()
        {
            return await _context.Providers.ToListAsync();
        }

        public async Task<Provider> GetByIdAsync(long id)
        {
            return await _context.Providers.Include(provider => provider.ChargingStations).Where(provider => provider.Id == id).SingleOrDefaultAsync();
        }

        public async Task RemoveAsync(Provider entity)
        {
            if (entity != null)
            {
                _context.Providers.Remove(entity);
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("Błąd podczas usuwania operatora. Przekazany operator jest nullem.");
        }

        // Removes all records from Providers table
        public async Task RemoveAllAsync()
        {
            await _context.Providers
                .ForEachAsync(provider => {
                if (provider != null)
                    _context.Providers.Remove(provider);
            });
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Provider entity)
        {
            if (entity != null)
            {
                Provider dbProvider = await GetByIdAsync(entity.Id);
                if (dbProvider != null)
                {
                    _context.Entry(dbProvider).CurrentValues.SetValues(entity);
                    await _context.SaveChangesAsync();
                }
                else
                    throw new Exception("Błąd podczas aktualizacji operatora. Nie znaleziono operatora o ID: " + entity.Id);
            }
            else
                throw new Exception("Błąd podczas aktualizacji operatora. Przekazany operator jest nullem.");
        }
    }
}
