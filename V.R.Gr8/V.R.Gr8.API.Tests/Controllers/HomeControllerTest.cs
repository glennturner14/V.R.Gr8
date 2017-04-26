using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using V.R.Gr8.API;
using V.R.Gr8.API.Controllers;

namespace V.R.Gr8.API.Tests.Controllers {
    [TestClass]
    public class HomeControllerTest {
        [TestMethod]
        public void Index() {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
