using System;
using Mafia.Core.Interfaces;
using Mafia.Core.Models;

namespace Mafia.Application.Services;

public class GameRegistrationService : IGameRegistrationService
{
    private readonly IGameRepository _gameRepository;
    private readonly IGameRegistrationRepository _gameRegistrationRepository;
    public GameRegistrationService(IGameRepository gameRepository,
        IGameRegistrationRepository gameRegistrationRepository)
    {
        _gameRepository = gameRepository;
        _gameRegistrationRepository = gameRegistrationRepository;   
    }

   public async Task<IEnumerable<GameRegistration>> GetAllAsync()
   {
        return await _gameRegistrationRepository.GetAllAsync();
   }

   public async Task<IEnumerable<GameRegistration>> GetAllByGameIdAsync(string gameId)
   {
    return await _gameRegistrationRepository.GetAllByGameIdAsync(gameId);
   }

    public async Task<IEnumerable<GameRegistration>> GetAllByUserIdAsync(string userId)
    {
        return await _gameRegistrationRepository.GetAllByUserIdAsync(userId);
    }

    public async Task<GameRegistration?> GetByIdAsync(string gameId)
    {
        return await _gameRegistrationRepository.GetByIdAsync(gameId);
    }

    public async Task<GameRegistration?> GetByGameIdAndUserIdAsync(string gameId, string userId)
    {
        return await _gameRegistrationRepository.GetByGameIdAndUserIdAsync(gameId, userId);
    }

    public async Task<string> RegisterForTheGameAsync(string userId, string gameId)
    {
        // Проверяем, существует ли игра
        var game = await _gameRepository.GetByIdAsync(gameId);
        if (game == null)
        {
            throw new InvalidOperationException("Игра не найдена");
        }

        // Проверяем, не закончилась ли регистрация
        if (game.EndOfRegistration < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Регистрация на игру закрыта");
        }

        // Проверяем, не заполнена ли игра
        if (game.CurrentPlayers >= game.MaxPlayers)
        {
            throw new InvalidOperationException("Все места на игру заняты");
        }

        // Проверяем, не зарегистрирован ли уже пользователь
        var existingRegistration = await _gameRegistrationRepository.GetByGameIdAndUserIdAsync(gameId, userId);
        if (existingRegistration != null)
        {
            throw new InvalidOperationException("Вы уже зарегистрированы на эту игру");
        }

        // Создаем регистрацию
        var registration = new GameRegistration
        {
            GameId = gameId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            IsApproved = false
        };

        var registrationId = await _gameRegistrationRepository.CreateAsync(registration);

        // Увеличиваем счетчик игроков
        await _gameRepository.IncrementCurrentPlayersAsync(gameId);

        return registrationId;
    }
}
