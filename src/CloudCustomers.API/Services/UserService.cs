using CloudCustomers.API.Config;
using CloudCustomers.API.Models;
using Microsoft.Extensions.Options;
using System.Net;

namespace CloudCustomers.API.Services;

public interface IUserService
{
  Task<List<User>> GetAllUsers();
}
public class UserService : IUserService
{
  private readonly HttpClient _htppClient;
  private readonly UsersApiOptions _apiConfig;

  public UserService(HttpClient htppClient, IOptions<UsersApiOptions> apiConfig)
  {
    _htppClient = htppClient;
    _apiConfig = apiConfig.Value;
  }

  public async Task<List<User>> GetAllUsers()
  {
    var usersResponse = await _htppClient.GetAsync(_apiConfig.EndPoint);
    if(usersResponse.StatusCode == HttpStatusCode.NotFound)
    {
      return new List<User> { };
    }
    var  responseContent = usersResponse.Content;
    var allUsers = await responseContent.ReadFromJsonAsync<List<User>>();

    return allUsers.ToList();
  }
}
