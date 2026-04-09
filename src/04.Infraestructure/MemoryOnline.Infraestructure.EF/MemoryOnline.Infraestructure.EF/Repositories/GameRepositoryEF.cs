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

            return await Task.FromResult(game != null ? new List<GameState> { game } : []);
        }

        public async Task<IEnumerable<GameState>> GetGameByNameAsync(string gameName)
        {
            // Owned entities (Players, Cards) are loaded automatically with OwnsMany
            var result = _context.Games
                 .Include(g => g.Players)
        .Include(g => g.Cards)
                .Where(g => g.Name == gameName)
                .ToList();
            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<GameState>> GetAllAsync()
        {
            // Owned entities (Players, Cards) are loaded automatically with OwnsMany
            var result = _context.Games
               
        .ToList();
            return await Task.FromResult(result);
        }

        public async Task AddAsync(GameState game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GameState game)
        {
            var existingGame = _context.Games
                .Include(g => g.Players)
                .Include(g => g.Cards)
                .FirstOrDefault(g => g.Id == game.Id);

            if (existingGame == null)
                throw new KeyNotFoundException($"Game with Id '{game.Id}' not found");

            // Actualizar propiedades escalares
            existingGame.Name = game.Name;
            existingGame.Level = game.Level;
            existingGame.Version = game.Version;

            // Sincronizar Players: agregar los nuevos
            foreach (var player in game.Players)
            {
                if (!existingGame.Players.Any(p => p.Id == player.Id))
                {
                    existingGame.Players.Add(player);
                }
            }

            // Sincronizar Cards: agregar las nuevas
            foreach (var card in game.Cards)
            {
                if (!existingGame.Cards.Any(c => c.Id == card.Id))
                {
                    existingGame.Cards.Add(card);
                }
            }

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
