using Application.Models;
using Application.UseCases;
using Common.Handlers;
using Common.Result;
using MediatR;

namespace Application.Handlers;

public class CreateItemHandler(IItemUseCase itemUseCase): IRequestHandler<CrateItemCommand,Result>
{
   

    public async Task<Result> Handle(CrateItemCommand request, CancellationToken cancellationToken)
    {
       return await itemUseCase.Create(request);
    }
}