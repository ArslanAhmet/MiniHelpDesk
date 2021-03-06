using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Rewrite;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MiniHelpDesk.BuildingBlocks.Shared;
using MiniHelpDesk.Services.TicketManagement.Core;
using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using MiniHelpDesk.Services.TicketManagement.Core.Interfaces;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Organizations;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Persons;
using MiniHelpDesk.Services.TicketManagement.Core.Models.Tickets;
using MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Organizations;
using MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Persons;
using MiniHelpDesk.Services.TicketManagement.Core.ObjectMapper.Tickets;
using MiniHelpDesk.Services.TicketManagement.Core.Security;
using MiniHelpDesk.Services.TicketManagement.Core.Services;
using MiniHelpDesk.Services.TicketManagement.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MiniHelpDesk.Services.TicketManagement
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
            services.AddCors();

            var helpDeskContextConnString = GetDecryptedString("AppSettings:HelpDeskDB");

            //services.AddSingleton<ITicketRepository>(new TicketRepository(helpDeskContextConnString));
            services.AddScoped<ITicketRepository, TicketRepository>(implementationFactory =>
            {
                return new TicketRepository(helpDeskContextConnString);
            });

            services.AddScoped<IOrganizationRepository, OrganizationRepository>(implementationFactory =>
            {
                return new OrganizationRepository(helpDeskContextConnString);
            });

            services.AddScoped<IPersonRepository, PersonRepository>(implementationFactory =>
            {
                return new PersonRepository(helpDeskContextConnString);
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var actionExecutingContext =
                        actionContext as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                    // if there are modelstate errors & all keys were correctly
                    // found/parsed we're dealing with validation errors
                    if (actionContext.ModelState.ErrorCount > 0
                        && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
                    {
                        return new UnprocessableEntityObjectResult(actionContext.ModelState);
                    }

                    // if one of the keys wasn't correctly found / couldn't be parsed
                    // we're dealing with null/unparsable input
                    return new BadRequestObjectResult(actionContext.ModelState);
                };
            });
            services.AddSwaggerGen(setupOptions =>
            {
                setupOptions.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "MiniHelpDesk.Services.TicketManagement",
                        Version = "v1",
                        Description = "TicketManagement is for simple ticket management for IT",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Name = "Ahmet Arslan",
                            Email = "arslanahmet1981@gmail.com"
                        }
                    });
            });
            services.AddTransient<IPropertyMappingService, TicketMappingService>();
            services.AddTransient<ITypeHelperService, TypeHelperService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            
            Mappings(services);

            services.AddHealthChecks();
            services.AddSignalR();
            services.AddControllers();
        }

        private static void Mappings(IServiceCollection services)
        {
            #region Persons
            services.AddTransient<IDoubleMapper<PersonForUpdateDto, Person>, PersonToPersonForUpdateDto>();
            services.AddTransient<IMapper<PersonForCreationDto, Person>, PersonForCreationToPerson>();
            services.AddTransient<IMapper<IEnumerable<Person>, IEnumerable<PersonDto>>, PersonListToPersonDtoList>();
            services.AddTransient<IMapper<Person, PersonDto>, PersonToPersonDto>();
            #endregion

            #region Organizations
            services.AddTransient<IDoubleMapper<OrganizationForUpdateDto, Organization>, OrganizationToOrganizationForUpdateDto>();
            services.AddTransient<IMapper<OrganizationForCreationDto, Organization>, OrganizationForCreationToOrganization>();
            services.AddTransient<IMapper<IEnumerable<Organization>, IEnumerable<OrganizationDto>>, OrganizationListToOrganizationDtoList>();
            services.AddTransient<IMapper<Organization, OrganizationDto>, OrganizationToOrganizationDto>();
            #endregion

            #region Tickets
            services.AddTransient<IDoubleMapper<TicketForUpdateDto, Ticket>, TicketToTicketForUpdateDto>();
            services.AddTransient<IMapper<TicketForCreationDto, Ticket>, TicketForCreationToTicket>();
            services.AddTransient<IMapper<IEnumerable<Ticket>, IEnumerable<TicketDto>>, TicketListToTicketDtoList>();
            services.AddTransient<IMapper<Ticket, TicketDto>, TicketToTicketDto>(); 
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(swaggerOptions => {
                    swaggerOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "MiniHelpDesk.Services.TicketManagement v1");
                    //swaggerOptions.RoutePrefix = "";

                    //swaggerOptions.DisplayOperationId();
                });
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.Use(async (httpContext, next) =>
            {
                string userName = "unknown";
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    var userClaimName = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    userName = userClaimName?.Value;
                }

                var remoteIpAddres = httpContext.Connection.RemoteIpAddress.ToString();
                Serilog.Context.LogContext.PushProperty("LoginUser", !string.IsNullOrWhiteSpace(userName) ? userName : "unknown");
                Serilog.Context.LogContext.PushProperty("RemoteIpAddress", !string.IsNullOrWhiteSpace(remoteIpAddres) ? remoteIpAddres : "unknown");

                await next.Invoke();

            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultHealthChecks();
                endpoints.MapControllers();
            });

            //var redirectRootToSwagger = new RewriteOptions()
            //    .AddRedirect("^$", "swagger");
            //app.UseRewriter(redirectRootToSwagger);
        }
        private string GetDecryptedString(string key)
        {
            string AESKey = Configuration["AppSettings:AES"];
            string encryptedConnStr = Configuration[key];
            return Cryptography.Decrypt(encryptedConnStr, AESKey);
        }

        private string EncryptConnectionString(string key)
        {
            string AESKey = Configuration["AppSettings:AES"];
            string encryptedConnStr = Configuration[key];
            return Cryptography.Encrypt(encryptedConnStr, AESKey);
        }
    }
}
