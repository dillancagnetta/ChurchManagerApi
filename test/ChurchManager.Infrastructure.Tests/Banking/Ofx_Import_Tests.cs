using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Extensions;
using ChurchManager.Infrastructure.Shared.BankImport;
using Xunit;

namespace ChurchManager.Infrastructure.Tests.Xero;

public class Ofx_Import_Tests()
{

    private string _ofxFilePath = "./Banking/Files/sample.ofx";

    [Fact]
    public async Task should_import_ofx_file()
    {
        using var stream = File.OpenRead(_ofxFilePath);
        var sut = new OfxBankStatementImporter();
        var result = await sut.ImportAsync(stream, "sample.ofx");
            
        Assert.NotNull(result);
    }
    
    [Fact]
    public void should_parse_bank_reference()
    {
        /*var peopleDb = new Mock<IPersonDbRepository>().Object;
        var churchesDb = new Mock<IReadDbRepository<Church>>().Object;
        var fundsDb = new Mock<IReadDbRepository<Fund>>().Object;
        var benefactorsDb = new Mock<IReadDbRepository<Benefactor>>().Object;
        var cache = new Mock<IQueryCache>().Object;*/
        //var sut = new GivingReferenceResolver(churchesDb, peopleDb, fundsDb, benefactorsDb, cache);
        var result = FinancesExtensions.ParsePaymentReference("CHU-0821234000-P-HS-F");
        
        Assert.Equal("CHU", result.ChurchCode);
        Assert.Equal("821234000", result.PhoneNumber); // Leading zeroes are trimmed
        Assert.True(result.Type.Value == GivingType.Partnership.Value);
        Assert.True(result.IsFamily);
    }
    

    [Theory]
    [InlineData("CHU-0821234000-T", true)]           // Basic tithe reference
    [InlineData("CHU-0821234000-T-F", true)]         // Family tithe reference
    [InlineData("CHU-0821234000-O", true)]           // Offering reference
    [InlineData("CHU-0821234000-P-HS-F", true)]      // Family partnership reference
    [InlineData("CHU-0821234000-P-R", true)]         // Valid Partnership reference
    [InlineData("CHU-0821234000-P", false)]          // missing Partnership category reference
    [InlineData("CHU-0821234000", false)]            // Missing type
    [InlineData("CHU0821234000-T", false)]           // Missing hyphen
    [InlineData("CH-0821234000-T", false)]           // Church code too short
    [InlineData("CHUR-0821234000-T", false)]         // Church code too long
    [InlineData("CHU-821234000-T", false)]           // Phone number doesn't start with 0
    [InlineData("CHU-08212340-T", false)]            // Phone number too short
    [InlineData("CHU-0821234000-", false)]           // Missing type
    [InlineData("CHU-0821234000-123", false)]        // Invalid type (numbers)
    [InlineData("", false)]                          // Empty string
    public void should_validate_reference_format(string reference, bool expectedResult)
    {
        var result = FinancesExtensions.IsValidPaymentReference(reference);
    
        Assert.Equal(expectedResult, result);
    }
}