using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Features.Finances.Services;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Shared.BankImport;
using Moq;
using Xunit;

namespace ChurchManager.Infrastructure.Tests.Xero;

public class Ofx_Import_Tests()
{

    private string _ofxFilePath = "./Banking/Files/sample.ofx";

    [Fact]
    public async Task Test_Import()
    {
        using var stream = File.OpenRead(_ofxFilePath);
        var sut = new OfxBankStatementImporter();
        var result = await sut.ImportAsync(stream, "sample.ofx");
            
        Assert.NotNull(result);
    }
    
    [Fact]
    public void Test_BankReference_Parsing()
    {
        var peopleDb = new Mock<IPersonDbRepository>().Object;
        var churchesDb = new Mock<IReadDbRepository<Church>>().Object;
        var cache = new Mock<IQueryCache>().Object;
        var sut = new GivingReferenceResolver(churchesDb, peopleDb, cache);

        var result = sut.Parse("CHU-0821234000-P-HS-F");
        
        Assert.Equal("CHU", result.ChurchCode);
        Assert.Equal("0821234000", result.PhoneNumber);
        Assert.True(result.Type.Value == GivingType.Partnership.Value);
        Assert.True(result.IsFamily);
    }

}