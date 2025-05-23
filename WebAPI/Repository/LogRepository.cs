using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.Repository;
using Data;
using ClassLibrary.Domain;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Repository
{
    /// <summary>
    /// Repository class for managing log-related database operations.
    /// </summary>
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _db_context;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogRepository"/> class.
        /// </summary>
        /// <param name="_db_context">The database context.</param>
        public LogRepository(ApplicationDbContext _db_context)
        {
            this._db_context = _db_context;
        }

        /// <inheritdoc/>
        public async Task<List<Log>> getAllLogsAsync()
        {
            List<LogEntity> log_entities = await _db_context.Logs.ToListAsync();

            return log_entities.Select(log => new Log
            {
                logId = log.logId,
                userId = (int)log.userId,
                actionType = log.actionType,
                timestamp = log.timestamp
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<Log> getLogByIdAsync(int id)
        {
            var log_entity = await _db_context.Logs.FindAsync(id);
            if (log_entity == null)
            {
                throw new Exception($"Log with ID {id} not found.");
            }

            return new Log
            {
                logId = log_entity.logId,
                userId = (int)log_entity.userId,
                actionType = log_entity.actionType,
                timestamp = log_entity.timestamp
            };
        }

        /// <inheritdoc/>
        public async Task<Log> getLogByUserIdAsync(int user_id)
        {
            var log_entity = await _db_context.Logs
                .FirstOrDefaultAsync(log => log.userId == user_id);

            if (log_entity == null)
            {
                throw new Exception($"Log for user ID {user_id} not found.");
            }

            return new Log
            {
                logId = log_entity.logId,
                userId = (int)log_entity.userId,
                actionType = log_entity.actionType,
                timestamp = log_entity.timestamp
            };
        }

        /// <inheritdoc/>
        public async Task addLogAsync(Log log)
        {
            var log_entity = new LogEntity
            {
                userId = log.userId,
                actionType = log.actionType,
                timestamp = log.timestamp
            };

            _db_context.Logs.Add(log_entity);
            await _db_context.SaveChangesAsync();

            log.logId = log_entity.logId;
        }

        /// <inheritdoc/>
        public async Task deleteLogAsync(int id)
        {
            var log_entity = await _db_context.Logs.FindAsync(id);
            if (log_entity == null)
            {
                throw new Exception($"Log with ID {id} not found.");
            }

            _db_context.Logs.Remove(log_entity);
            await _db_context.SaveChangesAsync();
        }
    }
}
