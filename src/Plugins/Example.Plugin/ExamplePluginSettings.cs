using ChurchManager.Domain.Features.Settings;

namespace Example.Plugin;

public class ExamplePluginSettings : ISettings
{
    public bool Setting1 { get; set; }
    public string Setting2 { get; set; } = "Default Value";
}