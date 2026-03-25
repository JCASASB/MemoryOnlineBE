using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Repository.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly GameDbContext _context;

        public GameRepository(GameDbContext context)
        {
            _context = context;
        }

        public void AddGame(GameState game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();
        }


        public GameState? GetGame(Guid id)
        {
            return _context.Games
                .Include(g => g.Players)
                .Include(g => g.Cards)
                .FirstOrDefault(g => g.Id == id);
        }


        public GameState? GetGameByName(string gameName)
        {
            return _context.Games
                .Include(g => g.Players)
                .Include(g => g.Cards)
                .AsNoTracking()
                .FirstOrDefault(g => g.Name == gameName);
        }


        public List<GameState> GetAllGames()
        {
            return _context.Games
                .Include(g => g.Players)
                .Include(g => g.Cards)
                .ToList();
        }


        public void UpdateGame(GameState game)
        {
            // Cargar el juego existente con sus hijos para eliminar todo manualmente si no hay Cascade Delete
            var existing = _context.Games
                .Include(g => g.Players)
                .Include(g => g.Cards)
                .FirstOrDefault(g => g.Id == game.Id);

            if (existing != null)
            {
                // Eliminar hijos manualmente si no tienes Cascade Delete
                _context.Players.RemoveRange(existing.Players);
                _context.Cards.RemoveRange(existing.Cards);
                _context.Games.Remove(existing);
                _context.SaveChanges();
            }

            _context.Games.Add(game);
            _context.SaveChanges();
        }


        public void DeleteGame(Guid id)
        {
            var game = _context.Games.Find(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                _context.SaveChanges();
            }
        }
    }
}
