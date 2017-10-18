using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace csMACnz.SeaOrDew.AspNetCore2Tests.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSeaOrDewHandlers(options =>
            {
                options.LoadAllHandlersFromAssembly(typeof(Startup).Assembly);
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
                        if (testQueryResult != null)
                        {
                            response = "query";
                            result = HttpStatusCode.OK;
                        }
                        break;
                    case "/commandFail":
                        var commandFailResult = await commandHandler.Handle(new FailureCommand());
                        if (!commandFailResult.IsSuccess)
                        {
                            response = "failed command";
                            result = HttpStatusCode.PreconditionFailed;
                        }
                        break;
                    case "/commandPass":
                        var commandResult = await commandHandler.Handle(new SuccessCommand());
                        if (commandResult.IsSuccess)
                        {
                            response = "command";
                            result = HttpStatusCode.OK;
                        }
                        break;
                }
                context.Response.StatusCode = (int)result;
                await context.Response.WriteAsync(response);
            });
        }
    }
}
