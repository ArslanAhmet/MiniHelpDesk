using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniHelpDesk.BuildingBlocks.Shared
{
    public class RabbitMQHealthCheck : IHealthCheck
    {
        //private readonly ManagementClient _managementClient;
        private readonly string _topicName;

        public RabbitMQHealthCheck(string connectionString, string topicName)
        {
            //_managementClient = new ManagementClient(connectionString);
            _topicName = topicName;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                //_ = await _managementClient.GetTopicRuntimeInfoAsync(_topicName, cancellationToken);

                return HealthCheckResult.Healthy();
            }
            catch (Exception e)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: e);
            }
        }
    }
}
