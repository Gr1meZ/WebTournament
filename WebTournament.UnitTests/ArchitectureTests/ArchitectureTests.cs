using System.Reflection;
using AutoMapper;
using FluentAssertions;
using NetArchTest.Rules;
using WebTournament.Application;
using WebTournament.Application.Configuration.AutoMapper;
using WebTournament.Domain;
using WebTournament.Infrastructure.Data;
using WebTournament.Infrastructure.Identity;
using WebTournament.Infrastructure.IoC;
using WebTournament.Presentation.MVC;
using WebTournament.Presentation.MVC.AutoMapper;
using WebTournament.Presentation.MVC.Controllers;
using Xunit.Abstractions;

namespace WebTournament.UnitTests.ArchitectureTests;

public class ArchitectureTests
{
    private const string DomainNamespace = "WebTournament.Domain";
    private const string ApplicationNamespace = "WebTournament.Application";
    private const string InfrastructureDataNamespace = "WebTournament.Infrastructure.Data";
    private const string InfrastructureIoCNamespace = "WebTournament.Infrastructure.IoC";
    private const string InfrastructureIdentityNamespace = "WebTournament.Infrastructure.Identity";
    private const string PresentationNamespace = "WebTournament.Presentation.MVC";
    
    [Fact]
    public void Domain_Should_Not_HaveAnyDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(DomainAssembleReference).Assembly;
        
        var otherProjects = new[]
        {
            InfrastructureDataNamespace,
            ApplicationNamespace,
            InfrastructureIoCNamespace,
            InfrastructureIdentityNamespace,
            PresentationNamespace
        };
        
        //Act
        
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();
        
        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Application_Should_Not_HaveAnyDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(ApplicationAssembleReference).Assembly;
        var otherProjects = new[]
        {
            InfrastructureIoCNamespace,
            InfrastructureIdentityNamespace,
            InfrastructureDataNamespace,
            PresentationNamespace
        };
        
        //Act
        
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();
        
        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Presentation_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(PresentationAssembleReference).Assembly;
        //Act
        
        var otherProjects = new[]
        {
            DomainNamespace,
            InfrastructureDataNamespace
        };
        
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();
        
        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void InfrastructureIdentity_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(InfrastructureIdentityAssembleReference).Assembly;
        //Act
        
        var otherProjects = new[]
        {
            DomainNamespace,
            ApplicationNamespace,
            PresentationNamespace
        };
        
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();
        
        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void InfrastructureIoC_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(NativeInjector).Assembly;
        //Act
        
        var otherProjects = new[]
        {
            ApplicationNamespace,
            PresentationNamespace
        };
        
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();
        
        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void InfrastructureData_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(InfrastructureDataAssembleReference).Assembly;
        //Act
        
        var otherProjects = new[]
        {
            ApplicationNamespace,
            PresentationNamespace
        };
        
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();
        
        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Handlers_Should_Have_DependencyOnDomain()
    {
        //Arrange
        var assembly = typeof(ApplicationAssembleReference).Assembly;
        //Act
        
        var result = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();
        
        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Controllers_Should_Have_DependencyOnMediatR()
    {
        //Arrange
        var assembly = typeof(PresentationAssembleReference).Assembly;
        //Act
        
        var result = Types
            .InAssembly(assembly)
            .That()
            .Inherit(typeof(BaseController))
            .Should()
            .HaveDependencyOn("MediatR")
            .GetResult();
        
        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Controllers_Should_NotHave_DependencyOnRepositories()
    {
        //Arrange
        var assembly = typeof(PresentationAssembleReference).Assembly;
        //Act
        
        var result = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .ShouldNot()
            .HaveDependencyOn($"{InfrastructureDataNamespace}.Repository")
            .GetResult();
        
        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Repositories_Must_DependOnDomain()
    {
        //Arrange
        var assembly = typeof(InfrastructureDataAssembleReference).Assembly;
        //Act
        
        var result = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Repository")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();
        
        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
    
    
    [Fact]
    public void AutoMapper_ApplicationConfiguration_MustBe_Valid()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationProfile>());
        config.AssertConfigurationIsValid();
    }
    
    [Fact]
    public void AutoMapper_PresentationConfiguration_MustBe_Valid()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<PresentationProfile>());
        config.AssertConfigurationIsValid();
    }
}