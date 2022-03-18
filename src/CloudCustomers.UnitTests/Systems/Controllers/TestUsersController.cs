using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CloudCustomers.API.Controllers;
using Moq;
using CloudCustomers.API.Services;
using System.Collections.Generic;
using CloudCustomers.API.Models;
using CloudCustomers.UnitTests.Fixtures;

namespace CloudCustomers.UnitTests.Systems.Controllers;

public class TestUsersController
{
  [Fact]
  public async Task Get_OnSuccess_ReturnsStatusCode200()
  {
    // Arrange
    var mockUsersService = new Mock<IUserService>();
    mockUsersService
      .Setup(service => service.GetAllUsers())
      .ReturnsAsync(UsersFixture.GetTestUsers());
    var sut = new UsersController(mockUsersService.Object);

    // Act
    var response = (OkObjectResult)await sut.Get();

    // Assert
    response.StatusCode.Should().Be(200);
  }

  [Fact]
  public async Task Get_OnSuccess_InvokesUserServiceExactlyOnce()
  {
    // Arrange
    var mockUsersService = new Mock<IUserService>();
    mockUsersService
      .Setup(service => service.GetAllUsers())
      .ReturnsAsync(UsersFixture.GetTestUsers());

    var sut = new UsersController(mockUsersService.Object);

    // Act    
    var result = await sut.Get();

    // Assert
    mockUsersService.Verify(service => service.GetAllUsers(), Times.Once());      
  }

  [Fact]
  public async Task Get_OnSuccess_ReturnsListOfUsers()
  {
    // Arrange
    var mockUsersService = new Mock<IUserService>();
    mockUsersService
      .Setup(service => service.GetAllUsers())
      .ReturnsAsync(UsersFixture.GetTestUsers());

    var sut = new UsersController(mockUsersService.Object);

    // Act        
    var result = await sut.Get();

    // Assert
    result.Should().BeOfType<OkObjectResult>();
    var objectResult = (OkObjectResult)result;
    objectResult.Value.Should().BeOfType<List<User>>();
  }

  [Fact]
  public async Task Get_OnNoUsersFound_Returns404()
  {
    // Arrange
    var mockUsersService = new Mock<IUserService>();
    mockUsersService
      .Setup(service => service.GetAllUsers())
      .ReturnsAsync(new List<User>());

    var sut = new UsersController(mockUsersService.Object);

    // Act        
    var result = await sut.Get();

    // Assert
    result.Should().BeOfType<NotFoundResult>();
    var objectResult = (NotFoundResult)result;
    objectResult.StatusCode.Should().Be(404);
  }
}