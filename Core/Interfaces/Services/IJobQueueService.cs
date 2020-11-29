using System;
using System.Linq.Expressions;

namespace Core.Interfaces.Services
{
    public interface IJobQueueService
    {
        public string Enqueue<T>(Expression<Action<T>> methodCall);
        public void RecurringJobHourly<T>(string id, Expression<Action<T>> methodCall, int everyHours);
    }
}
