using System.Net.Http.Headers;
using System.Web.Http;
using DAL;
using DAL.Models;
using Microsoft.Practices.Unity;
using NLog;
using NLog.Fluent;

namespace DummyWebAPI
{
    public static class WebApiConfig
    {
        public static UnityContainer container;

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            container = new UnityContainer();
            container.RegisterType<IRepository<UserDto>, UserRepository>();
            container.RegisterType<IRepository<MessageDto>, MessageRepository>();
            container.RegisterType<IRepository<ConferenceDto>, ConferenceRepository>();
            container.RegisterType<IRepository<FriendDto>, FriendRepository>();

            container.RegisterType<Logger>(new InjectionFactory(f => LogManager.GetCurrentClassLogger(typeof(Log))));

            AutoMapperConfiguration.Configure();

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
