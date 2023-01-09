using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprModels.Requests;
using MedprModels.Responses;
using MedprWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace TestsMedpr.Drugs.Controller;

public class EditTests
{
    private readonly Mock<IDrugService> _drugServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IOpenFDAService> _openFDAmock;

    public EditTests()
    {
        _drugServiceMock = new Mock<IDrugService>();
        _mapperMock = new Mock<IMapper>();
        _openFDAmock = new Mock<IOpenFDAService>();
    }

    [Theory]
    [MemberData(nameof(GetDrugs), parameters: true)]
    public async Task Edit_ReturnsOk_OnValid(DrugModelRequest model)
    {
        // Arrange
        _drugServiceMock.Setup(s => s.GetDrugByIdAsync(model.Id)).ReturnsAsync(new DrugDTO()
        {
            Id= model.Id,
            Name = model.Name,
        });
        _mapperMock.Setup(m => m
            .Map<DrugDTO>(It.IsAny<DrugModelRequest>()))
                .Returns(() => new DrugDTO()
                {
                    Id = model.Id,
                    Name = model.Name,
                });
        _drugServiceMock.Setup(s => s.PatchDrugAsync(It.IsAny<Guid>(), It.IsAny<List<PatchModel>>()));
        _mapperMock.Setup(m => m
           .Map<DrugModelResponse>(It.IsAny<DrugDTO>()))
               .Returns(() => new DrugModelResponse());
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Edit(model);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(201, ((OkObjectResult)actionResult).StatusCode);
        Assert.NotNull(((OkObjectResult)actionResult).Value);
        Assert.IsType<DrugModelResponse>(((OkObjectResult)actionResult).Value);
    }

    // TODO: Refactor to name checking <- name changing should be forbidden
    // TODO: Add test for NotModified
    [Theory]
    [MemberData(nameof(GetDrugs), parameters: true)]
    public async Task Edit_ForbidsDuplication(DrugModelRequest model)
    {
        // Arrange
        _drugServiceMock.Setup(s => s.GetDrugByNameAsync(model.Name)).ReturnsAsync(new DrugDTO());
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Edit(model);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<ForbidResult>(actionResult);
    }

    [Theory]
    [MemberData(nameof(GetDrugs), parameters: false)]
    public async Task Edit_ReturnsOk_WhenModelStateInvalid(DrugModelRequest model)
    {
        // Arrange
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);
        controller.ModelState.AddModelError("Error", "Invalid input model");

        // Act
        var actionResult = await controller.Edit(model);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(200, ((OkObjectResult)actionResult).StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetDrugs), parameters: false)]
    public async Task Edit_Returns500_IfSomethingIsOff(DrugModelRequest model)
    {
        // Arrange
        _drugServiceMock.Setup(s => s.GetDrugByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception());
        _mapperMock.Setup(m => m
            .Map<DrugDTO>(It.IsAny<DrugModelRequest>()))
                .Returns(() => new DrugDTO());
        _drugServiceMock.Setup(s => s.PatchDrugAsync(It.IsAny<Guid>(), It.IsAny<List<PatchModel>>())).ThrowsAsync(new Exception());
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Edit(model) as ObjectResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<ProblemDetails>(actionResult.Value);
        Assert.Equal(500, ((ProblemDetails)actionResult.Value).Status);
        Assert.NotNull(((ProblemDetails)actionResult.Value).Detail);
    }

    /// <summary>
    ///     <returns>Returns drugs for tests. </returns>
    ///     <paramref name="valid"/> <param name="valid">Determine what to return. </param>
    /// </summary>
    public static IEnumerable<object[]> GetDrugs(bool valid)
    {
        var allDrugs = new List<object[]>()
        {
            new object[]
            {
                new DrugModelRequest()
                {
                    Id = Guid.NewGuid(),
                    Name = "Valid Drug",
                    PharmGroup = "Valid PharmGroup",
                    Price = 5
                },
            },
            new object[]
            {
                new DrugModelRequest()
                {
                    Id = Guid.NewGuid(),
                    Name = "Invalid Drug",
                    PharmGroup = "",
                    Price = 5
                },
            }
        };

        return valid ? allDrugs.Take(1) : allDrugs.TakeLast(1);
    }
}