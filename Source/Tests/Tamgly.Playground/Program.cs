using Kysect.CommonLib.DependencyInjection.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using Tamgly.Playground;
using Tamgly.Playground.Scenarios;

Console.OutputEncoding = Encoding.UTF8;

ILogger logger = DefaultLoggerConfiguration.CreateConsoleLogger();

var config = new TamglyConfig();

//var clockifyTrackProvider = new ClockifyTrackProvider(config.ClockifyApiKey);
//await clockifyTrackProvider.Provide();

//var taskManagementWorkflow = await TaskManagementWithAzureAndTodoWorkflow.CreateForAzureAndTodo(config, logger);
TaskManagementWithAzureAndTodoWorkflow taskManagementWorkflow = await TaskManagementWithAzureAndTodoWorkflow.CreateForOffice(config, logger);
await taskManagementWorkflow.Execute();

//var publicClientApplicationHolder = new PublicClientApplicationHolder(config.MicrosoftTodo.ClientId);
//var graphServiceClientFactory = new GraphServiceClientFactory(publicClientApplicationHolder, MicrosoftStaticLinks.GraphApiUrl);
//GraphServiceClient graphClient = await graphServiceClientFactory.CreateIntegratedWindowsAuth();