using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using csMACnz.SeaOrDew;
using System.Reflection;
using Xunit;

namespace Tests.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSeaOrDewHandlers(options =>
            {
                options.LoadAllHandlersFromAssembly(typeof(Startup).GetTypeInfo().Assembly);
                options.UseLifetimeScope(ServiceLifetime.Scoped);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var queryHandler = context.RequestServices.GetService<QueryHandler>();
                var commandHandler = context.RequestServices.GetService<CommandHandler>();
                var response = "Error";
                var result = HttpStatusCode.InternalServerError;
                switch (context.Request.Path)
                {
                    case "/query":
                        var testQueryResult = await queryHandler.Handle(new TestQuery());
                        Assert.NotNull(testQueryResult);
                        response = "query";
                        result = HttpStatusCode.OK;
                        break;
                    case "/commandFail":
                    case "/commandPass":
                        var pass = context.Request.Path == "/commandPass";
                        var commandResult = await commandHandler.Handle(new TestCommand(pass));
                        if (commandResult.IsSuccess)
                        {
                            response = "command";
                            result = HttpStatusCode.OK;
                        }
                        else
                        {
                            response = "failed command";
                            result = HttpStatusCode.PreconditionFailed;
                        }
                        break;
                    default:
                        response = "Not Found";
                        result = HttpStatusCode.NotFound;
                        break;
                }
                context.Response.StatusCode = (int)result;
                await context.Response.WriteAsync(response);
            });
        }
    }
}
