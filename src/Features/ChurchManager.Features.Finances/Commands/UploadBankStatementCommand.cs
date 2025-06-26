using ChurchManager.Domain.Features.Finances.Services;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ChurchManager.Features.Finances.Commands;

public record UploadBankStatementCommand(IFormFile File) : IRequest<ApiResponse>;

public class UploadBankStatementHandler(
    IBankStatementImporter importer,
    IBankStatementProcessor processor) : IRequestHandler<UploadBankStatementCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(UploadBankStatementCommand command, CancellationToken ct)
    {
        using var memoryStream = new MemoryStream();
        await command.File.CopyToAsync(memoryStream, ct);
        var import = await importer.ImportAsync(memoryStream, command.File.FileName, ct);
        
        var (processedImport, unprocessedTransactions) = await processor.ProcessAsync(import, ct);

        return ApiResponse.Success();
    }
}