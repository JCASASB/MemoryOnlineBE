using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.EF.Context;
using MemoryOnline.Infraestructure.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.EF.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _context;

        public GameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<GameState> GetAsync(Guid id)
        {
            var game = _context.Games
                .Include(g => g.Players)
                .Include(g => g.Cards)
                .FirstOrDefault(g => g.Id == id);

            return game != null ? [game] : [];
        }

        public IEnumerable<GameState> GetGameByNameAsync(string gameName)
        {
            return _context.Games
                .Include(g => g.Players)
                .Include(g => g.Cards)
                .Where(g => g.Name == gameName)
                .ToList();
        }

        public IEnumerable<GameState> GetAllAsync()
        {
            return _context.Games
                .Include(g => g.Players)
                .Include(g => g.Cards)
                .ToList();
        }

        public void AddAsync(GameState game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();
        }

        public void UpdateAsync(GameState game)
        {
            // Con POCOs, simplemente usar Update que manejará todo automáticamente
            _context.Games.Update(game);
            _context.SaveChanges();
        }

        public void DeleteAsync(Guid id)
        {
            var game = _context.Games.FirstOrDefault(g => g.Id == id);
            if (game != null)
            {
                _context.Games.Remove(game);
                _context.SaveChanges();
            }
        }
    }
}
