
namespace MemoryOnline.Domain.Entities.Game
{
    public class GameState
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Level { get; private set; }
        public bool IsProcessing { get; private set; }
        public virtual ICollection<Card> Cards { get; private set; } = new List<Card>();
        public virtual ICollection<Player> Players { get; private set; } = new List<Player>();

        // Constructor privado para EF Core
        private GameState() { }

        public bool AddPlayer(Player player)
        {
            if (Players.Count >= 2)
                return false;
            Players.Add(player);
            return true;
        }

        public bool AddCard(Card card)
        {
            Cards.Add(card);
            return true;
        }

        public void InitializeCards(bool reinitialize = false)
        {
            // Solo inicializar si no hay cartas o si se solicita explícitamente reinicializar
            if (this.Cards.Count > 0 && !reinitialize)
                return;

            // Limpiar cartas previas si reinicializamos
            if (reinitialize && this.Cards.Count > 0)
            {
                var cardsToRemove = this.Cards.ToList();
                foreach (var card in cardsToRemove)
                {
                    this.Cards.Remove(card);
                }
            }

            var deck = new List<Card>();
            var random = new Random();
            var usedValues = new HashSet<int>();

            // El número de parejas depende del nivel
            for (int i = 0; i < Level; i++)
            {
                int value;
                // Buscamos un valor único para la pareja
                do { value = random.Next(1, 10000); } while (!usedValues.Add(value));

                string image = $"img_{value}";

                // Creamos el par de cartas
                // IMPORTANTE: EF Core asociará estas cartas al GameState automáticamente

                deck.Add(new Card.Builder()
                    .WithId(Guid.NewGuid())
                    .WithValue(value)
                    .WithImage(image)
                    .Build());

                deck.Add(new Card.Builder()
                    .WithId(Guid.NewGuid())
                    .WithValue(value)
                    .WithImage(image)
                    .Build());
            }

            // Barajamos la lista usando OrderBy con un nuevo GUID (truco rápido y efectivo)
            var shuffledCards = deck.OrderBy(x => Guid.NewGuid()).ToList();

            // Añadimos las cartas barajadas a la colección principal
            foreach (var card in shuffledCards)
            {
                this.Cards.Add(card);
            }
        }


        // Clase Builder interna
        public class Builder
        {
            private readonly GameState _state = new GameState();

            public Builder WithId(Guid id) { _state.Id = id; return this; }
            public Builder WithName(string name) { _state.Name = name; return this; }
            public Builder WithLevel(int level) { _state.Level = level; return this; }

            public GameState Build()
            {
                // Aquí puedes validar reglas de negocio antes de retornar
                if (string.IsNullOrEmpty(_state.Name))
                    throw new InvalidOperationException("El juego debe tener un nombre.");

                if (_state.Id == Guid.Empty) _state.Id = Guid.NewGuid();

                return _state;
            }
        }
    }
}
