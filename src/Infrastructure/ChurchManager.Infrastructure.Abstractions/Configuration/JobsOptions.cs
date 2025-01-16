namespace ChurchManager.Infrastructure.Abstractions.Configuration;

public class JobsOptions
{
    /// <summary>
    /// Enabled flag for jobs. Default is true.
    /// </summary>
    public bool Enabled { get; set; }
    
    /// <summary>
    /// Enable Debug Jobs for testing.
    /// <seealso cref="TestJob"/>
    /// <seealso cref="TestJobTwo"/>
    /// </summary>
    public bool DebugEnabled { get; set; }
}