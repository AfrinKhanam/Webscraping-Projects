using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Middleware.Telemetry
{
    /// <summary>
    /// RasaRuntimeClient
    /// </summary>
    /// <seealso cref="Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.LUISRuntimeClient" />
    public class RasaRuntimeClient : LUISRuntimeClient
    {
        #region Properties

        public string RasaProjectName { get; set; }
        private IPrediction prediction;

        #endregion

        #region Constructor

        public RasaRuntimeClient(string rasaEndPoint, string rasaProjectName)
        {
            Endpoint = rasaEndPoint;
            RasaProjectName = rasaProjectName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the IPrediction.
        /// </summary>
        public override IPrediction Prediction
        {
            get
            {
                if (prediction == null)
                {
                    prediction = new RasaPrediction(this, Endpoint, RasaProjectName);
                }

                return prediction;
            }
        }

        #endregion

    }
}
