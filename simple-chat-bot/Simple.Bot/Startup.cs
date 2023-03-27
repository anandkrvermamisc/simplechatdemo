﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.13.2

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Azure.Blobs;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simple.Bot.Bots;
using Simple.Bot.Dialogs;
using Simple.Bot.Services;

namespace Simple.Bot
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
			services.AddControllers().AddNewtonsoftJson();

			// Create the Bot Framework Adapter with error handling enabled.
			services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

			// Configure State
			ConfigureState(services);

			ConfigureDialogs(services);

			// Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
			services.AddTransient<IBot, DialogBot<MainDialog>>();
		}
		public void ConfigureDialogs(IServiceCollection services)
		{
			services.AddSingleton<MainDialog>();
		}
		public void ConfigureState(IServiceCollection services)
		{
			// Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.) 
			services.AddSingleton<IStorage, MemoryStorage>();
			
			// Create the User state. 
			services.AddSingleton<UserState>();

			// Create the Conversation state. 
			services.AddSingleton<ConversationState>();

			// Create an instance of the state service 
			services.AddSingleton<StateService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseDefaultFiles()
				.UseStaticFiles()
				.UseWebSockets()
				.UseRouting()
				.UseAuthorization()
				.UseEndpoints(endpoints =>
				{
					endpoints.MapControllers();
				});
		}
	}
}
