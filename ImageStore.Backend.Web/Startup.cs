using System;
using System.IO;
using System.Security.Claims;
using AutoMapper;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ImageStore.Backend.Web.Extensions;
using ImageStore.Backend.Web.Helpers;
using ImageStore.Backend.Common.Dtos.Account;
using ImageStore.Backend.Common.Exceptions;
using ImageStore.Backend.Common.Options;
using ImageStore.Backend.Dal;
using ImageStore.Backend.Dal.Entities;
using ImageStore.Backend.Dal.Mapper;
using ImageStore.Backend.Dal.Seed;
using ImageStore.Backend.Bll.Services.Account;
using ImageStore.Backend.Bll.Services.Image;
using Microsoft.AspNetCore.Authorization;

namespace ImageStore.Backend.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtOptions = services.ConfigureOption<JwtOptions>(Configuration);
            services.ConfigureOption<Common.Options.ImageOptions>(Configuration);
            services.ConfigureOption<DbSeedOptions>(Configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtHelper.CreateTokenValidationParameters(jwtOptions);
            });

            services.AddDbContext<ImageStoreDbContext>(builder =>
            {
                builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), o => o.CommandTimeout(300));
            });

            services.AddTransient<JwtHelper>();
            services.AddTransient<AccountService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient(s =>
            {
                IHttpContextAccessor contextAccessor = s.GetService<IHttpContextAccessor>();
                ClaimsPrincipal user = contextAccessor?.HttpContext?.User;
                return user ?? throw new Exception("User not resolved");
            });

            services.AddIdentity<User, IdentityRole<int>>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 1;
            })
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<ImageStoreDbContext>()
            .AddDefaultTokenProviders();

            services.AddControllers()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<LoginUserDtoValidator>();
                    fv.ImplicitlyValidateChildProperties = true;
                });

            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) => Environment.IsDevelopment();
                options.ShouldLogUnhandledException = (ctx, ex, details) => true;

                options.Map<ImageStoreException>(ex => new StatusCodeProblemDetails(ex.StatusCode));
            });

            services.AddSpaStaticFiles(configuration => configuration.RootPath = "wwwroot");

            services.AddSwaggerDocument(settings =>
            {
                settings.Title = "ImageStore";
                settings.DefaultReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;
                settings.DefaultResponseReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;
            });

            services.AddAutoMapper(
                typeof(UserProfile)
            );

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IImageService, ImageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ImageStoreDbContext context, IServiceProvider serviceProvider, IOptions<Common.Options.ImageOptions> imageOptions)
        {
            app.UseProblemDetails();

            if (env.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            if (!env.IsDevelopment() || env.ShouldRunAngular())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            var filesPath = Path.Combine(imageOptions.Value.RootPath, imageOptions.Value.FilesPath);
            var thumbnailsPath = Path.Combine(imageOptions.Value.RootPath, imageOptions.Value.ThumbnailsPath);

            Directory.CreateDirectory(filesPath);
            Directory.CreateDirectory(thumbnailsPath);

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    if (!context.Context.User.Identity.IsAuthenticated)
                        context.Context.Response.Redirect("/");
                },
                FileProvider = new PhysicalFileProvider(filesPath),
                RequestPath = new PathString("/images")
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    if (!context.Context.User.Identity.IsAuthenticated)
                        context.Context.Response.Redirect("/");
                },
                // TODO: ide majd thumbnailsPath
                FileProvider = new PhysicalFileProvider(filesPath),
                RequestPath = new PathString("/thumbnails")
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "../ImageStore.Frontend";

                if (env.IsDevelopment() && env.ShouldRunAngular())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            if (env.IsDevelopment())
            {
                DbInitializer.Seed(context, serviceProvider).Wait();
            }
        }
    }
}
