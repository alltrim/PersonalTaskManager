using System.Web;
using System.Web.Optimization;

namespace PersonalTaskManager
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/placeholders.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/tasks").Include(
                        "~/Scripts/tasks.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css",
                      "~/Content/tasks.css"));

        }
    }
}
