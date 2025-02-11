namespace ChurchManager.Infrastructure.Abstractions.Communication;

public interface ITemplateDataResolverFactory
{
    ITemplateDataResolver CreateResolver(string templateName);
}