namespace ChurchManager.Infrastructure.Shared.Templating.DataResolvers;

[AttributeUsage(AttributeTargets.Class)]
public class TemplateNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}