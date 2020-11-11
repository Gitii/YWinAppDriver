// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WinAppDriver.Infra;
using WinAppDriver.Infra.CommandHandler;
using WinAppDriver.Infra.Communication;

namespace WinAppDriver
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers().AddNewtonsoftJson();
      services.AddSingleton<ISessionManager>(new SessionManager(() => { return new Session(
                                                                                     new ApplicationMananger()
                                                                                    ); }));
      services.AddSingleton<ICommandHandlers, CommandHandlers>();
      services.AddSingleton<IElementCommandHandler, ElementCommandHandler>();

      services.AddSwaggerDocument();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UsePathBase("/wd/hub");

      app.UseRouting();

      app.UseAuthorization();

      // Register the Swagger generator and the Swagger UI middlewares
      app.UseOpenApi();
      app.UseSwaggerUi3();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
