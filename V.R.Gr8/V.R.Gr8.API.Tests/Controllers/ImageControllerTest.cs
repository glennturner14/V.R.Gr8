using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using V.R.Gr8.API;
using V.R.Gr8.API.Controllers;

namespace V.R.Gr8.API.Tests.Controllers {
    [TestClass]
    public class ImageControllerTest {
        [TestMethod]
        public void Get() {
            // Arrange
            ImageController controller = new ImageController();

            // Act
            IEnumerable<string> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetById() {
            // Arrange
            ImageController controller = new ImageController();

            // Act
            HttpResponseMessage result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post() {
            // Arrange
            ImageController controller = new ImageController();

            // Act
            controller.Post("value");

            // Assert
        }

        [TestMethod]
        public void Put() {
            // Arrange
            ImageController controller = new ImageController();

            // Act
            controller.Put(5, "value");

            // Assert
        }

        [TestMethod]
        public void Delete() {
            // Arrange
            ImageController controller = new ImageController();

            // Act
            controller.Delete(5);

            // Assert
        }
    }
}
