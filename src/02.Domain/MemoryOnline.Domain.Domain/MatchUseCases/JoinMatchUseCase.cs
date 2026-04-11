
using MemoryOnline.Domain.Entities.Game;
using System.Text.Json;

namespace MemoryOnline.Domain.Domain.MatchUseCases
{
    public class JoinMatchUseCase : IJoinMatchUseCase
    {
        public BoardState Execute(Match match, string playerName)
        {
            var state = match.States.Last();

            var newState = JsonSerializer.Deserialize<BoardState>(JsonSerializer.Serialize(state));

            // Generar un nuevo Id para el nuevo estado
            newState.Id = Guid.NewGuid();

            var countPlayers = newState.Players.Count;

            // Crear el nuevo jugador como POCO
            var player = new Player
            {
                Id = Guid.NewGuid(),
                Name = playerName,
                RemainMoves = 2,
                TotalMoves = 0,
                Points = 0,
                Turn = countPlayers == 1 // El segundo jugador es quien inicia
            };


            newState.Players.Add(player);

            // Inicializar cartas cuando se une el segundo jugador
            if (countPlayers == 1)
            {
                InitializeCards(newState, reinitialize: false);
            }

            newState.Version = newState.Version+1;

            return newState;
        }

        private void InitializeCards(BoardState game, bool reinitialize = false)
        {
            // Solo inicializar si no hay cartas o si se solicita explícitamente reinicializar
            if (game.Cards.Count > 0 && !reinitialize)
                return;

            // Limpiar cartas previas si reinicializamos
            if (reinitialize && game.Cards.Count > 0)
            {
                game.Cards.Clear();
            }

            var deck = new List<Card>();
            var random = new Random();
            var usedValues = new HashSet<int>();

            // El número de parejas depende del nivel
            for (int i = 0; i < game.Level; i++)
            {
                int value;
                // Buscamos un valor único para la pareja
                do { value = random.Next(1, 10000); } while (!usedValues.Add(value));

                string image = $"img_{value}";

                // Creamos el par de cartas como POCOs
                deck.Add(new Card
                {
                    Id= Guid.NewGuid(),
                    Value = value,
                    ImgUrl = image,
                });

                deck.Add(new Card
                {
                    Id= Guid.NewGuid(),
                    Value = value,
                    ImgUrl = image,
                });
            }

            // Barajamos la lista usando OrderBy con un nuevo GUID
            var shuffledCards = deck.OrderBy(x => Guid.NewGuid()).ToList();

            // Añadimos las cartas barajadas a la colección
            foreach (var card in shuffledCards)
            {
                game.Cards.Add(card);
            }
        }
    }
}
