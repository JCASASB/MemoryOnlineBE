using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.EF.Game.Context;
using MemoryOnline.Infraestructure.IRepository.Game;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.EF.Game.Repositories
{
    public class GameRepositoryEF : IGameRepository
    {
        private readonly IGameDbContext _context;

        public GameRepositoryEF(IGameDbContext context)
        {
            _context = context;
        }

        public async Task AddMatchAsync(Match match)
        {
            _context.AddMatch(match);
            await _context.SaveChangesAsync();
        }

        public async Task AddChallengeAsync(Challenge challenge)
        {
            challenge.MatchId = challenge.Match.Id;
            _context.AddChallenge(challenge);
            await _context.SaveChangesAsync();
        }

        

        public async Task<Match> GetMatchByNameAsync(string name)
        {
            var match = await _context.Matches
                                .Include(m => m.States) 
                                .FirstOrDefaultAsync(m => m.Name == name);
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
            var challenge = await _context.Challenges
                .Include(c => c.Players)
                .FirstOrDefaultAsync(c => c.Id == challengeId);

            if (challenge is not null && challenge.Match is null)
            {
                challenge.Match = await _context.Matches
                    .FirstOrDefaultAsync(m => m.Id == challenge.MatchId);
            }

            return challenge;
        }
    }
}
