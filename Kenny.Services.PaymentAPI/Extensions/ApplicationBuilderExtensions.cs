using Kenny.Services.PaymentAPI.Messaging.Interfaces;

namespace Kenny.Services.PaymentAPI.Extensions
{
	public static class ApplicationBuilderExtensions
	{
		public static IAzureServiceBusConsumerPayment ServiceBusConsumer { get; set; }
		public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
		{
			ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumerPayment>();
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
