using MedprCore.Abstractions;
using MedprCore.DTO;

namespace MedprWebAPI.Utils;

public static class UserNotification
{
    /// <summary>
    /// Plug method for future SignalR functionality
    /// </summary>
    /// <typeparam name="T">Model with date, user info and notification id</typeparam>
    /// <param name="dto">Object to form notification</param>
    /// <param name="notificationService">SignalR service</param>
    /// <param name="model">Model</param>
    public static async Task NotifyUser<T>(T dto, INotificationService notificationService) where T : INotifyUser
    {
        Console.WriteLine($"User will be notified {dto.Date}");
    }
}