

using Application.Dtos;
using Application.Models;
using Application.UseCases;
using Common.Repository;
using Common.ValueObject;
using Domain.Entities;
using Domain.Repository;
using Moq;

[TestFixture]
public class ItemUseCaseTests
{
    private Mock<IItemRepository> _itemRepoMock;
    private Mock<IUnitOfWork<Item>> _unitOfWorkMock;
    private IItemUseCase _useCase;

    [SetUp]
    public void Setup()
    {
        _itemRepoMock = new Mock<IItemRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork<Item>>();
        _useCase = new ItemUseCase(_itemRepoMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Create_ShouldSucceed_WhenDataIsValid()
    {
        var command = new CreateItemCommand(Guid.NewGuid(), "Valid Item 1000", "Description for Test item1 XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", 120M, new List<ItemStockDto>
            {
                new(Guid.NewGuid(),"Warehouse A",Quantity.CreateInstance(10,QuantityType.Tab).Value)
            });

        _itemRepoMock.Setup(r => r.AddAsync(It.IsAny<Item>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<Item>(),CancellationToken.None)).ReturnsAsync(1);

        var result = await _useCase.Create(command);

        Assert.IsTrue(result.IsSuccess);
        _itemRepoMock.Verify(r => r.AddAsync(It.IsAny<Item>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<Item>(), CancellationToken.None), Times.Once);
    }

    [Test]
    public async Task Create_ShouldFail_WhenNameIsInvalid()
    {
        var command = new CreateItemCommand(Guid.NewGuid(), "", "A good description", 120M, new List<ItemStockDto>
            {
                new(Guid.NewGuid(),"Warehouse A",Quantity.CreateInstance(10,QuantityType.Tab).Value)
            });

        var result = await _useCase.Create(command);

        Assert.IsTrue(result.IsFailure);
        _itemRepoMock.Verify(r => r.AddAsync(It.IsAny<Item>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<Item>(), CancellationToken.None), Times.Never);
    }

    [Test]
    public async Task Create_ShouldFail_WhenPriceIsInvalid()
    {
        var command = new CreateItemCommand(Guid.NewGuid(), "Valid Item", "A good description", -120M, new List<ItemStockDto>
            {
                new(Guid.NewGuid(),"Warehouse A",Quantity.CreateInstance(10,QuantityType.Tab).Value)
            });

        var result = await _useCase.Create(command);

        Assert.IsTrue(result.IsFailure);
        _itemRepoMock.Verify(r => r.AddAsync(It.IsAny<Item>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<Item>(), CancellationToken.None), Times.Never);
    }
}
