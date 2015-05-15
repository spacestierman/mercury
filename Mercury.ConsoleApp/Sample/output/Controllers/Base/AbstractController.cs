using EPiServerMvcBootstrap.Controllers;
using EPiServerMvcBootstrap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.Space150.SampleProject.Web.Controllers
{
    [IsLoggedInHeader]
    public abstract class AbstractController<T> : ApplicationPageController<T> where T : TypedPageData
    {
        private Logger _logger;
        protected Logger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = Logger.GetLogger(this);
                }
                return _logger;
            }
        }
    }
}