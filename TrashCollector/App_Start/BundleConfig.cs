using System.Web;
using System.Web.Optimization;

namespace TrashCollector
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //< script type = "text/javascript" src = "/path/to/moment.js" ></ script >
            //< script type = "text/javascript" src = "/path/to/bootstrap/js/transition.js" ></ script >
            //< script type = "text/javascript" src = "/path/to/bootstrap/js/collapse.js" ></ script >
            //< script type = "text/javascript" src = "/path/to/bootstrap/dist/bootstrap.min.js" ></ script >
            //< script type = "text/javascript" src = "/path/to/bootstrap-datetimepicker.min.js" ></ script >

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/moment.js",
                      "~/Scripts/transition.js",
                      "~/Scripts/collapse.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datetimepicker.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min-flatly.css",
                      //"~/Content/bootstrap.min-united.css",
                      //"~/Content/bootstrap.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/site.css"));

        }
    }
}
