using GUI.Data;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Blazored.Modal;

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

            services.AddSingleton<ElectronManifestService, ElectronManifestService>();
            services.AddSingleton<ConfigLoaderService, ConfigLoaderService>();
            services.AddSingleton<UpdateCheckerService>(
                provider => new UpdateCheckerService(provider.GetService<ElectronManifestService>(), provider.GetService<ConfigLoaderService>())
            );
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
                    Width = 1028,
                    Height = 840,
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