#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY "/artifacts" /app
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "CloudCustomers.API.dll"]