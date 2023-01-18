using Kenny.Services.OrderAPI.Messaging.Interfaces;

namespace Kenny.Services.OrderAPI.Extensions
{
	public static class ApplicationBuilderExtensions
	{
		public static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }
		public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
		{
			ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
			var hostApplicatinLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

			hostApplicatinLife.ApplicationStarted.Register(OnStart);
			hostApplicatinLife.ApplicationStopped.Register(OnStop);
			return app;
		}

		public static void OnStart()
		{
			ServiceBusConsumer.Start();
		}

		public static void OnStop()
		{
			ServiceBusConsumer.Stop();
		}
	}
}
