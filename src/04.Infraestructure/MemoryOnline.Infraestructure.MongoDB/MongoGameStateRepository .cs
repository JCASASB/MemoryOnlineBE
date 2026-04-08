using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Infraestructure.MongoDB
{
    public class MongoGameStateRepository : IGameStateRepository
    {
        private readonly IMongoCollection<GameState> _states;

        public MongoGameStateRepository(IMongoDatabase database)
        {
            _states = database.GetCollection<GameState>("GameStates");
        }

        public async Task SaveAsync(GameState state)
        {
            // Filtramos por PlayerId para actualizar el documento correcto
            var filter = Builders<GameState>.Filter.Eq(s => s.PlayerId, state.PlayerId);

            // IsUpsert = true hace que se cree si no existe
            await _states.ReplaceOneAsync(filter, state, new ReplaceOptions { IsUpsert = true });
        }

        public async Task<GameState?> GetByPlayerIdAsync(string playerId) =>
            await _states.Find(s => s.PlayerId == playerId).FirstOrDefaultAsync();
    }
}
