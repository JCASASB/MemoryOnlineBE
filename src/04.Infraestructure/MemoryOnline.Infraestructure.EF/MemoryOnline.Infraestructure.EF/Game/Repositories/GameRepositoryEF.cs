using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.EF.Game.Context;
using MemoryOnline.Infraestructure.IRepository.Game;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.EF.Game.Repositories
{
    public class GameRepositoryEF : IGameRepository
    {
        private readonly GameDbContext _context;

        public GameRepositoryEF(IGameDbContext context)
        {
            _context = context as GameDbContext;
        }

        public async Task AddMatchAsync(Match match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
        }

        public async Task AddChallengeAsync(Challenge challenge)
        {
            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync();
        }

        

        public async Task<Match> GetMatchByNameAsync(string name)
        {
            var match = await _context.Matches
                                .Include(m => m.States) 
                                .FirstOrDefaultAsync(m => m.States.First().Name == name);
            return match;
        }

        public async Task<IEnumerable<Match>> GetAllMatchAsync()
        {
            var matches = await _context.Matches
                                .Include(m => m.States)
                                .ToListAsync();
            return matches;
        }

        public async Task<IEnumerable<BoardState>> GetAllBoardStatesAsync(Guid matchId)
        {
            var match = await _context.Matches
                                .Include(m => m.States) // Carrega els estats de la taula/JSON
                                .FirstOrDefaultAsync(m => m.Id == matchId);

            // Si el match existeix, retornem els seus estats; si no, una llista buida
            return match?.States ?? new List<BoardState>();
        }

        public async Task<Match> GetMatchByIdAsync(Guid matchId)
        {
            return await _context.Matches
                .FirstOrDefaultAsync(m => m.Id == matchId);
        }

        public async Task UpdateMatchAsync(Match match)
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

        public async Task<Challenge> GetChallengeAsync(Guid challengeId)
        {
            return await _context.Challenges
                .FirstOrDefaultAsync(c => c.Id == challengeId);
        }
    }
}
