using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sports_Store.Domain.Entities;

namespace ShoppingSiteASP.Infrastructure
{
    public class CartModelBinder : IModelBinder
    {
        private const string cartSessionKey = "Cart";
        public object BindModel(ControllerContext controllerContext, 
            ModelBindingContext bindingContext)
        {
            Cart cart = null;
            if(controllerContext.HttpContext.Session != null)
            {
                cart = (Cart)controllerContext.HttpContext.Session[cartSessionKey];
            }

            if(cart == null)
            {
                cart = new Cart();
                if (controllerContext.HttpContext.Session != null)
                    controllerContext.HttpContext.Session[cartSessionKey] = cart;
            }

            return cart;
        }
    }
}