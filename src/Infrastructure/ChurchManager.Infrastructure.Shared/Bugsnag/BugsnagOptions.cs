using BugsnagLib = Bugsnag;

namespace ChurchManager.Infrastructure.Shared.Bugsnag
{
    public class BugsnagOptions : BugsnagLib.Configuration
    {
        public bool Enabled { get; set; } = false;
    }
}
