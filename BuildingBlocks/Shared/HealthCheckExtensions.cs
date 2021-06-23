using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;

namespace MiniHelpDesk.BuildingBlocks.Shared
{
    public static class HealthCheckExtensions
    {
        public static IHealthChecksBuilder AddRabbitMQTopicHealthCheck(this IHealthChecksBuilder builder,
            string connectionString,
            string topicName,
            string name = default,
            HealthStatus failureStatus = HealthStatus.Degraded,
            IEnumerable<string> tags = default,
            TimeSpan? timeout = default)
        {
            return builder.AddCheck(name ?? $"Rabbit MQ Service Bus: {topicName}",
                new RabbitMQHealthCheck(connectionString, topicName), failureStatus, tags, timeout);
        }
    }
}
