<%@ Application Language="C#" %>
<%@ Import Namespace="MLMS.Common" %>
<%@ Import Namespace="MLMS.Objects" %>
<%@ Import Namespace="MLMS.Service" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Security.Principal" %>
<script runat="server">
    protected void Application_Start(object sender, EventArgs e)
    {

    }

    protected void Session_Start(object sender, EventArgs e)
    {
        SessionHelper.ClearSession();

        Host host = new Host();
        host.HostID = 1;
        host.HostName = "local.FoodFrenzy.com";
        host.ApplicationName = "FoodFrenzy";
        host.TemplateID = 1;
        host.ThemeName = "bdsu";
        host.IsSecure = false;
            SessionHelper.Host = host;

    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
       
    }

    protected void Application_PostAcquireRequestState(object sender, EventArgs e)
    {
        

    }
   
    protected void Application_Error(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError();
        ExceptionHandler.Write(ex, "Global.asax");
    }

    protected void Session_End(object sender, EventArgs e)
    {

    }

    protected void Application_End(object sender, EventArgs e)
    {

    }
</script>