﻿using ClassLibrary.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.IRepository;
using ClassLibrary.Domain;

namespace WinUI.Service.NotificationServiceFile
{
    /// <summary>
    /// Provides high-level notification operations on top of the repository.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="repo">The notification repository used for data access.</param>
        public NotificationService(INotificationRepository repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// Retrieves all notifications across all users.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, 
        /// containing the list of all notifications.
        /// </returns>
        public Task<List<Notification>> GetAllAsync()
        {
            return repo.GetAllNotificationsAsync();
        }

        /// <summary>
        /// Retrieves all notifications for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A task representing the asynchronous operation, 
        /// containing the list of notifications for the given user.
        /// </returns>
        public Task<List<Notification>> GetByUserIdAsync(int userId)
        {
            return repo.GetNotificationsByUserIdAsync(userId);
        }

        /// <summary>
        /// Deletes a single notification if it belongs to the specified user.
        /// </summary>
        /// <param name="notificationId">The unique identifier of the notification.</param>
        /// <param name="userId">The unique identifier of the user attempting the deletion.</param>
        /// <returns>A task representing the asynchronous delete operation.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when no notification with the specified ID exists.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// Thrown when the notification does not belong to the given user.
        /// </exception>
        public async Task DeleteAsync(int notificationId, int userId)
        {
            // Fetch the notification to verify ownership
            var notification = await repo.GetNotificationByIdAsync(notificationId);

            if (notification == null)
                throw new KeyNotFoundException(
                    $"Notification with ID {notificationId} not found.");

            // Ensure the user owns this notification
            if (notification.UserId != userId)
                throw new UnauthorizedAccessException(
                    $"User {userId} is not allowed to delete notification {notificationId}.");

            // Proceed with deletion
            await repo.DeleteNotificationAsync(notificationId);
        }
    }
}
 