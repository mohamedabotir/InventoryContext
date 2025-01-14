using Application.Models;
using Common.Result;
using Domain.Entities;

namespace Application.UseCases;

public interface IItemUseCase
{
    Task<Result> Create(CreateItemCommand item);
}