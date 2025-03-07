namespace ChurchManager.Infrastructure.Abstractions.Network;

public interface IAwsIpRangeLoader
{
    public Task<HashSet<string>> ReadAllowedIpRanges(string[] regions);
}