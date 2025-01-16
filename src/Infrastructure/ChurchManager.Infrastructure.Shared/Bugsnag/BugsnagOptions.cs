using Bugsnag;

namespace ChurchManager.Infrastructure.Shared.Bugsnag
{
    public class BugsnagOptions : Configuration
    {
        public bool Enabled { get; set; } = false;
    }
}
