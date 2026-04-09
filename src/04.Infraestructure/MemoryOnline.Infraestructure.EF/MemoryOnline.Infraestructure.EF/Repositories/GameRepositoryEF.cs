using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.EF.Repositories
{
    public class GameRepositoryEF : IGameRepository
    {
        private readonly IApplicationDbContext _context;

        public GameRepositoryEF(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GameState>> GetAsync(Guid id)
        {
            // Owned entities (Players, Cards) are loaded automatically with OwnsMany
            var game = _context.Games.FirstOrDefault(g => g.Id == id);

            return await Task.FromResult(game != null ? new List    <GameState> { game } : []);
        }

        public async Task<IEnumerable<GameState>> GetGameByNameAsync(string gameName)
        {
            // Owned entities (Players, Cards) are loaded automatically with OwnsMany
            var result = _context.Games
                .Where(g => g.Name == gameName)
                .ToList();
            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<GameState>> GetAllAsync()
        {
            // Owned entities (Players, Cards) are loaded automatically with OwnsMany
            var result = _context.Games.ToList();
            return await Task.FromResult(result);
        }

        public async Task AddAsync(GameState game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GameState game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var game = _context.Games.FirstOrDefault(g => g.Id == id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
        }
    }
}
