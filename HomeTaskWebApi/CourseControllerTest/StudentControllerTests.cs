using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using NSubstitute;
using Services;
using WebApi.Controllers;
using WebApi.ViewModels;
using Xunit;
using FluentAssertions;
using System;
using Services.Validators;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApiTest
{
    public class StudentControllerTests
    {
        [Fact]
        public async Task GetStudents_ReturnsViewResult_WithListOfStudents()
        {
            // Arrange
            var studentService = Substitute.For<StudentService>();
            studentService.GetAllStudents().Returns(GetStudentList());
            var controller = new StudentController(studentService, null, GetLog());

            // Act
            var result = await controller.GetStudents();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<StudentVM>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void CreateGet_ReturnViewResult_WithViewModel()
        {
            //Arrange
            var sut = new StudentController(null, null, GetLog());

            //Act
            var actual = sut.Create();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            Assert.IsAssignableFrom<StudentVM>(viewResult.Model);
            Assert.Equal("Student", viewResult.ViewName);
            Assert.Equal("Create", viewResult.ViewData["Action"]);
        }

        [Fact]
        public async Task CreatePost_ReturnsBadRequest_WhenStudentVMIsNull()
        {
            //Arrange
            var sut = new StudentController(null, null, GetLog());

            //Act
            var actual = await sut.Create(null);

            //Assert
            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public async Task CreatePost_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new StudentController(null, null, GetLog());
            controller.ModelState.AddModelError("A", "B");

            // Act
            var result = await controller.Create(new StudentVM());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<StudentVM>(viewResult.Model);
            Assert.Equal("Student", viewResult.ViewName);
            Assert.Equal("Create", viewResult.ViewData["Action"]);
        }

        [Fact]
        public async Task CreatePost_RedirectsToStudentsAndCreatesStudent_WhenRequestIsValid()
        {
            //Arrange
            var studentVM = new StudentVM()
            {
                Id = 1,
                Email = "email",
                BirthDate = new DateTime(2020, 2, 2),
                Name = "Test"
            };

            var studentService = Substitute.For<StudentService>();
            studentService.CreateStudent(Arg.Is<Student>(s =>
            s.Id == studentVM.Id &&
            s.BirthDate == studentVM.BirthDate &&
            s.Name == studentVM.Name &&
            s.Email == studentVM.Email))
                .Returns(new ValidationResponse<Student>(new Student()));

            var sut = new StudentController(studentService, null, GetLog());

            //Act
            var actual = await sut.Create(studentVM);

            //Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            studentService.Received(1);
        }

        [Fact]
        public async Task EditGet_ReturnNotFoundResult_WhenStudentIdIncorrect()
        {
            //Arrange
            int studentId = 0;
            var studentService = Substitute.For<StudentService>();
            studentService.GetStudentById(studentId).Returns((Student)null);
            var sut = new StudentController(studentService, null, GetLog());

            //Act
            var actual = await sut.Edit(studentId);

            //Assert
            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public async Task EditGet_ReturnViewResult_WithViewModel()
        {
            //Arrange
            int studentId = 1;
            StudentVM studentVM = new() { Id = studentId };
            Student student = new Student() { Id = studentId };

            var studentService = Substitute.For<StudentService>();
            studentService.GetStudentById(studentId).Returns(student);
           
            var authorizeService = Substitute.For<IAuthorizationService>();
            authorizeService.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Is<Student>(x => x.Id == student.Id), "UserAccessPolicy")
                .Returns(Task.FromResult(AuthorizationResult.Success()));

            var sut = new StudentController(studentService, authorizeService, GetLog());
            //Act
            var actual = await sut.Edit(studentId);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            Assert.IsAssignableFrom<StudentVM>(viewResult.Model);
            viewResult.Model.Should().BeEquivalentTo(studentVM);
            Assert.Equal("Student", viewResult.ViewName);
            Assert.Equal("Edit", viewResult.ViewData["Action"]);
        }

        [Fact]
        public async Task EditPost_ReturnsBadRequest_WhenStudentVMIsNull()
        {
            //Arrange
            var sut = new StudentController(null, null, GetLog());

            //Act
            var actual = await sut.Edit(null);

            //Assert
            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public async Task EditPost_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new StudentController(null, null, GetLog());
            controller.ModelState.AddModelError("A", "B");

            // Act
            var result = await controller.Edit(new StudentVM());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<StudentVM>(viewResult.Model);
            Assert.Equal("Student", viewResult.ViewName);
            Assert.Equal("Edit", viewResult.ViewData["Action"]);
        }

        [Fact]
        public async Task EditPost_CallsAuthorizeService_WhenRequestIsValid()
        {
            //Arrange
            var studentVM = new StudentVM()
            {
                Id = 1,
                Email = "email",
                BirthDate = new DateTime(2020, 2, 2),
                Name = "Test"
            };

            var studentService = Substitute.For<StudentService>();
            studentService.UpdateStudent(Arg.Is<Student>(s =>
            s.Id == studentVM.Id &&
            s.BirthDate == studentVM.BirthDate &&
            s.Name == studentVM.Name &&
            s.Email == studentVM.Email))
                .Returns(new ValidationResponse<Student>(new Student()));

            var authorizeService = Substitute.For<IAuthorizationService>();
            authorizeService.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Is<Student>(x => x.Id == studentVM.Id), "UserAccessPolicy")
                .Returns(Task.FromResult(AuthorizationResult.Success()));

            var sut = new StudentController(studentService, authorizeService, GetLog());

            //Act
            var actual = await sut.Edit(studentVM);

            //Assert
            authorizeService.Received(1);
        }

        [Fact]
        public async Task EditPost_RedirectsToCoursesAndEditCourse_WhenRequestIsValid()
        {
            //Arrange
            var studentVM = new StudentVM()
            {
                Id = 1,
                Email = "email",
                BirthDate = new DateTime(2020, 2, 2),
                Name = "Test"
            };

            var studentService = Substitute.For<StudentService>();
            studentService.UpdateStudent(Arg.Is<Student>(s =>
            s.Id == studentVM.Id &&
            s.BirthDate == studentVM.BirthDate &&
            s.Name == studentVM.Name &&
            s.Email == studentVM.Email))
                .Returns(new ValidationResponse<Student>(new Student()));

            var authorizeService = Substitute.For<IAuthorizationService>();
            authorizeService.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Is<Student>(x => x.Id == studentVM.Id), "UserAccessPolicy")
                .Returns(Task.FromResult(AuthorizationResult.Success()));

            var sut = new StudentController(studentService, authorizeService, GetLog());

            //Act
            var actual = await sut.Edit(studentVM);

            //Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            studentService.Received(1);
        }
        [Fact]
        public async Task Delete_RedirectsToCoursesAndDeleteCourse()
        {
            //Arrange
            int studentId = 1;
            int counter = 0;
            var studentService = Substitute.For<StudentService>();
            studentService.When(c => c.DeleteStudent(studentId)).Do(x => counter++);

            var sut = new StudentController(studentService, null, GetLog());

            //Act
            var actual = await sut.Delete(studentId);

            //Assert
            Assert.IsType<RedirectToActionResult>(actual);
            Assert.Equal(1, counter);
        }

        private List<Student> GetStudentList()
        {
            return new List<Student>() { new Student(), new Student() };
        }
        private ILogger<StudentController> GetLog()
        {
            return Substitute.For<ILogger<StudentController>>();
        }
    }
}

