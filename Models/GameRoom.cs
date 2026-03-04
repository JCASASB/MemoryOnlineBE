namespace MemoryOnlineBE.Models;

/// <summary>
/// Representa una sala de juego con sus jugadores y estado de cartas.
/// </summary>
public class GameRoom
{
    public string GameId { get; set; } = string.Empty;
    public List<string> Players { get; } = [];
    public List<CardDto> Cards { get; set; } = [];
    public int Level { get; set; }
}
