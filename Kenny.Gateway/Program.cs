using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Add Authentication service
builder.Services.AddAuthentication("Bearer")
	.AddJwtBearer("Bearer", options =>
	{
		options.Authority = builder.Configuration["ServiceUrls:IdentityAPI"];
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = false
		};
	});

builder.Services.AddOcelot();

await app.UseOcelot();

app.MapGet("/", () => "Hello World!");

app.Run();
