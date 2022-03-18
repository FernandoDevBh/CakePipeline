#addin nuget:?package=Cake.Docker&version=1.1.1

var target = Argument("target", "Docker Up");
var configuration = Argument("configuration", "Release");
var solutionFolder = "./CloudCustomers.sln";
var outputFolder = "./artifacts";
var testProjects = "**/*.csproj";

Information($"Running target {target} in configuration {configuration}");

Setup(ctx =>
{
   // Esse bloco é executado antes da primeira TASK
   Information("Executando Tarefas...");
});

Teardown(ctx =>
{
   // Esse bloco é executado após da última TASK
   Information("Execução de Tarefas finalizado.");
});
Task("Docker Down")
    .Does(() => {
        DockerComposeDown();
    });

Task("Clean")
    .IsDependentOn("Docker Down")
    .Does(() => {
        CleanDirectory(outputFolder);
    });

Task("Restore")
    .Does(() => {
        DotNetRestore(solutionFolder);
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => {
        DotNetCoreBuild(solutionFolder, new DotNetCoreBuildSettings {
           NoRestore = true,
           Configuration = configuration 
        });
    });


Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetCoreTest(solutionFolder, new DotNetCoreTestSettings
        {
            NoRestore = true,
            Configuration = configuration,
            NoBuild = true   
        });
    });

Task("Publish")
    .IsDependentOn("Clean")
    .IsDependentOn("Test")
    .Does(() =>{
        DotNetCorePublish(solutionFolder, new DotNetCorePublishSettings
        {
            NoRestore = true,
            Configuration = configuration,
            NoBuild = true,
            OutputDirectory = outputFolder
        });
    });

Task("Docker Up")
    .IsDependentOn("Publish")
    .Does(() => {
        DockerComposeUp();    
    });

RunTarget(target);