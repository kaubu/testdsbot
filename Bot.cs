using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OdysseusBot
{
	public class Bot
	{
		public DiscordClient Client { get; private set; }

		public CommandsNextExtension Commands { get; private set; }

		public async Task RunAsync()
		{
			string json = string.Empty;

			using (FileStream fs = File.OpenRead("config.json"))
			using (StreamReader sr = new StreamReader(fs, new UTF8Encoding(false))) {
				json = await sr.ReadToEndAsync().ConfigureAwait(false);
			}

			ConfigJson configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

			DiscordConfiguration config = new DiscordConfiguration
			{
				Token = configJson.Token,
				TokenType = TokenType.Bot,
				AutoReconnect = true,
				MinimumLogLevel = LogLevel.Debug
				//UseInternalLogHandler = true
			};

			Client = new DiscordClient(config);

			Client.Ready += OnClientReady;

			CommandsNextConfiguration commandsConfig = new CommandsNextConfiguration
			{
				StringPrefixes = new string[] { configJson.Prefix },
				EnableMentionPrefix = true,
				EnableDms = false
			};

			Commands = Client.UseCommandsNext(commandsConfig);

			await Client.ConnectAsync();

			await Task.Delay(-1);
		}

		private Task OnClientReady(object sender, ReadyEventArgs e)
		{
			return Task.CompletedTask;
		}
	}
}
