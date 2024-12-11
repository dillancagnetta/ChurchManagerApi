namespace ChurchManager.Infrastructure.Abstractions.FeatureFlags
{
    public interface IFeatureFlagClient
    {
        Task<bool?> IsFeatureEnabledAsync(string featureId, string identity = null);
        Task<string> GetFeatureValueAsync(string featureId, string identity = null);
    }
}
