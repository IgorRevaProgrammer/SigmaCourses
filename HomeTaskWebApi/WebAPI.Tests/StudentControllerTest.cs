using Microsoft.AspNetCore.Authorization;
using Models;
using Models.Models;
using NSubstitute;
using Services;
using System;
using WebApi.Controllers;
using WebApi.ViewModels;
using Xunit;

namespace WebAPI.Tests
{
    public class StudentControllerTest
    {
        [Fact]
        public void Edit_ReturnsSuccess_WhenEmailIsChanged()
        {
            // Arrange
            var studentServiceMock = Substitute.For<StudentService>();
            var authMock = Substitute.For<IAuthorizationService>();
            StudentController sut = new StudentController(studentServiceMock, authMock, null);

            StudentVM studentVM = new StudentVM
            {
                Name = "test",
                BirthDate = DateTime.Now,
                Email = "test"
            };

            // Act
            var actual = sut.Edit(studentVM);

            // Assert
        }
    }
}
