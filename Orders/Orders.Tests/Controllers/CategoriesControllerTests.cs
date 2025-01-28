using Microsoft.AspNetCore.Mvc;
using Moq;
using Orders.BackEnd.Controllers;
using Orders.BackEnd.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Tests.Controllers
{
    [TestClass]
    public class CategoriesControllerTests
    {
        private Mock<IGenericUnitOfWork<Category>> _morckGenericUnitOfWork = null!;
        private Mock<ICategoriesUnitOfWork> _morckCategoriesUnitOfWork = null!;
        private CategoriesController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            _morckGenericUnitOfWork = new Mock<IGenericUnitOfWork<Category>>();
            _morckCategoriesUnitOfWork = new Mock<ICategoriesUnitOfWork>();
            _controller = new CategoriesController(_morckGenericUnitOfWork.Object, _morckCategoriesUnitOfWork.Object);
        }

        [TestMethod]
        public async Task GetComboAsync_RetunrnsOkObject()
        {
            //Arrenge
            var comboData = new List<Category> { new() };
            _morckCategoriesUnitOfWork.Setup(x => x.GetComboAsync()).ReturnsAsync(comboData);

            //Act
            var result = await _controller.GetComboAsync();

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(comboData, okResult!.Value);
            _morckCategoriesUnitOfWork.Verify(x => x.GetComboAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GetAsyn_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            //Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<IEnumerable<Category>> { WasSuccess = true };
            _morckCategoriesUnitOfWork.Setup(x => x.GetAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(pagination);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _morckCategoriesUnitOfWork.Verify(x => x.GetAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetAsyn_ReturnsOkObjectResult_WhenWasSuccessIsFalse()
        {
            //Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<IEnumerable<Category>> { WasSuccess = false };
            _morckCategoriesUnitOfWork.Setup(x => x.GetAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(pagination);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _morckCategoriesUnitOfWork.Verify(x => x.GetAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetPagesAsyn_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            //Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = true, Result = 5 };
            _morckCategoriesUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _morckCategoriesUnitOfWork.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetPagesAsyn_ReturnsOkObjectResult_WhenWasSuccessIsFalse()
        {
            //Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = false };
            _morckCategoriesUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _morckCategoriesUnitOfWork.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once());
        }
    }
}