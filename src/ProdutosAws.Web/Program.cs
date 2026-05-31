namespace ProdutosAws.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services
            .AddApplication()
            .AddInfrastructure(builder.Configuration);
        
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        if (app.Environment.IsProduction())
        {
            try
            {
                using var scope = app.Services.CreateScope();

                var db = scope.ServiceProvider
                    .GetRequiredService<ProductsAwsDbContext>();

                Console.WriteLine("Iniciando migration...");

                db.Database.Migrate();

                Console.WriteLine("Migration concluída.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            app.MapGet("health", () => "OK");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}