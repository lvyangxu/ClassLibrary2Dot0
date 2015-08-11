using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ClassLibrary2Dot0
{
    public class DoMvcInit
    {
        DoIO DoIO1 = new DoIO();

        public void InitMvcWebFrontEnd(HttpServerUtility Server) {
            DoIO1.directoryCopy(Server.MapPath("bin/js/"), Server.MapPath("Content/js/"));
            DoIO1.directoryCopy(Server.MapPath("bin/css/"), Server.MapPath("Content/css/"));
        }
    }
}
