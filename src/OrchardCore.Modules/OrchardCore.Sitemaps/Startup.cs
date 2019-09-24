using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Entities;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using OrchardCore.Sitemaps.Builders;
using OrchardCore.Sitemaps.Drivers;
using OrchardCore.Sitemaps.Models;
using OrchardCore.Sitemaps.Routing;
using OrchardCore.Sitemaps.Services;
using OrchardCore.Sitemaps.SitemapNodes;

namespace OrchardCore.Sitemaps
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, Migrations>();
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddIdGeneration();
            services.AddSingleton<ISitemapRoute, SitemapRoute>();
            services.AddScoped<ISitemapIdGenerator, SitemapIdGenerator>();
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<ISitemapSetService, SitemapSetService>();
            services.AddScoped<IDisplayManager<SitemapNode>, DisplayManager<SitemapNode>>();
            services.AddScoped<ISitemapBuilder, SitemapBuilder>();

            // index treeNode
            services.AddScoped<ISitemapNodeProviderFactory, SitemapNodeProviderFactory<SitemapIndexNode>>();
            services.AddScoped<ISitemapNodeBuilder, SitemapIndexNodeBuilder>();
            services.AddScoped<IDisplayDriver<SitemapNode>, SitemapIndexNodeDriver>();
            //sitemap part
            services.AddScoped<IContentPartDisplayDriver, SitemapPartDisplay>();
            services.AddContentPart<SitemapPart>();
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
                  name: SitemapRouteConstraint.RouteKey,
                  areaName: "OrchardCore.Sitemaps",
                  pattern: "{*sitemaps}",
                  constraints: new { sitemaps = new SitemapRouteConstraint() },
                  defaults: new { controller = "Sitemaps", action = "Index" }
              );
        }
    }
}