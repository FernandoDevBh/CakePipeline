using CloudCustomers.API.Models;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CloudCustomers.UnitTests.Helpers;

public static class MockHttpMessageHandler<T>
{
  public static Mock<HttpMessageHandler> SetupBasicGetResourceList(List<T> expectResponse)
  {
    var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
    {
      Content = new StringContent(JsonConvert.SerializeObject(expectResponse))
    };

    mockResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

    var handlerMock = new Mock<HttpMessageHandler>();
    handlerMock
      .Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(mockResponse);

    return handlerMock;
  }

  internal static Mock<HttpMessageHandler> SetupBasicGetResourceList(List<User> expectResponse, string endpoint)
  {
    var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
    {
      Content = new StringContent(JsonConvert.SerializeObject(expectResponse))
    };

    mockResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

    var handlerMock = new Mock<HttpMessageHandler>();

    var httpRequestMethod = new HttpRequestMessage
    {
      RequestUri = new Uri(endpoint),
      Method = HttpMethod.Get,
    };

    handlerMock
      .Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(mockResponse);

    return handlerMock;
  }

  internal static Mock<HttpMessageHandler> SetupReturn404(List<User> expectResponse)
  {
    var mockResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
    {
      Content = new StringContent("")
    };

    mockResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

    var handlerMock = new Mock<HttpMessageHandler>();
    handlerMock
      .Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(mockResponse);

    return handlerMock;
  } 
}
