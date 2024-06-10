using System.Runtime.CompilerServices;
using Fina.Api.Data.Mappings;
using Fina.Core.Common;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers
{
	public class TransactionHandler(AppDbContext context) : ITransactionHandler
	{
		public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
		{
			if (request is { Type: Core.Enums.ETransactionType.Withdraw, Amount: >= 0})
			{
				request.Amount *= -1;
			}
			
			try
			{
				var transaction = new Transaction
				{
					UserId = request.UserId,
					CategoryId = request.CategoryId,
					CreatedAt = DateTime.Now,
					Amount = request.Amount,
					PaidOrReceivedAt = request.PaidOrReceivedAt,
					Title = request.Title,
					Type = request.Type
				};
				
				await context.Transactions.AddAsync(transaction);
				await context.SaveChangesAsync();
				
				return new Response<Transaction?>(transaction, 201, "Transação criada com sucesso");
			}
			catch (Exception ex)
			{
				return new Response<Transaction?>(null, 500, ex.Message);		
			}
		}

		public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
		{
			if (request is {Type: Fina.Core.Enums.ETransactionType.Withdraw, Amount: >= 0 })
			{
				request.Amount *= -1;
			}
			
			try
			{
				var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
				
				if (transaction is null)
				{
					return new Response<Transaction?>(null, 404, "Transação não encontrada");
				}
				
				transaction.CategoryId = request.CategoryId;
				transaction.Amount = request.Amount;
				transaction.Title = request.Title;
				transaction.Type = request.Type;
				transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;
				
				context.Transactions.Update(transaction);
				await context.SaveChangesAsync();
				
				return new Response<Transaction?>(transaction);
			}
			catch (Exception ex)
			{
				return new Response<Transaction?>(null, 500, ex.Message);	
			}
		}
		
		public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
		{
			try
			{
				var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
				
				if (transaction is null){
					return new Response<Transaction?>(null, 404, "Transação não encontrada");
				}
				
				context.Transactions.Remove(transaction);
				await context.SaveChangesAsync();
				
				return new Response<Transaction?>(transaction);
			}
			catch (Exception ex)
			{
				return new Response<Transaction?>(null, 500, ex.Message);
			}
		}

		public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
		{
			var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
			
			return transaction is null 
				? new Response<Transaction?>(null, 404, "Transação não encontrada")
				: new Response<Transaction?>(transaction);
		}

		public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
		{
			try
			{
				request.StartDate ??= DateTime.Now.GetFirstDay();
				request.EndDate   ??= DateTime.Now.GetLastDay();
			}
			catch (Exception ex)
			{
				return new PagedResponse<List<Transaction>?>(null, 500, ex.Message);
			}
			
			try
			{
				var query = context
					.Transactions
					.AsNoTracking()
					.Where(x => 
						x.PaidOrReceivedAt >= request.StartDate && 
						x.PaidOrReceivedAt >= request.EndDate &&
						x.UserId == request.UserId)
						.OrderBy(x => x.PaidOrReceivedAt);
						
				var transaction = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
				
				int count = await query.CountAsync();
				
				return new PagedResponse<List<Transaction>?>(transaction, count, request.PageNumber, request.PageSize);
			}
			catch (Exception ex)
			{
				return new PagedResponse<List<Transaction>?>(null, 500, ex.Message);
			}
			
		}

	}
}