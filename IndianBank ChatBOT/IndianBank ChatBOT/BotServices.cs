using IndianBank_ChatBOT.Middleware.Telemetry;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace IndianBank_ChatBOT
{
    /// <summary>
    /// Represents references to external services.
    ///
    /// For example, LUIS services are kept here as a singleton.  This external service is configured
    /// using the <see cref="BotConfiguration"/> class.
    /// </summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1"/>
    /// <seealso cref="https://www.luis.ai/home"/>
    public class BotServices
    {
        #region Properties

        public IServiceProvider ServiceProvider { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BotServices"/> class.
        /// </summary>
        /// <param name="botConfiguration">The <see cref="BotConfiguration"/> instance for the bot.</param>
        public BotServices(BotConfiguration botConfiguration)
        {
            foreach (var service in botConfiguration.Services)
            {
                switch (service.Type)
                {
                    case ServiceTypes.Luis:
                        {
                            var luis = service as LuisService;
                            LuisServices.Add(service.Id, new TelemetryRasaRecognizer(
                                   luis.Properties["RasaEndPoint"].Value<string>(),
                                   luis.Properties["RasaProjectName"].Value<string>()
                               )
                           );
                            break;
                        }
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets the set of LUIS Services used.
        /// Given there can be multiple <see cref="TelemetryLuisRecognizer"/> services used in a single bot,
        /// LuisServices is represented as a dictionary.  This is also modeled in the
        /// ".bot" file since the elements are named.
        /// </summary>
        /// <remarks>The LUIS services collection should not be modified while the bot is running.</remarks>
        /// <value>
        /// A <see cref="LuisRecognizer"/> client instance created based on configuration in the .bot file.
        /// </value>
        public Dictionary<string, TelemetryRasaRecognizer> LuisServices { get; } = new Dictionary<string, TelemetryRasaRecognizer>();

        /// <summary>
        /// Gets the set of QnAMaker Services used.
        /// Given there can be multiple <see cref="TelemetryQnAMaker"/> services used in a single bot,
        /// QnAServices is represented as a dictionary.  This is also modeled in the
        /// ".bot" file since the elements are named.
        /// </summary>
        /// <remarks>The QnAMaker services collection should not be modified while the bot is running.</remarks>
        /// <value>
        /// A <see cref="TelemetryQnAMaker"/> client instance created based on configuration in the .bot file.
        /// </value>

        public IConfiguration Configuration { get; set; }

    }
}
