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
}