using ChurchManager.Infrastructure.Abstractions.FeatureFlags;
using DotLiquid.Util;
using Flagsmith;

namespace ChurchManager.Infrastructure.Shared.FeatureFlags
{
    public class FlagsmithFeatureClient : IFeatureFlagClient
    {
        private readonly IFlagsmithClient _client;

        public FlagsmithFeatureClient(IFlagsmithClient client)
        {
            _client = client;
        }
        public async Task<bool?> IsFeatureEnabledAsync(string featureId, string identity = null)
        {
            var flags = await _client.GetEnvironmentFlags();
            var featureEnabled = await flags.IsFeatureEnabled(featureId);
            return featureEnabled.IsTruthy();
        }

        public async Task<string> GetFeatureValueAsync(string featureId, string identity = null)
        {
            var flags = await _client.GetEnvironmentFlags();
            return await flags.GetFeatureValue(featureId);
        }
    }
}
