using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingSiteASP.Controllers;
using ShoppingSiteASP.Models;
using Sports_Store.Domain.Abstract;
using Sports_Store.Domain.Entities;
using ShoppingSiteASP.HtmlHelpers;

namespace Sports_Store.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IProductRepository> mockProducts = new Mock<IProductRepository>();
            mockProducts.Setup(m => m.Products).Returns(new Product[] 
            {
                new Product() { ProductID = 1, Name ="P1" },
                new Product() { ProductID = 2, Name ="P2" },
                new Product() { ProductID = 3, Name ="P3" },
                new Product() { ProductID = 4, Name ="P4" },
                new Product() { ProductID = 5, Name ="P5" }
            });
            ProductController productCtrl = new ProductController(mockProducts.Object);
            productCtrl.pageSize = 3;
            //Act
            ProductsListViewModel result= (ProductsListViewModel) productCtrl.List(null,2).Model;

            //Assert
            Product[] productArray = result.Products.ToArray();
            Assert.IsTrue(productArray.Length == 2);
            Assert.AreEqual(productArray[0].Name, "P4");
            Assert.AreEqual(productArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Arrange
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo()
            {
                CurrentPage = 2,
                ItemsPerPage = 10,
                TotalItems = 28
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                            + @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(pr => pr.Products).Returns(new Product[]
            {
                new Product() { ProductID = 1, Name ="P1"},
                new Product() { ProductID = 1, Name ="P2"},
                new Product() { ProductID = 1, Name ="P3"},
                new Product() { ProductID = 1, Name ="P4"},
                new Product() { ProductID = 1, Name ="P5"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            //Act
            ProductsListViewModel result = (ProductsListViewModel) controller.List(null,2).Model;

            //Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(pr => pr.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name ="P1", Category ="Cat1" },
                new Product { ProductID = 2, Name ="P2", Category ="Cat1" },
                new Product { ProductID = 3, Name ="P3", Category = "Cat2" },
                new Product { ProductID = 4, Name ="P4", Category = "Cat2" },
                new Product { ProductID = 5, Name ="P5", Category = "Cat1" }
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            //Act
            Product[] result = ((ProductsListViewModel) controller.List("Cat1", 2).Model).Products.ToArray();

            //Assert
            Assert.AreEqual(result.Length, 3);
            Assert.IsTrue(result[0].Name == "P1" && result[0].Category == "Cat1");
            Assert.IsTrue(result[1].Name == "P2" && result[1].Category == "Cat1");
            Assert.IsTrue(result[2].Name == "P5" && result[2].Category == "Cat1");
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(pr => pr.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name ="P1", Category ="Cat1" },
                new Product { ProductID = 2, Name ="P2", Category ="Cat2" },
                new Product { ProductID = 3, Name ="P3", Category = "Cat1" },
                new Product { ProductID = 4, Name ="P4", Category = "Cat2" },
                new Product { ProductID = 5, Name ="P5", Category = "Cat3" }
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            //Act
            int result1 = ((ProductsListViewModel)controller.List("Cat1", 2).Model).PagingInfo.TotalItems;
            int result2 = ((ProductsListViewModel)controller.List("Cat2", 2).Model).PagingInfo.TotalItems;
            int result3 = ((ProductsListViewModel)controller.List("Cat3", 2).Model).PagingInfo.TotalItems;
            int result4 = ((ProductsListViewModel)controller.List(null, 2).Model).PagingInfo.TotalItems;

            //Assert
            Assert.AreEqual(result1, 2);
            Assert.AreEqual(result2, 2);
            Assert.AreEqual(result3,1);
            Assert.AreEqual(result4,5);
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(pr => pr.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name ="P1", Category ="Apples" },
                new Product { ProductID = 2, Name ="P2", Category ="Apples" },
                new Product { ProductID = 3, Name ="P3", Category = "Plums" },
                new Product { ProductID = 4, Name ="P4", Category = "Oranges" }
            });
            NavController controller = new NavController(mock.Object);

            //Act
            string[] result = ((IEnumerable<string>)controller.Menu().Model).ToArray();

            //Assert
            Assert.AreEqual(result.Length, 3);
            Assert.IsTrue(result[0] == "Apples");
            Assert.IsTrue(result[1] == "Plums");
            Assert.IsTrue(result[2] == "Oranges");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(pr => pr.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name ="P1", Category ="Apples" },
                new Product { ProductID = 4, Name ="P2", Category = "Oranges" }
            });
            NavController controller = new NavController(mock.Object);
            string categoryToSelect = "Apples";
            //Act
            string result = controller.Menu(categoryToSelect).ViewBag.SelectedCategory;

            //Assert
            Assert.AreEqual(result, "Apples");
        }

        //TODO :: Test Can_Add_New_Lines
        //TODO :: Can_Add_Quantity_For_Existing_Lines
        //TODO :: Can_remove_Lines
        //TODO :: Can_Calculate_Total
        //TODO :: Can_Clear_Contents

        [TestMethod]
        public void Can_Add_To_Cart()
        {

            // Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
            }.AsQueryable());

            // Arrange - create a Cart
            Cart cart = new Cart();

            // Arrange - create the controller
            CartController target = new CartController(mock.Object,null);

            // Act - add a product to the cart
            target.AddToCart(cart, 1, null);

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {

            // Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
            }.AsQueryable());

            // Arrange - create a Cart
            Cart cart = new Cart();

            // Arrange - create the controller
            CartController target = new CartController(mock.Object,null);

            // Act - add a product to the cart
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

            // Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");

        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {

            // Arrange - create a Cart
            Cart cart = new Cart();

            // Arrange - create the controller
            CartController target = new CartController(null);

            // Act - call the Index action method
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {

            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create an empty cart
            Cart cart = new Cart();
            // Arrange - create shipping details
            ShippingDetails shippingDetails = new ShippingDetails();
            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);

            // Act
            ViewResult result = target.Checkout(cart, shippingDetails);
            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());
            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            // Assert - check that I am passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {

            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Arrange - create a cart with an item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);

            // Arrange - add an error to the model
            target.ModelState.AddModelError("error", "error");

            // Act - try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());
            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            // Assert - check that I am passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {

            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Arrange - create a cart with an item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);

            // Act - try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            // Assert - check that the order has been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());
            // Assert - check that the method is returning the Completed view
            Assert.AreEqual("Completed", result.ViewName);
            // Assert - check that I am passing a valid model to the view
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
