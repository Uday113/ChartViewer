using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChartViewer.Filters;
using HomeModule;
using HomeContracts;

namespace ChartViewer.Controllers
{
    [SessionExpireFilterAttribute]
    public class HomeController : Controller
    {
        readonly IHomeModule homeservice;
        public HomeController(IHomeModule homeservice)
        {
            this.homeservice = homeservice;
        }

        public ActionResult ViewChart()
        {
            var listOfStatesData = homeservice.GetChartData();
            ViewBag.StatesData = listOfStatesData;
            ViewBag.IsLogoutVisible ="Y";
            return View();
        }
      
    }
}