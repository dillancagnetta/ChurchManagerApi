using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.Finances.Banking;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Finances.Extensions;

public static class FinanceModelExtensions
{
    public static BankStatementViewModel? ToModel(this BankStatementImport? model)
    {
        if (model == null) return null;

        return new BankStatementViewModel
        {
            Id = model.Id,
            FileName = model.FileName,
            BankAccount = model.BankAccount,
            ImportDate = model.ImportDate,
            Currency = model.Currency.Value,
            TransactionCount = model.Transactions.Count,
            ProcessedCount = model.ProcessedCount(),
            UnProcessedCount = model.UnProcessedCount(),
            ErrorCount = model.ErrorCount,
            IsCompleted = model.IsCompleted,
            StatementStartDate = model.StatementStartDate,
            StatementEndDate = model.StatementEndDate,
            Transactions = model.Transactions.Select(t => t.ToModel()).ToList(),
            Givings = model.Givings.Select(t => t.ToModel()).ToList()
        };
    }
    
    public static TransactionViewModel? ToModel(this Transaction? model)
    {
        if (model == null) return null;

        return new TransactionViewModel
        {
            Id = model.Id,
            ImportId = model.ImportId,
            OriginalReference = model.OriginalReference,
            Amount = model.TransactionAmount.ToModel()!,
            TransactionDate = model.TransactionDate,
            BankTransactionId = model.BankTransactionId,
            TransactionType = model.TransactionType,
            IsMatched = model.IsMatched,
            IsProcessed = model.IsProcessed,
            IsResolved = model.IsResolved,
            Memo = model.Memo,
            Error = model.Error,
        };
    }
    
    public static GivingViewModel? ToModel(this Giving? model)
    {
        if (model == null) return null;

        return new GivingViewModel
        {
            Id = model.Id,
            Date = model.Date,
            Amount = model.GivingAmount.ToModel()!,
            Benefactor = model.Benefactor?.ToModel(),
            PaymentMethod = model.PaymentMethod.Value,
            GivingType = model.GivingType.Value,
            Notes = model.Notes,
            ReceiptSent = model.ReceiptSent,
            ParsedReference = model.ParsedReference,
            BankTransactionId = model.BankTransactionId,
            FundId = model.FundId,
            Fund = model.Fund?.ToModel()
        };
    }
    
    
    public static FundViewModel? ToModel(this Fund? model)
    {
        if (model == null) return null;

        return new FundViewModel
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Code = model.Code,
            FundType = model.FundType.Value,
        };
    }
    
    public static BenefactorViewModel? ToModel(this Benefactor? model)
    {
        if (model == null) return null;

        return new BenefactorViewModel
        {
            Id = model.Id,
            Name = model.Name,
            Type = model.Type.Value,
            // Reference IDs to original entities
            PersonId = model.PersonId,
            FamilyId = model.FamilyId,
            GroupId = model.GroupId,
            ChurchId = model.ChurchId,
        };
    }
}