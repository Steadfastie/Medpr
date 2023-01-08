using MedprBusiness;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprModels.Requests;
using MedprModels.Responses;
using MedprWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NuGet.Protocol;

namespace TestsMedpr.Drugs.Controller;

public class CreateTests
{
    private readonly Mock<IDrugService> _drugServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IOpenFDAService> _openFDAmock;

    public CreateTests()
    {
        _drugServiceMock = new Mock<IDrugService>();
        _mapperMock = new Mock<IMapper>();
        _openFDAmock = new Mock<IOpenFDAService>();
    }

    [Theory]
    [MemberData(nameof(GetDrugs), parameters: true)]
    public async Task Create_ReturnsOk_OnValid(DrugModelRequest model)
    {
        // Arrange
        _drugServiceMock.Setup(s => s.GetDrugByNameAsync(model.Name)).ReturnsAsync(null as DrugDTO);
        _mapperMock.Setup(m => m
            .Map<DrugDTO>(It.IsAny<DrugModelRequest>()))
                .Returns(() => new DrugDTO());
        _drugServiceMock.Setup(s => s.CreateDrugAsync(It.IsAny<DrugDTO>()));
        _mapperMock.Setup(m => m
           .Map<DrugModelResponse>(It.IsAny<DrugDTO>()))
               .Returns(() => new DrugModelResponse());
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Create(model);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<CreatedAtActionResult>(actionResult);
        Assert.Equal(201, ((CreatedAtActionResult)actionResult).StatusCode);
        Assert.NotNull(((CreatedAtActionResult)actionResult).Value);
        Assert.IsType<DrugModelResponse>(((CreatedAtActionResult)actionResult).Value);
    }

    [Theory]
    [MemberData(nameof(GetDrugs), parameters: true)]
    public async Task Create_ForbidsDuplication(DrugModelRequest model)
    {
        // Arrange
        _drugServiceMock.Setup(s => s.GetDrugByNameAsync(model.Name)).ReturnsAsync(new DrugDTO());
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);

        // Act
        var actionResult = await controller.Create(model);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<ForbidResult>(actionResult);
    }

    [Theory]
    [MemberData(nameof(GetDrugs), parameters: false)]
    public async Task Create_ReturnsOk_WhenModelStateInvalid(DrugModelRequest model)
    {
        // Arrange
        var controller = new DrugsController(_drugServiceMock.Object, _mapperMock.Object, _openFDAmock.Object);
        controller.ModelState.AddModelError("Error", "Invalid input model");

        // Act
        var actionResult = await controller.Create(model);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(200, ((OkObjectResult)actionResult).StatusCode);
        Assert.NotNull(((OkObjectResult)actionResult).Value);
        Assert.IsType<DrugModelRequest>(((OkObjectResult)actionResult).Value);
        Assert.Equal(model, ((OkObjectResult)actionResult).Value);
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