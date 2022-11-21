using AutoMapper;
using MediatR;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprModels;
using MedprModels.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MedprWebAPI.Utils;

public static class UserNotification
{
    //private readonly IUserService _userService;
    //public UserNotification(IUserService userService)
    //{
    //    _userService = userService;
    //}

    /// <summary>
    /// Plug method for future SignalR functionality
    /// </summary>
    /// <typeparam name="T">Model with date, user info and notification id</typeparam>
    /// <param name="model">Model</param>
    public static async Task NotifyUser<T>(T dto) where T : INotifyUser
    {
        Console.WriteLine($"User will be notified {dto.Date}");
    }
}
