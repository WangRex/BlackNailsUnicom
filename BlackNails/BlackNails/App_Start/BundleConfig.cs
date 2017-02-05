using System.Web;
using System.Web.Optimization;

namespace BlackNails
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Jquery必备的StyleBundle和ScriptBundle
            StyleBundle css = new StyleBundle("~/Cloud_Admin/css");
            ScriptBundle jquery = new ScriptBundle("~/Cloud_Admin/js");

            //添加Cloud_Admin的样式
            css.Include("~/Cloud_Admin/css/cloud-admin.css",
                        "~/Cloud_Admin/css/themes/default.css",
                        "~/Cloud_Admin/css/responsive.css",
                        "~/Cloud_Admin/font-awesome/css/font-awesome.min.css",
                        "~/Cloud_Admin/js/jquery-ui-1.10.3.custom/css/custom-theme/jquery-ui-1.10.3.custom.min.css");

            //添加Cloud_Admin的JS文件
            jquery.Include("~/Cloud_Admin/js/jquery/jquery-2.0.3.min.js",
                        "~/Cloud_Admin/js/jquery-ui-1.10.3.custom/js/jquery-ui-1.10.3.custom.min.js",
                        "~/Cloud_Admin/bootstrap-dist/js/bootstrap.min.js",
                        "~/Cloud_Admin/js/jQuery-Cookie/jquery.cookie.min.js",
                        "~/Cloud_Admin/js/script.js",
                        "~/Scripts/template.js"); 


            //全部增加到集合里面去
            bundles.Add(css);
            bundles.Add(jquery);
        }
    }
}
