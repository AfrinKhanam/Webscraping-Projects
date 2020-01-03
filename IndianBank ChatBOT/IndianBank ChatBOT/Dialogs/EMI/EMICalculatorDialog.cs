using IndianBank_ChatBOT.Dialogs.Main;
using IndianBank_ChatBOT.Dialogs.Shared;
using IndianBank_ChatBOT.Middleware;
using IndianBank_ChatBOT.Utils;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Options;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number
    ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Dialogs.EMI
{
    public class EMICalculatorDialog : EnterpriseDialog
    {
        private BotServices _services;
        private UserState _userState;
        private ConversationState _conversationState;
        private MainResponses _responder = new MainResponses();

        public EMICalculatorDialog(BotServices services, ConversationState conversationState, UserState userState) : base(services, nameof(EMICalculatorDialog))
        {

            InitialDialogId = nameof(EMICalculatorDialog);

            _services = services ?? throw new ArgumentNullException(nameof(services));
            _conversationState = conversationState;
            _userState = userState;

            var steps = new WaterfallStep[]
            {
                  AskforPrincipalAmount,
                  AskDuration,
                  CalculateEMI,
            };

            AddDialog(new WaterfallDialog(InitialDialogId, steps));
            AddDialog(new TextPrompt(DialogIds.AskforPrincipalAmount,ValidatePrincipalAmount));
            AddDialog(new TextPrompt(DialogIds.AskDuration));
            //AddDialog(new TextPrompt(DialogIds.CalculateEMI));


        }
      
        public async Task<DialogTurnResult> AskforPrincipalAmount(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
           
            var prompt = MessageFactory.Text("Please Enter Your Loan Amount");
        
            return await stepContext.PromptAsync(DialogIds.AskforPrincipalAmount, new PromptOptions
            {
                Prompt = prompt,
            }, cancellationToken);

        }

        private async Task<bool> ValidatePrincipalAmount(PromptValidatorContext<string> pc, CancellationToken cancellationToken)
        {
            string principalAmount = pc.Recognized.Value;
            int enteredPrincipalAmount = AmountUtils.Parse(principalAmount);
            pc.Context.TurnState["PrincipalAmount"] = enteredPrincipalAmount;

            if (enteredPrincipalAmount == 0)
            {
                await pc.Context.SendActivityAsync("Invalid Amount \n Please specify the correct loan amount");
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<DialogTurnResult> AskDuration(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            int principalAmount = (int)stepContext.Context.TurnState["PrincipalAmount"];
            stepContext.Values["PrincipalAmount"] = principalAmount;
            var prompt = MessageFactory.Text("Please Enter Duration in terms of Years");

            return await stepContext.PromptAsync(DialogIds.AskDuration, new PromptOptions
            {
                Prompt = prompt,
            }, cancellationToken);

        }
       

        public async Task<DialogTurnResult> CalculateEMI(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
          
            stepContext.Values["Duration"] = stepContext.Result;
            string duration=stepContext.Result as string;
            int.TryParse(duration, out int timePeriod);
            //var  pricipalAmount1 = stepContext.Values["PrincipalAmount"];
            string pricipalAmount = Convert.ToString(stepContext.Values["PrincipalAmount"]);
            long.TryParse(pricipalAmount, out long principalAmt);
            //double Rate = 12;
            
            ////double negNumberMonths = 0 - numberMonths;
            ////double month = Rate * principalAmt * numberMonths / 100;
            //double monthlyPayment = principalAmt * Rate * Math.Pow(1 + Rate, timePeriod) / (Math.Pow(1 + Rate, timePeriod) - 1);
            //// double monthlyPayment = principalAmt * loanM / (1 - System.Math.Pow((1 + loanM), negNumberMonths));
            //await stepContext.Context.SendActivityAsync($"EMI per month : " + monthlyPayment);


            double loanM = (12 / 1200.0);
            double numberMonths = timePeriod * 12;
            double negNumberMonths = 0 - numberMonths;
            double monthlyPayment = principalAmt * loanM / (1 - System.Math.Pow((1 + loanM), negNumberMonths));
            await stepContext.Context.SendActivityAsync($"EMI per month : " + monthlyPayment.ToString());
            // await stepContext.Context.SendActivityAsync($"EMI per month : 5252");
            return await stepContext.EndDialogAsync();
            //return await stepContext.PromptAsync(DialogIds.AskPhoneNo, new PromptOptions
            //{
            //    Prompt = prompt,
            //}, cancellationToken);

        }

        public class DialogIds
        {
            public const string AskforPrincipalAmount = "AskforPrincipalAmount";
            public const string AskDuration = "AskDuration";
            public const string CalculateEMI = "CalculateEMI";
        }

    }
}
