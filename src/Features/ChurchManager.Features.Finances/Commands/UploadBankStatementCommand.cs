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
        var importOperation = await importer.ImportAsync(memoryStream, command.File.FileName, ct);

        if (!importOperation.IsSuccess) return ApiResponse.FromOperation(importOperation);
        
        var processedOperation = await processor.ProcessAsync(importOperation.Result, ct);
        
        //if (!processedOperation.IsSuccess) return ApiResponse.FromOperation(importOperation);

        return ApiResponse.FromOperation(processedOperation);
    }
}