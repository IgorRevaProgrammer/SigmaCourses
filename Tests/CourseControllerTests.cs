using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using NSubstitute;
using Services;
using University.MVC.Controllers;
using University.MVC.ViewModels;
using Xunit;
using FluentAssertions;
using System;
using Services.Validators;
using System.Linq;

namespace University.MVC.Tests
{
    public class CourseControllerTests
    {
        [Fact]
        public void Courses_ReturnsViewResult_WithListOfCourses()
        {
            // Arrange
            var courseServiceMock = Substitute.For<CourseService>();
            courseServiceMock.GetAllCourses().Returns(GetCoursesList());
            var controller = new CourseController(courseServiceMock, null);

            // Act
            var result = controller.Courses();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Course>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void AssignStudents_ReturnsViewResult_WithViewModel()
        {
            // Arrange
            var courseServiceMock = Substitute.For<CourseService>();
            courseServiceMock.GetCourseById(Arg.Any<int>()).Returns(new Course());
            var studentService = Substitute.For<StudentService>();
            studentService.GetAllStudents().Returns(new List<Student>());
            var controller = new CourseController(courseServiceMock, studentService);

            // Act
            var result = controller.AssignStudents(0);
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<CourseStudentAssignmentViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void AssignStudents_ReturnsViewResult_WithCoursesAndStudentsAreAssigned()
        {
            // Arrange
            const int courseId = 1;
            const int assignedStudentId = 11;
            const int nonAssignedStudentId = 22;
            const string courseName = "test";

            CourseStudentAssignmentViewModel expectedModel = new CourseStudentAssignmentViewModel()
            {
                Id = courseId,
                Name = courseName,
                Students = new List<AssignmentStudentViewModel>()
                {
                   new AssignmentStudentViewModel(){StudentId = assignedStudentId,StudentFullName = "Test1", IsAssigned = true},
                   new AssignmentStudentViewModel(){StudentId = nonAssignedStudentId,StudentFullName = "Test2", IsAssigned = false}
                }
            };
            var courseServiceMock = Substitute.For<CourseService>();

            courseServiceMock.GetCourseById(courseId).Returns(new Course()
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
            var controller = new CourseController(courseServiceMock, studentService);

            // Act
            var result = controller.AssignStudents(courseId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualModel = Assert.IsAssignableFrom<CourseStudentAssignmentViewModel>(viewResult.ViewData.Model);
            actualModel.Should().BeEquivalentTo(expectedModel);
        }

        [Fact]
        public void Create_ReturnsBadRequest_WhenCourseParameterIsNull()
        {
            //Arrange
            var sut = new CourseController(null, null);
            //Act
            var actual = sut.Create(null);
            //Assert
            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public void Create_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var courseServiceMock = Substitute.For<CourseService>();
            courseServiceMock.GetCourseById(Arg.Any<int>()).Returns(new Course());
            var studentService = Substitute.For<StudentService>();
            studentService.GetAllStudents().Returns(new List<Student>());
            var controller = new CourseController(courseServiceMock, studentService);

            controller.ModelState.AddModelError("s", "v");

            // Act
            var result = controller.Create(new CourseViewModel());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<CourseViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void Create_RedirectsToCoursesAndCreatesCourses_WhenRequestIsValid()
        {
            //Arrange
            var courseVM = new CourseViewModel()
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

            var sut=new CourseController(courseService, null);

            //Act
            var actual = sut.Create(courseVM);

            //Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            courseService.Received(1);
        }

        [Fact]
        public void AssignStudents_ReturnsBadRequest_WhenNonExistingCourseId()
        {
            //Arrange
            int courseId = 0;

            var courseService= Substitute.For<CourseService>();
            courseService.GetCourseById(courseId).Returns((Course)null);

            var studentService= Substitute.For<StudentService>();
            studentService.GetAllStudents().Returns(new List<Student>());

            var sut = new CourseController(courseService, studentService);
            //Act
            var actual = sut.AssignStudents(courseId);
            //Assert
            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public void AssignStudents_ReturnsBadRequest_WhenAssignmentViewModelIsEmpty()
        {
            //Arrange
            var sut = new CourseController(null, null);
            //Act
            var actual = sut.AssignStudents(null);
            //Assert
            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public void AssignStudents_SetStudentsToCoursesAndRedirectsToCourses_WhenCoursesAndStudentsAreAssigned()
        {
            //Arrange
            var courseStudentAssignmentVM = new CourseStudentAssignmentViewModel()
            {
                Id = 1,
                Students = new List<AssignmentStudentViewModel>()
                {
                    new AssignmentStudentViewModel() {StudentId=1,IsAssigned=true},
                    new AssignmentStudentViewModel() {StudentId=2,IsAssigned=true}
                }
            };
            int counter = 0;
            var courseService = Substitute.For<CourseService>();
            courseService.When(x => x.SetStudentsToCourse(courseStudentAssignmentVM.Id,
                Arg.Is<IEnumerable<int>>(s => s.Count() == courseStudentAssignmentVM.Students.Count)))
                .Do(x => counter++);
          
            var sut = new CourseController(courseService, null);

            //Act
            var actual = sut.AssignStudents(courseStudentAssignmentVM);

            //Assert
            Assert.IsType<RedirectToActionResult>(actual);
            Assert.Equal(1, counter);
        }

        private List<Course> GetCoursesList()
        {
            return new List<Course>() { new Course(), new Course() };
        }
    }
}
