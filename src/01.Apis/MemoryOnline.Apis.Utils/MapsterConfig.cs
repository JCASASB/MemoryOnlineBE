using Mapster;
using MemoryOnline.Apis.Utils.DTOs.In;
using MemoryOnline.Apis.Utils.DTOs.Out;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Apis.Utils
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // 1. Mapeo Individual de Player (Usando su Builder)
            config.NewConfig<PlayerDtoIn, Player>()
                .ConstructUsing(src => new Player.Builder()
                    .WithId(Guid.Parse(src.Id))
                    .WithName(src.Name)
                    .WithRemainMoves(src.RemainMoves)
                    .WithTotalMoves(src.TotalMoves)
                    .WithPoints(src.Points)
                    .WithTurn(src.Turn)
                    .Build());

            // 2. Mapeo Individual de Card (Usando su Builder)
            config.NewConfig<CardDtoIn, Card>()
                .ConstructUsing(src => new Card.Builder()
                    .WithId(Guid.Parse(src.Id))
                    .WithValue(int.Parse(src.Value))
                    .WithImage(src.ImgUrl)
                    .WithState(src.IsMatched ? CardState.Matched : (src.IsRevealed ? CardState.FaceUp : CardState.FaceDown))
                    .Build());

            // 3. Mapeo de GameState (El objeto principal)
            config.NewConfig<GameStateDtoIn, GameState>()
                .ConstructUsing(src => new GameState.Builder()
                    .WithId(Guid.Parse(src.Id))
                    .WithName(src.Name)
                    .WithLevel(src.Level)
                    .Build())
                // ¡ESTO ES LO MÁS IMPORTANTE! 
                // Evita que Mapster intente hacer: dest.Players = src.Players
                .Ignore(dest => dest.Players)
                .Ignore(dest => dest.Cards)
                .AfterMapping((src, dest) =>
                {
                    // Limpiamos para evitar duplicados si el objeto ya existía
                    dest.Players.Clear();
                    dest.Cards.Clear();

                    // Usamos Adapt para que Mapster aplique las reglas 1 y 2 que definimos arriba
                    if (src.Players != null)
                    {
                        var players = src.Players.Adapt<List<Player>>();
                        foreach (var p in players) dest.AddPlayer(p);
                    }

                    if (src.Cards != null)
                    {
                        var cards = src.Cards.Adapt<List<Card>>();
                        foreach (var c in cards) dest.AddCard(c);
                    }
                });

            // --- DOMINIO -> DTO OUT (Solo lectura, es más sencillo) ---
            // --- MAPEO DE PLAYER (Dominio -> DTO Out) ---
            config.NewConfig<Player, PlayerDtoOut>()
                .Map(dest => dest.id, src => src.Id.ToString())
                .Map(dest => dest.name, src => src.Name)
                .Map(dest => dest.remainMoves, src => src.RemainMoves)
                .Map(dest => dest.totalMoves, src => src.TotalMoves)
                .Map(dest => dest.points, src => src.Points)
                .Map(dest => dest.turn, src => src.Turn);

            // --- MAPEO DE CARD (Dominio -> DTO Out) ---
            config.NewConfig<Card, CardDtoOut>()
                .Map(dest => dest.id, src => src.Id.ToString())
                .Map(dest => dest.value, src => src.Value.ToString())
                .Map(dest => dest.imgUrl, src => src.ImgUrl) // <-- Revisa si en Card es 'ImgUrl' o 'Image'
                .Map(dest => dest.isRevealed, src => src.State == CardState.FaceUp)
                .Map(dest => dest.isMatched, src => src.State == CardState.Matched);

            // --- MAPEO DE GAMESTATE (Dominio -> DTO Out) ---
            config.NewConfig<GameState, GameStateDtoOut>()
                .Map(dest => dest.id, src => src.Id.ToString())
                .Map(dest => dest.name, src => src.Name)
                .Map(dest => dest.level, src => src.Level)
                .Map(dest => dest.isProcessing, src => src.IsProcessing)
                // Al tener configurados Player y Card arriba, estas listas ya no saldrán null
                .Map(dest => dest.cards, src => src.Cards)
                .Map(dest => dest.players, src => src.Players);



        }

    }
}