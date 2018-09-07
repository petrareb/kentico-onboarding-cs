﻿using System.Web.Http;

namespace MyOnboardingApp.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(JsonSerializerConfig.Register);
        }
    }
}
