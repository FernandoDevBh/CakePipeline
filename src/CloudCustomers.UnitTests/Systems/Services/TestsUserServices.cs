using CloudCustomers.API.Models;
using CloudCustomers.API.Services;
using CloudCustomers.UnitTests.Fixtures;
using CloudCustomers.UnitTests.Helpers;
using Microsoft.Extensions.Options;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using CloudCustomers.API.Config;
using System;

namespace CloudCustomers.UnitTests.Systems.Services;

public class TestsUserServices
{

  [Fact]
  public async Task GetAllUsers_WhenCalled_InvokesHttpGetResponse()
  {
    // Arrange
    var expectResponse = UsersFixture.GetTestUsers();
    var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectResponse);
    var httpClient = new HttpClient(handlerMock.Object);
    var endpoint = "https://example.com/users";
    var config = Options.Create(new UsersApiOptions
    {
      EndPoint = endpoint
    });
    var sut = new UserService(httpClient, config);

    // Act
    await sut.GetAllUsers();

    // Assert
    // Verify HTTP request is model
    handlerMock
      .Protected()
      .Verify(
      "SendAsync",
      Times.Exactly(1),
      ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
      ItExpr.IsAny<CancellationToken>());
  }

  [Fact]
  public async Task GetAllUsers_WhenHits404_ReturnEmptyList()
  {
    // Arrange
    var expectResponse = UsersFixture.GetTestUsers();
    var handlerMock = MockHttpMessageHandler<User>.SetupReturn404(expectResponse);
    var httpClient = new HttpClient(handlerMock.Object);
    var endpoint = "https://example.com/users";
    var config = Options.Create(new UsersApiOptions
    {
      EndPoint = endpoint
    });
    var sut = new UserService(httpClient, config);


    // Act
    var result = await sut.GetAllUsers();

    // Assert    
    result.Count.Should().Be(0);
  }

  [Fact]
  public async Task GetAllUsers_WhenCalled_ReturnListOfUsersOfExpectedSize()
  {
    // Arrange
    var expectResponse = UsersFixture.GetTestUsers();
    var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectResponse);
    var httpClient = new HttpClient(handlerMock.Object);
    var endpoint = "https://example.com/users";
    var config = Options.Create(new UsersApiOptions
    {
      EndPoint = endpoint
    });
    var sut = new UserService(httpClient, config);


    // Act
    var result = await sut.GetAllUsers();

    // Assert    
    result.Count.Should().Be(expectResponse.Count);
  }

  [Fact]
  public async Task GetAllUsers_WhenCalled_InvokesConfiguredExternalUrl()
  {
    // Arrange
    var expectResponse = UsersFixture.GetTestUsers();
    var endpoint = "https://example.com/users";
    var handlerMock = MockHttpMessageHandler<User>
                        .SetupBasicGetResourceList(expectResponse, endpoint);
    var httpClient = new HttpClient(handlerMock.Object);    
    var config = Options.Create(new UsersApiOptions
    {
      EndPoint = endpoint
    });

    var sut = new UserService(httpClient, config);

    // Act
    var result = await sut.GetAllUsers();

    var uri = new Uri(endpoint);

    // Assert    
    handlerMock
      .Protected()
      .Verify(
      "SendAsync",
      Times.Exactly(1),
      ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == uri),
      ItExpr.IsAny<CancellationToken>());
  }
}

