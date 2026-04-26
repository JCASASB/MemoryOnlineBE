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
            // 1. Mapeo Individual de Player (POCO)
            config.NewConfig<PlayerDtoIn, Player>()
                .Map(dest => dest.Id, src => Guid.Parse(src.Id))
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Order, src => src.Order)
                .Map(dest => dest.RemainMoves, src => src.RemainMoves)
                .Map(dest => dest.TotalMoves, src => src.TotalMoves)
                .Map(dest => dest.Points, src => src.Points)
                .Map(dest => dest.Turn, src => src.Turn);

            // 2. Mapeo Individual de Card (POCO)
            config.NewConfig<CardDtoIn, Card>()
                .Map(dest => dest.Id, src => Guid.Parse(src.Id))
                .Map(dest => dest.Value, src => src.Value)
                .Map(dest => dest.ImgUrl, src => src.ImgUrl)
                .Map(dest => dest.State, src => src.State);

            // 3. Mapeo de GameState (POCO)
            config.NewConfig<GameStateDtoIn, BoardState>()
                .Map(dest => dest.Id, src => Guid.Parse(src.Id))
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Level, src => src.Level)
                .Map(dest => dest.Version, src => src.Version)
                .Map(dest => dest.Players, src => src.Players)
                .Map(dest => dest.Cards, src => src.Cards);

            // --- DOMINIO -> DTO OUT (Solo lectura) ---
            // --- MAPEO DE PLAYER (Dominio -> DTO Out) ---
            config.NewConfig<Player, PlayerDtoOut>()
                .Map(dest => dest.id, src => src.Id.ToString())
                .Map(dest => dest.name, src => src.Name)
                .Map(dest => dest.order, src => src.Order)
                .Map(dest => dest.remainMoves, src => src.RemainMoves)
                .Map(dest => dest.totalMoves, src => src.TotalMoves)
                .Map(dest => dest.points, src => src.Points)
                .Map(dest => dest.turn, src => src.Turn);

            // --- MAPEO DE CARD (Dominio -> DTO Out) ---
            config.NewConfig<Card, CardDtoOut>()
                .Map(dest => dest.id, src => src.Id.ToString())
                .Map(dest => dest.value, src => src.Value.ToString())
                .Map(dest => dest.imgUrl, src => src.ImgUrl)
                .Map(dest => dest.state, src => src.State);

            // --- MAPEO DE GAMESTATE (Dominio -> DTO Out) ---
            config.NewConfig<BoardState, GameStateDtoOut>()
                .Map(dest => dest.id, src => src.Id.ToString())
                .Map(dest => dest.name, src => src.Name)
                .Map(dest => dest.level, src => src.Level)
                .Map(dest => dest.version, src => src.Version)
                .Map(dest => dest.cards, src => src.Cards)
                .Map(dest => dest.players, src => src.Players);
        }
    }
}