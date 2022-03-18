using CloudCustomers.API.Models;
using System.Collections.Generic;

namespace CloudCustomers.UnitTests.Fixtures
{
  public static class UsersFixture
  {
    public static List<User> GetTestUsers() =>
      new List<User>
      {
        new()
        {
          Id = 1,
          Email = "teste@teste.com",
          Name = "userteste",
          Address = new Address
          {
            City = "Madison",
            Street = "123 Main St",
            ZipCode = "53704"
          }
        },
        new()
        {
          Id = 2,
          Email = "teste2@teste.com",
          Name = "userteste2",
          Address = new Address
          {
            City = "Madison",
            Street = "452 Main St",
            ZipCode = "53704"
          }
        },
        new()
        {
          Id = 3,
          Email = "teste3@teste.com",
          Name = "userteste3",
          Address = new Address
          {
            City = "Madison",
            Street = "120 Main St",
            ZipCode = "53704"
          }
        }
      };
  }
}
