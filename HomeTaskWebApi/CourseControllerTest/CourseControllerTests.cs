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

namespace WebApiTest
{
    public class CourseControllerTests
    {
        [Fact]
        public async Task GetCourses_ReturnsViewResult_WithListOfCourses()
        {
            // Arrange
            var courseService = Substitute.For<CourseService>();
            courseService.GetAllCourses().Returns(GetCoursesList());
            var controller = new CourseController(courseService, null, GetLog());

            // Act
            var result = await controller.GetCourses();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<CourseVM>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void CreateGet_ReturnViewResult_WithViewModel()
        {
            //Arrange
            var sut = new CourseController(null, null, GetLog());

            //Act
            var actual = sut.Create();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            Assert.IsAssignableFrom<CourseVM>(viewResult.Model);
            Assert.Equal("Course", viewResult.ViewName);
            Assert.Equal("Create", viewResult.ViewData["Action"]);
        }

        [Fact]
        public async Task CreatePost_ReturnsBadRequest_WhenCourseVMIsNull()
        {
            //Arrange
            var sut = new CourseController(null, null, GetLog());

            //Act
            var actual = await sut.Create(null);

            //Assert
            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public async Task CreatePost_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new CourseController(null, null, GetLog());
            controller.ModelState.AddModelError("A", "B");

            // Act
            var result = await controller.Create(new CourseVM());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<CourseVM>(viewResult.Model);
            Assert.Equal("Course", viewResult.ViewName);
            Assert.Equal("Create", viewResult.ViewData["Action"]);
        }

        [Fact]
        public async Task CreatePost_RedirectsToCoursesAndCreatesCourse_WhenRequestIsValid()
        {
            //Arrange
            var courseVM = new CourseVM()
            {
                Id = 1,
                EndDate = new DateTime(2020, 2, 2),
                StartDate = new DateTime(2020, 1, 1),
                Name = "Test",
                PassCredits = 10
            };

            var courseService = Substitute.For<CourseService>();
            courseService.CreateCourse(Arg.Is<Course>(c =>
            c.Id == courseVM.Id &&
            c.EndDate == courseVM.EndDate &&
            c.StartDate == courseVM.StartDate &&
            c.Name == courseVM.Name &&
            c.PassCredits == courseVM.PassCredits))
                .Returns(new ValidationResponse<Course>(new Course()));

            var sut = new CourseController(courseService, null, GetLog());

            //Act
            var actual = await sut.Create(courseVM);

            //Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            courseService.Received(1);
        }

        [Fact]
        public async Task EditGet_ReturnNotFoundResult_WhenCourseIdIncorrect()
        {
            //Arrange
            int courseId = 0;
            var courseService = Substitute.For<CourseService>();
            courseService.GetCourseById(courseId).Returns((Course)null);
            var sut = new CourseController(courseService, null, GetLog());

            //Act
            var actual = await sut.Edit(courseId);

            //Assert
            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public async Task EditGet_ReturnViewResult_WithViewModel()
        {
            //Arrange
            int courseId = 0;
            CourseVM courseVM = new() { Id = courseId };
            var courseService = Substitute.For<CourseService>();
            courseService.GetCourseById(courseId).Returns(new Course() { Id = courseId });
            var sut = new CourseController(courseService, null, GetLog());

            //Act
            var actual = await sut.Edit(courseId);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            Assert.IsAssignableFrom<CourseVM>(viewResult.Model);
            viewResult.Model.Should().BeEquivalentTo(courseVM);
            Assert.Equal("Course", viewResult.ViewName);
            Assert.Equal("Edit", viewResult.ViewData["Action"]);
        }

        [Fact]
        public void EditPost_ReturnsBadRequest_WhenCourseVMIsNull()
        {
            //Arrange
            var sut = new CourseController(null, null, GetLog());

            //Act
            var actual = sut.Edit(null);

            //Assert
            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public void EditPost_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new CourseController(null, null, GetLog());
            controller.ModelState.AddModelError("A", "B");

            // Act
            var result = controller.Edit(new CourseVM());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<CourseVM>(viewResult.Model);
            Assert.Equal("Course", viewResult.ViewName);
            Assert.Equal("Edit", viewResult.ViewData["Action"]);
        }

        [Fact]
        public void EditPost_RedirectsToCoursesAndEditCourse_WhenRequestIsValid()
        {
            //Arrange
            var courseVM = new CourseVM()
            {
                Id = 1,
                EndDate = new DateTime(2020, 2, 2),
                StartDate = new DateTime(2020, 1, 1),
                Name = "Test",
                PassCredits = 10
            };

            var courseService = Substitute.For<CourseService>();
            courseService.UpdateCourse(Arg.Is<Course>(c =>
            c.Id == courseVM.Id &&
            c.EndDate == courseVM.EndDate &&
            c.StartDate == courseVM.StartDate &&
            c.Name == courseVM.Name &&
            c.PassCredits == courseVM.PassCredits))
                .Returns(new ValidationResponse<Course>(new Course()));

            var sut = new CourseController(courseService, null, GetLog());

            //Act
            var actual = sut.Edit(courseVM);

            //Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            courseService.Received(1);
        }

        [Fact]
        public async Task AddStudentsToCourseGet_ReturnsBadRequest_WhenNonExistingCourseId()
        {
            //Arrange
            int courseId = 0;

            var courseService = Substitute.For<CourseService>();
            courseService.GetCourseById(courseId).Returns((Course)null);

            var sut = new CourseController(courseService, null, null);
            
            //Act
            var actual = await sut.AddStudentsToCourse(courseId);

            //Assert
            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public async Task AddStudentsToCourseGet_ReturnsViewResult_WithViewModel()
        {
            // Arrange
            int courseId = 0;
            const int assignedStudentId = 11;
            const int nonAssignedStudentId = 22;
            const string courseName = "test";
            AddStudentsToCourseVM expectedModel = new()
            {
                Id = courseId,
                Name = courseName,
                students = new List<AssignmentStudentVM>()
                {
                   new AssignmentStudentVM(){Id = assignedStudentId,Name = "Test1", IsAssigned = true},
                   new AssignmentStudentVM(){Id = nonAssignedStudentId,Name = "Test2", IsAssigned = false}
                }
            };
            var courseService = Substitute.For<CourseService>();
            courseService.GetCourseById(courseId).Returns(new Course()
            {
                Id = courseId,
                Name = courseName,
                Students = new List<Student>() { new Student() { Id = assignedStudentId } }
            });

            var studentService = Substitute.For<StudentService>();
            var students = new List<Student>()
            {
                new Student() { Id = assignedStudentId, Name = "Test1" },
                new Student() { Id = nonAssignedStudentId, Name = "Test2" }
            };
            studentService.GetAllStudents().Returns(students);
            var controller = new CourseController(courseService, studentService,GetLog());

            // Act
            var result = await controller.AddStudentsToCourse(courseId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualModel = Assert.IsAssignableFrom<AddStudentsToCourseVM>(viewResult.Model);
            actualModel.Should().BeEquivalentTo(expectedModel);
        }

        [Fact]
        public async Task AddStudentsToCoursePost_ReturnsBadRequest_WhenViewModelIsEmpty()
        {
            //Arrange
            var sut = new CourseController(null, null, GetLog());

            //Act
            var actual = await sut.AddStudentsToCourse(null);

            //Assert
            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public async Task AddStudentsToCoursePost_SetStudentsToCoursesAndRedirectsToCourses_WhenCoursesAndStudentsAreAssigned()
        {
            //Arrange
            var addStudentsToCourseVM = new AddStudentsToCourseVM()
            {
                Id = 1,
                students = new List<AssignmentStudentVM>()
                {
                    new AssignmentStudentVM() {Id=1,IsAssigned=true},
                    new AssignmentStudentVM() {Id=2,IsAssigned=true}
                }
            };
            int counter = 0;
            var courseService = Substitute.For<CourseService>();
            courseService.When(x => x.SetStudentsToCourse(addStudentsToCourseVM.Id,
                Arg.Is<IEnumerable<int>>(s => s.Count() == addStudentsToCourseVM.students.Count)))
                .Do(x => counter++);

            var sut = new CourseController(courseService, null,GetLog());

            //Act
            var actual = await sut.AddStudentsToCourse(addStudentsToCourseVM);

            //Assert
            Assert.IsType<RedirectToActionResult>(actual);
            Assert.Equal(1, counter);
        }

        [Fact]
        public async Task Delete_RedirectsToCoursesAndDeleteCourse()
        {
            //Arrange
            int courseId = 1;
            int counter = 0;
            var courseService = Substitute.For<CourseService>();
            courseService.When(c => c.DeleteCourse(courseId)).Do(x => counter++);

            var sut = new CourseController(courseService, null, GetLog());

            //Act
            var actual = await sut.Delete(courseId);

            //Assert
            Assert.IsType<RedirectToActionResult>(actual);
            Assert.Equal(1, counter);
        }

        private List<Course> GetCoursesList()
        {
            return new List<Course>() { new Course(), new Course() };
        }
        private ILogger<CourseController> GetLog()
        {
            return Substitute.For<ILogger<CourseController>>();
        }
    }
}
