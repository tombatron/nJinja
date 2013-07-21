namespace nJinja.MvcTest.Controllers
{
    using System.Web.Mvc;
    using nJinja.MvcTest.Models;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("home.html");
        }

        public ActionResult Car()
        {
            var car = new Car()
            {
                Make = "Chevrolet",
                Model = "Camaro",
                Year = 2012
            };

            return View("car.html", car);
        }
    }
}
