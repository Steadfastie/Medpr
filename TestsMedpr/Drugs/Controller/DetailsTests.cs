using MedprBusiness;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprModels.Responses;
using MedprWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NuGet.Protocol;

namespace TestsMedpr.Drugs.Controller;

public class DetailsTests
{
    private readonly Mock<IDrugService> _drugServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IOpenFDAService> _openFDAmock;

    public DetailsTests()
    {
        _drugServiceMock = new Mock<IDrugService>();
        _mapperMock = new Mock<IMapper>();
        _openFDAmock = new Mock<IOpenFDAService>();
    }

    [Theory]
    [InlineData("52F17FA5-BD77-42B4-B19A-561F725EB1E1")]
    public async Task Details_ReturnsOk_WhenIdValid(Guid id)
    {
        // Arrange
        _drugServiceMock.Setup(s => s.GetDrugByIdAsync(id)).ReturnsAsync(new DrugDTO());
        _mapperMock.Setup(m => m
            .Map<DrugModelResponse>(It.IsAny<DrugDTO>()))
                .Returns(() => new DrugModelResponse());
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Details(id);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(200, ((OkObjectResult)actionResult).StatusCode);
        Assert.NotNull(((OkObjectResult)actionResult).Value);
    }

    [Theory]
    [InlineData("52F17FA5-BD77-42B4-B19A-561F725EB1E1")]
    public async Task Details_Returns404_WhenIdInvalid(Guid id)
    {
        // Arrange
        _drugServiceMock.Setup(s => s.GetDrugByIdAsync(id)).ReturnsAsync(null as DrugDTO);
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Details(id);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<NotFoundResult>(actionResult);
        Assert.Equal(404, ((NotFoundResult)actionResult).StatusCode);
    }

    [Theory]
    [InlineData("52F17FA5-BD77-42B4-B19A-561F725EB1E1")]
    public async Task DetailsReturns500StatusCodeIfSomethingIsOff(Guid id)
    {
        // Arrange
        _drugServiceMock.Setup(s => s.GetDrugByIdAsync(id)).ThrowsAsync(new Exception());
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Details(id) as ObjectResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<ProblemDetails>(actionResult.Value);
        Assert.Equal(500, ((ProblemDetails)actionResult.Value).Status);
        Assert.NotNull(((ProblemDetails)actionResult.Value).Detail);
    }
}