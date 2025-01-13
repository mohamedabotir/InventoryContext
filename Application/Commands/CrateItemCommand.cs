using Application.Dtos;
using Common.Result;
using MediatR;

namespace Application.Models;

public abstract record  CrateItemCommand(Guid Guid,string Name, string Description,decimal Price,List<ItemStockDto> Stocks):IRequest<Result>;