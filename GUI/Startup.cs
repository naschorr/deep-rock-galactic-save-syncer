using GUI.Data;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Blazored.Modal;
using Core;

namespace GUI
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
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredModal();

            // Core Services
            Core.CoreServiceExtensions.AddClasses(services);

            // GUI Services
            services.AddScoped<ModalManagerService, ModalManagerService>(); // Unable to construct when `AddSingleton` is used instead of `AddScoped`

            services.AddSingleton<ElectronManifestService, ElectronManifestService>();
            services.AddSingleton<ConfigLoaderService, ConfigLoaderService>();
            services.AddSingleton<UpdateCheckerService, UpdateCheckerService>();
            services.AddSingleton<SaveFileManagerService, SaveFileManagerService>();
            services.AddSingleton<SyncerManagerService, SyncerManagerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            if (HybridSupport.IsElectronActive)
            {
                ElectronBootstrap(env);
            }
        }

        public async void ElectronBootstrap(IWebHostEnvironment env)
        {
            var window = await Electron.WindowManager.CreateWindowAsync(
                new BrowserWindowOptions
                {
                    Show = false,
                    Width = 1000,
                    Height = 846,
                    Title = "Deep Rock Galactic Save Syncer",
                    AutoHideMenuBar = true,
                    Resizable = env.IsDevelopment()
                }
            );

            await window.WebContents.Session.ClearCacheAsync();

            window.OnReadyToShow += () => window.Show();
            window.OnClosed += () => Electron.App.Quit();
        }
    }
}