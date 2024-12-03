using Models;
using Services;
using Xunit;

namespace CountriesProject.Tests
{
    public class ArrayServiceTest
    {
        private readonly ArrayService _arrayService = new ArrayService();

        [Fact]
        public void GetSecondLargest_ValidInput_ReturnsSecondLargest()
        {
            // Arrange
            var request = new RequestObj
            {
                RequestArrayObj = new List<int> { 3, 5, 1, 2, 5 }
            };

            // Act
            var result = _arrayService.GetSecondLargest(request);

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public void GetSecondLargest_NotEnoughDistinctNumbers_ThrowsException()
        {
            // Arrange
            var request = new RequestObj
            {
                RequestArrayObj = new List<int> { 1, 1, 1 }
            };

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => _arrayService.GetSecondLargest(request));

            // Assert
            Assert.Equal("Not enough distinct numbers to determine the second largest.", exception.Message);
        }

        [Fact]
        public void GetSecondLargest_EmptyInput_ThrowsException()
        {
            // Arrange
            var request = new RequestObj
            {
                RequestArrayObj = new List<int>()
            };

            // Act
            var exception = Assert.Throws<ArgumentException>(() => _arrayService.GetSecondLargest(request));

            // Assert
            Assert.Equal("Request array cannot be null or empty.", exception.Message);
        }

        [Fact]
        public void GetSecondLargest_NullInput_ThrowsException()
        {
            // Arrange
            var request = new RequestObj
            {
                RequestArrayObj = null
            };

            // Act
            var exception = Assert.Throws<ArgumentException>(() => _arrayService.GetSecondLargest(request));

            // Assert
            Assert.Equal("Request array cannot be null or empty.", exception.Message);
        }
    }
}

