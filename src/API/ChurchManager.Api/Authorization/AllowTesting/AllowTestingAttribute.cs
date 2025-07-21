namespace ChurchManager.Api.Authorization.AllowTesting;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AllowTestingAttribute() : Attribute;