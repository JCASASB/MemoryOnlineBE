
namespace MemoryOnline.Domain.Entities.Game
{
    public class Player
    {
        // Propiedades con private set para proteger el estado (Encapsulamiento)
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int RemainMoves { get; private set; }
        public int TotalMoves { get; private set; }
        public int Points { get; private set; }
        public bool Turn { get; private set; }

        // Constructor vacío para EF Core
        private Player() { }

        public class Builder
        {
            private readonly Player _player = new Player();

            public Builder()
            {
                // Valores por defecto para un jugador nuevo
                _player.Id = Guid.NewGuid();
                _player.Points = 0;
                _player.TotalMoves = 0;
                _player.Turn = false;
                _player.RemainMoves = 2; // O el valor inicial de tu juego
            }

            public Builder WithId(Guid id) { _player.Id = id; return this; }

            public Builder WithName(string name)
            {
                _player.Name = name;
                return this;
            }

            public Builder WithRemainMoves(int moves)
            {
                _player.RemainMoves = moves;
                return this;
            }

            public Builder WithTotalMoves(int moves)
            {
                _player.TotalMoves = moves;
                return this;
            }

            public Builder WithPoints(int points)
            {
                _player.Points = points;
                return this;
            }

            public Builder WithTurn(bool isHisTurn)
            {
                _player.Turn = isHisTurn;
                return this;
            }

            public Player Build()
            {
                if (string.IsNullOrWhiteSpace(_player.Name))
                    throw new InvalidOperationException("El jugador debe tener un nombre.");

                return _player;
            }
        }
    }
}
