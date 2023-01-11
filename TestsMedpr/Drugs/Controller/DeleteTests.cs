using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprModels.Requests;
using MedprModels.Responses;
using MedprWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace TestsMedpr.Drugs.Controller;

public class DeleteTests
{
    private readonly Mock<IDrugService> _drugServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IOpenFDAService> _openFDAmock;

    public DeleteTests()
    {
        _drugServiceMock = new Mock<IDrugService>();
        _mapperMock = new Mock<IMapper>();
        _openFDAmock = new Mock<IOpenFDAService>();
    }

    [Theory]
    [InlineData("52F17FA5-BD77-42B4-B19A-561F725EB1E1")]
    public async Task Delete_ReturnsOk_OnValid(Guid id)
    {
        // Arrange
        _drugServiceMock.Setup(s => s.GetDrugByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new DrugDTO());
        _drugServiceMock.Setup(s => s.DeleteDrugAsync(It.IsAny<DrugDTO>()));
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Delete(id);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<OkResult>(actionResult);
        Assert.Equal(200, ((OkResult)actionResult).StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsBadRequest_OnEmptyGuid()
    {
        // Arrange
        var emptyGuid = Guid.Empty;
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Delete(emptyGuid);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<BadRequestResult>(actionResult);
    }

    [Theory]
    [InlineData("52F17FA5-BD77-42B4-B19A-561F725EB1E1")]
    public async Task Delete_Returns400_WhenDrugMissing(Guid id)
    {
        // Arrange
        _drugServiceMock.Setup(s => s.GetDrugByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as DrugDTO);
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Delete(id);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<BadRequestResult>(actionResult);
        Assert.Equal(400, ((BadRequestResult)actionResult).StatusCode);
    }

    [Theory]
    [InlineData("52F17FA5-BD77-42B4-B19A-561F725EB1E1")]
    public async Task Delete_Returns500_IfSomethingIsOff(Guid id)
    {
        // Arrange
        _drugServiceMock.Setup(s => s.GetDrugByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception());
        _drugServiceMock.Setup(s => s.DeleteDrugAsync(It.IsAny<DrugDTO>())).ThrowsAsync(new Exception());
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Delete(id) as ObjectResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<ProblemDetails>(actionResult.Value);
        Assert.Equal(500, ((ProblemDetails)actionResult.Value).Status);
        Assert.NotNull(((ProblemDetails)actionResult.Value).Detail);
    }
}