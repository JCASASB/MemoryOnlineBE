using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.EF.Game.Repositories
{
    public class MatchRepositoryEF : IMatchRepository
    {
        private readonly IApplicationDbContext _context;

        public MatchRepositoryEF(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Match match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
        }

        public async Task<Match> GetMatchByNameAsync(string name)
        {
            var match = await _context.Matches
                                .Include(m => m.States) 
                                .FirstOrDefaultAsync(m => m.States.First().Name == name);
            return match;
        }

        public async Task<IEnumerable<BoardState>> GetAllBoardStatesAsync(Guid matchId)
        {
            var match = await _context.Matches
                                .Include(m => m.States) // Carrega els estats de la taula/JSON
                                .FirstOrDefaultAsync(m => m.Id == matchId);

            // Si el match existeix, retornem els seus estats; si no, una llista buida
            return match?.States ?? new List<BoardState>();
        }

        public async Task UpdateAsync(Match match)
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNewStateAsync(Guid matchId, BoardState newState)
        {
            // 1. Recuperem el Match amb els seus estats
            var match = await _context.Matches
                .Include(m => m.States)
                .FirstOrDefaultAsync(m => m.Id == matchId);

            if (match == null) return;

            match.States.Add(newState);

            // 3. Guardar
            await _context.SaveChangesAsync();
        }
    }
}
