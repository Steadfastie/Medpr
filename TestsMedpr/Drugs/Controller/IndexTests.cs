using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprModels.Responses;
using MedprWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace TestsMedpr.Drugs.Controller;

public class IndexTests
{
    private readonly Mock<IDrugService> _drugServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IOpenFDAService> _openFDAmock;

    public IndexTests()
    {
        _drugServiceMock = new Mock<IDrugService>();
        _mapperMock = new Mock<IMapper>();
        _openFDAmock = new Mock<IOpenFDAService>();
    }

    [Fact]
    public async Task Index_ReturnsOk_WithEmptyResponse()
    {
        // Arrange
        _drugServiceMock.Setup(s => s.GetAllDrugsAsync()).ReturnsAsync(new List<DrugDTO>());
        _mapperMock.Setup(m => m
            .Map<List<DrugModelResponse>>(It.IsAny<List<DrugDTO>>()))
                .Returns(() => new List<DrugModelResponse>());
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Index();

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(200, ((OkObjectResult)actionResult).StatusCode);
        Assert.Null(((OkObjectResult)actionResult).Value);
    }

    [Fact]
    public async Task Index_ReturnsOk_WithSomeResponse()
    {
        // Arrange
        _drugServiceMock.Setup(s => s.GetAllDrugsAsync()).ReturnsAsync(new List<DrugDTO>()
        {
            new DrugDTO(),
            new DrugDTO()
        });
        _mapperMock.Setup(m => m
            .Map<List<DrugModelResponse>>(It.IsAny<List<DrugDTO>>()))
                .Returns(() => new List<DrugModelResponse>()
                {
                    new DrugModelResponse(),
                    new DrugModelResponse()
                });
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Index();
        var okResult = actionResult as OkObjectResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(200, ((OkObjectResult)actionResult).StatusCode);
        Assert.NotNull(((OkObjectResult)actionResult).Value);
    }

    [Fact]
    public async Task Index_Returns500_IfSomethingOff()
    {
        // Arrange
        _drugServiceMock.Setup(s => s.GetAllDrugsAsync()).ThrowsAsync(new Exception());
        _mapperMock.Setup(m => m
            .Map<List<DrugModelResponse>>(It.IsAny<List<DrugDTO>>()))
                .Returns(() => new List<DrugModelResponse>());
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Index() as ObjectResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<ProblemDetails>(actionResult.Value);
        Assert.Equal(500, ((ProblemDetails)actionResult.Value).Status);
        Assert.NotNull(((ProblemDetails)actionResult.Value).Detail);
    }
}