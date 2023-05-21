using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Controllers;
using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Models;
using MovieCatalog.Data;

namespace MovieCatalog.UnitTests
{
    internal class MoviesControllerTests
    {
        private readonly List<Movie> movies;
        private readonly Mock<DbSet<Movie>> dbSetMock;
        private readonly Mock<ILogger<MoviesController>> loggerMock;
        private readonly Mock<MoviesDbContext> dbContextMock;
        private readonly MoviesController controller;

        public MoviesControllerTests()
        {
            // Arrange
            movies = new List<Movie> {
                new Movie("movie1", "genre1", 2001),
                new Movie("movie2", "genre2", 2002),
                new Movie("movie3", "genre3", 2003),
                new Movie("movie4", "genre4", 2002),
                new Movie("movie5", "genre3", 2005)
            };

            var options = new DbContextOptionsBuilder<MoviesDbContext>()
            .UseInMemoryDatabase("Movies")
            .Options;

            dbSetMock = movies.AsDbSetMock();
            loggerMock = new Mock<ILogger<MoviesController>>();
            dbContextMock = new Mock<MoviesDbContext>(options);

            dbContextMock.Setup(db => db.Movies).Returns(dbSetMock.Object);

            controller = new MoviesController(dbContextMock.Object, loggerMock.Object);
        }


        [Test]
        public void GetMovie_ReturnsLastAddedMovie()
        {
            // Act
            var result = controller.GetMovie();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public void GetMovie_WithEmptyDatabase_ReturnsNotFound()
        {
            // Arrange
            var movies2 = new List<Movie>();

            var options = new DbContextOptionsBuilder<MoviesDbContext>()
            .UseInMemoryDatabase("Movies")
            .Options;

            var dbSetMock2 = movies2.AsDbSetMock();
            var dbContextMock2 = new Mock<MoviesDbContext>(options);

            dbContextMock2.Setup(db => db.Movies).Returns(dbSetMock2.Object);

            var controller2 = new MoviesController(dbContextMock2.Object, loggerMock.Object);

            // Act
            var result = controller2.GetMovie();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public void GetMovieByYear_CorrectArgument_ReturnsMoviesList()
        {
            // Act
            var result = controller.GetMoviesByYear("2002");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public void GetMovieByYear_NotNumberArgument_ReturnsBadRequest()
        {
            // Act
            var result = controller.GetMoviesByYear("string");

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public void GetMovieByYear_OutsideRangeArgument_ReturnsBadRequest()
        {
            // Act
            var result = controller.GetMoviesByYear("1600");

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public void GetMovieByYear_ArgumentNotInDatabase_ReturnsNotFound()
        {
            // Act
            var result = controller.GetMoviesByYear("1999");

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public void GetMovieByGenre_CorrectArgument_ReturnsMoviesList()
        {
            // Act
            var result = controller.GetMoviesByGenre("genre4");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public void GetMovieByGenre_ArgumentNotInDatabase_ReturnsNotFound()
        {
            // Act
            var result = controller.GetMoviesByGenre("random genre");

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public void AddMovie_ReturnsCreatedAtAction()
        {
            // Arrange
            var movie = new Movie("movie", "genre", 2023);

            // Act
            var result = controller.AddMovie(movie);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
        }
    }
}
