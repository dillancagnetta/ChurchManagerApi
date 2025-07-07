using ChurchManager.Domain.Features.Finances.Banking;
using ChurchManager.Domain.Features.Finances.Extensions;
using ChurchManager.Domain.Features.Finances.Services;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ChurchManager.Features.Finances.Commands;

public record UploadBankStatementCommand(IFormFile File, bool IsDryRun = true) : IRequest<ApiResponse>;

public class UploadBankStatementHandler(
    IBankStatementImporter importer,
    IBankStatementProcessor processor,
    IGenericDbRepository<BankStatementImport> importDb) : IRequestHandler<UploadBankStatementCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(UploadBankStatementCommand command, CancellationToken ct)
    {
        using var memoryStream = new MemoryStream();
        await command.File.CopyToAsync(memoryStream, ct);
        var importOperation = await importer.ImportAsync(memoryStream, command.File.FileName, ct);

        if (!importOperation.IsSuccess) return ApiResponse.Failed("Failed to import bank statement. Please check the file format and try again.");
        
        var processedOperation = await processor.ProcessAsync(importOperation.Result, ct);
        var processedBankStatement = processedOperation.Result;
        
        // DRY RUN: No more processing required
        if (command.IsDryRun) return new ApiResponse(processedBankStatement.ToModel()) {Succeeded = processedOperation.IsSuccess};

        // Insert into database
        if (processedOperation.IsSuccess)
        {
            await importDb.AddAsync(processedBankStatement, ct);
            await importDb.SaveChangesAsync(ct);
        }
        
        return new ApiResponse(processedBankStatement.ToModel()){Succeeded = processedOperation.IsSuccess};
    }
}