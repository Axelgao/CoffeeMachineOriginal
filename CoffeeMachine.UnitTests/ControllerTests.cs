using CoffeeMachine.Controllers;
using CoffeeMachine.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using System;
using Newtonsoft.Json;

namespace CoffeeMachine.UnitTests
{
    public class ControllerTests
    {
        private readonly CoffeeMachineController _controller;
        private readonly Mock<ILogger<CoffeeMachineController>> _loggerMock = new Mock<ILogger<CoffeeMachineController>>();

        public ControllerTests() 
        {
            _controller = new CoffeeMachineController(_loggerMock.Object);
        }

        [Fact]
        public void TestRequirementOne()
        {
            var res = _controller.BrewCoffee();

            var okResult = Assert.IsType<OkObjectResult>(res);
            Assert.NotNull(okResult);

            var coffeeResponse = Assert.IsType<Response>(okResult.Value);
            Assert.Equal("Your piping hot coffee is ready", coffeeResponse.Message);
        }

        [Fact]
        public void TestRequirementTwo()
        {
            IActionResult res = null;
            for (int i = 0; i < 5; i++)
            {
                res = _controller.BrewCoffee();
            }

            var statusCodeRes = Assert.IsType<StatusCodeResult>(res);
            Assert.Equal(503, statusCodeRes.StatusCode);
        }

        [Fact]
        public void TestRequirementThree()
        {
            SystemTime.Now = () => new DateTime(2000, 4, 1);

            var res = _controller.BrewCoffee();

            var statusCodeRes = Assert.IsType<StatusCodeResult>(res);
            Assert.Equal(418, statusCodeRes.StatusCode);

            SystemTime.Reset();
        }
    }
}