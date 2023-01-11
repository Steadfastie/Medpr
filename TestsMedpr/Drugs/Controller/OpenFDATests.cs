using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprModels.Requests;
using MedprModels.Responses;
using MedprWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace TestsMedpr.Drugs.Controller;

public class OpenFDATests
{
    private readonly Mock<IDrugService> _drugServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IOpenFDAService> _openFDAmock;

    public OpenFDATests()
    {
        _drugServiceMock = new Mock<IDrugService>();
        _mapperMock = new Mock<IMapper>();
        _openFDAmock = new Mock<IOpenFDAService>();
    }

    [Fact]
    public async Task OpenFDA_ReturnsOk_OnValid()
    {
        // Arrange
        _openFDAmock.Setup(s => s.GetRandomDrug()).ReturnsAsync(new DrugDTO());
        _mapperMock.Setup(m => m
            .Map<RandomDrugModel>(It.IsAny<DrugDTO>()))
                .Returns(() => new RandomDrugModel());
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.GetOpenFDA();

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(200, ((OkObjectResult)actionResult).StatusCode);
        Assert.NotNull(((OkObjectResult)actionResult).Value);
        Assert.IsType<RandomDrugModel>(((OkObjectResult)actionResult).Value);
    }

    [Fact]
    public async Task OpenFDA_Returns400_WhenDrugNotFound()
    {
        // Arrange
        _openFDAmock.Setup(s => s.GetRandomDrug()).ReturnsAsync(null as DrugDTO);
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.GetOpenFDA();

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<NotFoundResult>(actionResult);
        Assert.Equal(404, ((NotFoundResult)actionResult).StatusCode);
    }

    [Fact]
    public async Task OpenFDA_Returns500_IfSomethingIsOff()
    {
        // Arrange
        _openFDAmock.Setup(s => s.GetRandomDrug()).ThrowsAsync(new Exception());
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.GetOpenFDA() as ObjectResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<ProblemDetails>(actionResult.Value);
        Assert.Equal(500, ((ProblemDetails)actionResult.Value).Status);
        Assert.NotNull(((ProblemDetails)actionResult.Value).Detail);
    }
}