using Kenny.Services.Email.Messaging.Interfaces;

namespace Kenny.Services.Email.Extension
{
	public static class ApplicationBuilderExtensions
	{
		public static IAzureServiceBusConsumerEmail ServiceBusConsumer { get; set; }
		public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
		{
			ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumerEmail>();
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
