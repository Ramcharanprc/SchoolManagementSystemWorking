using System.Web.Optimization;

namespace SchoolManagementSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
         
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js","~/Scripts/SweetAlert.js", "~/Scripts/jquery-{version}.js",
                      "~/Scripts/jquery.validate*", "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/design.css",
                      "~/Content/site.css", 
                      "~/Content/SweetAlert.css", "~/Content/style1.css",
                      "~/Content/style.css", "~/Content/bootstrap.css"));
            BundleTable.EnableOptimizations = true;
        }
    }
}
