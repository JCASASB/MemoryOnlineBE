using System.Collections.Concurrent;
using MemoryOnlineBE.Models;
using Microsoft.AspNetCore.SignalR;

namespace MemoryOnlineBE.Hubs;

/// <summary>
/// Hub de SignalR para el juego Memory multijugador en tiempo real.
/// Cada "sala" (gameId) permite a dos jugadores ver las mismas cartas.
/// </summary>
public class GameHub : Hub
{
    // Salas activas en memoria (en producción usar Redis o similar)
    private static readonly ConcurrentDictionary<string, GameRoom> Rooms = new();

    /// <summary>
    /// Un jugador se une a una sala de juego.
    /// </summary>
    public async Task JoinGame(string gameId)
    {
        var room = Rooms.GetOrAdd(gameId, _ => new GameRoom { GameId = gameId });

        if (!room.Players.Contains(Context.ConnectionId))
        {
            room.Players.Add(Context.ConnectionId);
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);

        // Notificar a los demás que un jugador se unió
        await Clients.OthersInGroup(gameId).SendAsync("PlayerJoined", Context.ConnectionId);

        Console.WriteLine($"[GameHub] Jugador {Context.ConnectionId} se unió a sala {gameId} ({room.Players.Count} jugadores)");

        // Si la sala ya tiene cartas (el otro jugador ya inició), enviar el estado actual
        if (room.Cards.Count > 0)
        {
            await Clients.Caller.SendAsync("GameStarted", room.Level, room.Cards);
            Console.WriteLine($"[GameHub] Enviando estado existente al jugador que se une: {room.Cards.Count} cartas");
        }
    }

    /// <summary>
    /// Un jugador abandona la sala.
    /// </summary>
    public async Task LeaveGame(string gameId)
    {
        if (Rooms.TryGetValue(gameId, out var room))
        {
            room.Players.Remove(Context.ConnectionId);
            if (room.Players.Count == 0)
            {
                Rooms.TryRemove(gameId, out _);
            }
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
        Console.WriteLine($"[GameHub] Jugador {Context.ConnectionId} abandonó sala {gameId}");
    }

    /// <summary>
    /// Un jugador voltea una carta. Se reenvía a los demás jugadores de la sala.
    /// </summary>
    public async Task FlipCard(string gameId, string cardId)
    {
        Console.WriteLine($"[GameHub] FlipCard en sala {gameId}: carta {cardId}");
        await Clients.OthersInGroup(gameId).SendAsync("CardFlipped", cardId);
    }

    /// <summary>
    /// El host inicia la partida. Genera cartas y las envía a todos para sincronizar el tablero.
    /// </summary>
    public async Task StartGame(string gameId, int level)
    {
        var cards = GenerateCards(level);

        // Guardar las cartas en la sala para sincronizar jugadores que se unan después
        if (Rooms.TryGetValue(gameId, out var room))
        {
            room.Cards = cards;
            room.Level = level;
        }

        Console.WriteLine($"[GameHub] StartGame en sala {gameId}: nivel {level}, {cards.Count} cartas");

        // Enviar a todos los DEMÁS (el host ya las tiene localmente)
        await Clients.OthersInGroup(gameId).SendAsync("GameStarted", level, cards);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Limpiar al jugador de todas las salas
        foreach (var (gameId, room) in Rooms)
        {
            if (room.Players.Remove(Context.ConnectionId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
                await Clients.OthersInGroup(gameId).SendAsync("PlayerDisconnected", Context.ConnectionId);

                if (room.Players.Count == 0)
                {
                    Rooms.TryRemove(gameId, out _);
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Genera pares de cartas barajadas para el nivel dado.
    /// </summary>
    private static List<CardDto> GenerateCards(int level)
    {
        var rng = new Random();
        var values = Enumerable.Range(1, level - 1)
            .SelectMany(i => new[] { i.ToString(), i.ToString() })
            .OrderBy(_ => rng.Next())
            .Select((v, i) => new CardDto(i.ToString(), v, false, false))
            .ToList();

        return values;
    }
}
