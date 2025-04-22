using Application.Dtos;
using Application.Interfaces.IFactories;
using Application.Services;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Moq;

namespace Tests.ServicesTests
{
    public class MovieTheatersServiceTests
    {
        private readonly Mock<IMovieTheatersRepository> _mockRepository;
        private readonly Mock<IMovieTheatersFactory> _mockFactory;
        private readonly NotificationContext _notificationContext;
        private readonly MovieTheatersService _service;

        public MovieTheatersServiceTests()
        {
            _mockRepository = new Mock<IMovieTheatersRepository>();
            _mockFactory = new Mock<IMovieTheatersFactory>();
            _notificationContext = new NotificationContext();

            _service = new MovieTheatersService(
                _mockRepository.Object,
                _notificationContext,
                _mockFactory.Object);
        }

        [Fact]
        public void GetById_ReturnsCorrectDto_WhenMovieTheaterExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var theater = new MovieTheaters { Id = id, Name = "Test Theater" };
            var expectedDto = new MovieTheatersReadDto { Id = id, Name = "Test Theater" };

            _mockRepository.Setup(r => r.GetByElement(It.IsAny<FilterByItem>()))
                .Returns(theater);
            _mockFactory.Setup(f => f.MapToMovieTheaterReadDto(theater))
                .Returns(expectedDto);

            // Act
            var result = _service.GetById(new FilterMovieTheatersById { Id = id });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Name, result.Name);
        }

        [Fact]
        public void GetById_ReturnsNull_WhenMovieTheaterDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByElement(It.IsAny<FilterByItem>()))
                .Returns((MovieTheaters)null);

            // Act
            var result = _service.GetById(new FilterMovieTheatersById { Id = Guid.NewGuid() });

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetFilter_ReturnsFilteredResults()
        {
            // Arrange
            var filter = new FilterMovieTheatersTable();
            var theaters = new List<MovieTheaters> { new MovieTheaters { Id = Guid.NewGuid(), Name = "Theater 1" } };
            var filterReturn = new FilterReturn<MovieTheaters>
            {
                TotalRegister = 10,
                TotalRegisterFilter = 5,
                TotalPages = 2,
                ItensList = theaters
            };

            _mockRepository.Setup(r => r.GetFilter(filter)).Returns(filterReturn);
            _mockFactory.Setup(f => f.MapToMovieTheaterReadDto(It.IsAny<MovieTheaters>()))
                .Returns(new MovieTheatersReadDto { Name = "Theater 1" });

            // Act
            var result = _service.GetFilter(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(filterReturn.TotalRegister, result.TotalRegister);
            Assert.Equal(filterReturn.TotalRegisterFilter, result.TotalRegisterFilter);
            Assert.Single(result.ItensList);
        }

        [Fact]
        public void Add_ReturnsDto_WhenValid()
        {
            // Arrange
            var createDto = new MovieTheatersCreateDto { Name = "New Theater", TheaterLocationId = Guid.NewGuid() };
            var theater = new MovieTheaters { Id = Guid.NewGuid(), Name = "New Theater", TheaterLocationId = createDto.TheaterLocationId };
            var updateDto = new MovieTheatersUpdateDto { Id = theater.Id, Name = "New Theater", TheaterLocationId = createDto.TheaterLocationId };

            _mockRepository.Setup(r => r.ValidateInput(createDto, false, null)).Returns(true);
            _mockFactory.Setup(f => f.MapToMovieTheater(createDto)).Returns(theater);
            _mockRepository.Setup(r => r.Add(theater)).Returns(theater);
            _mockFactory.Setup(f => f.MapToMovieTheaterUpdateDto(theater)).Returns(updateDto);

            // Act
            var result = _service.Add(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateDto.Id, result.Id);
            Assert.Equal(updateDto.Name, result.Name);
        }

        [Fact]
        public void Update_Executes_WhenValid()
        {
            // Arrange
            var updateDto = new MovieTheatersUpdateDto { Id = Guid.NewGuid(), Name = "Updated Theater", TheaterLocationId = Guid.NewGuid() };
            var theater = new MovieTheaters { Id = updateDto.Id, Name = "Old Theater", TheaterLocationId = updateDto.TheaterLocationId };

            _mockRepository.Setup(r => r.GetByElement(It.IsAny<FilterByItem>())).Returns(theater);
            _mockRepository.Setup(r => r.ValidateInput(updateDto, true, theater)).Returns(true);
            _mockFactory.Setup(f => f.MapToMovieTheaterFromUpdateDto(updateDto)).Returns(theater);

            // Act
            _service.Update(updateDto);

            // Assert
            _mockRepository.Verify(r => r.Update(theater), Times.Once);
        }

        [Fact]
        public void Delete_Executes_WhenTheaterExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var theater = new MovieTheaters { Id = id, Name = "Theater to Delete" };

            _mockRepository.Setup(r => r.GetByElement(It.IsAny<FilterByItem>())).Returns(theater);

            // Act
            _service.Delete(id);

            // Assert
            _mockRepository.Verify(r => r.Delete(theater), Times.Once);
        }

        [Fact]
        public void Delete_DoesNotExecute_WhenTheaterDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByElement(It.IsAny<FilterByItem>())).Returns((MovieTheaters)null);

            // Act
            _service.Delete(Guid.NewGuid());

            // Assert
            _mockRepository.Verify(r => r.Delete(It.IsAny<MovieTheaters>()), Times.Never);
        }
    }
}