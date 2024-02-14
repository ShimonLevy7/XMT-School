using Google.Protobuf.WellKnownTypes;

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

using XmtSchoolDatabase;

using XmtSchoolWebApi.Services;

namespace XmtSchoolWebApi
{
	public class Program
	{
		private const string CorsPolicyCredentials = "_corsPolicyCredentials";

		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			// Get connection string for database
			string? connectionString = builder.Configuration.GetConnectionString("XmtSchoolDb");

			if (string.IsNullOrEmpty(connectionString))
				throw new Exception("No connection string found in configuration, process will now halt.");

			// Configure the database context
			builder.Services.AddDbContext<XmtSchoolDbContext>(options => options.UseMySQL(connectionString));

			// Add custom services
			builder.Services.AddHostedService<TokenDisposer>();

			// Add services to the container
			builder.Services.AddControllers();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddCors(options =>
			{
				options.AddDefaultPolicy(
					builder =>
					{
						builder.AllowAnyOrigin();
						builder.AllowAnyHeader();
						builder.AllowAnyMethod();
					});

				options.AddPolicy(name: CorsPolicyCredentials,
					builder =>
					{
						builder.WithOrigins(new[] { "http://localhost:3000", "https://xmt-school.tiro-finale.com" })
						   .AllowAnyHeader()
						   .AllowAnyMethod()
						   .AllowCredentials();
					});
			});

			WebApplication app = builder.Build();

			// Use CORS middleware
			app.UseCors(CorsPolicyCredentials);

			// Configure the HTTP request pipeline
			ConfigureHttpPipeline(app, builder);

			// Configure app
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

			app.UseAuthentication();

			app.UseHttpsRedirection();

			// Allow authorisation
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}

		/// <summary>
		/// Configure the pipeline based on the environment.
		/// </summary>
		/// <param name="app"></param>
		/// <param name="builder"></param>
		private static void ConfigureHttpPipeline(WebApplication app, WebApplicationBuilder builder)
		{
			if (app.Environment.IsDevelopment())
			{
				ConfigureForDevelopment(app);

				return;
			}

			ConfigureForRelease(app, builder);
		}

		/// <summary>
		/// Configure the app for development (debug).
		/// </summary>
		/// <param name="app"></param>
		private static void ConfigureForDevelopment(WebApplication app)
		{
			app.Urls.Add("http://localhost:50000");

			app.UseSwagger();
			app.UseSwaggerUI();
		}

		/// <summary>
		/// Configure the app for release.
		/// </summary>
		/// <param name="app"></param>
		/// <param name="builder"></param>
		private static void ConfigureForRelease(WebApplication app, WebApplicationBuilder builder)
		{
			app.Urls.Add("https://*:50001");

			// Include a certificate.
			string certPath = @"/etc/letsencrypt/live/api.tiro-finale.com/";

			builder.Configuration["Kestrel:Certificates:Default:Path"] = Path.Combine(certPath, @"fullchain.pem");
			builder.Configuration["Kestrel:Certificates:Default:KeyPath"] = Path.Combine(certPath, @"privkey.pem");
		}
	}
}
