using IndianBank_ChatBOT.Dialogs.Main;
using IndianBank_ChatBOT.Dialogs.Shared;
using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.Utils;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace IndianBank_ChatBOT.Dialogs.Onboarding
{
    public class OnBoardingFormDialog : EnterpriseDialog
    {
        private BotServices _services;
        private UserState _userState;
        private ConversationState _conversationState;
        private MainResponses _responder = new MainResponses();

        private readonly AppSettings _appSettings;

        public OnBoardingFormDialog(BotServices services, ConversationState conversationState, UserState userState, AppSettings appsettings) : base(services, nameof(OnBoardingFormDialog))
        {

            _appSettings = appsettings;
            InitialDialogId = nameof(OnBoardingFormDialog);

            _services = services ?? throw new ArgumentNullException(nameof(services));
            _conversationState = conversationState;
            _userState = userState;

            var steps = new WaterfallStep[]
          {
                AskforName,
                AskPhoneNo,
               EndOnboardingDialog
           };

            AddDialog(new WaterfallDialog(InitialDialogId, steps));
            AddDialog(new TextPrompt(DialogIds.AskforName, ValidateNameAsync));
            AddDialog(new TextPrompt(DialogIds.AskPhoneNo, ValidateMobileNumberAsync));
        }

        private async Task<bool> ValidateNameAsync(PromptValidatorContext<string> pc, CancellationToken cancellationToken)
        {
            string name = pc.Recognized.Value;

            if (name == "about us" || name == "product" || name == "services" || name == "rates" || name == "contacts" || name == "links")
            {
                await pc.Context.SendActivityAsync("Please enter your name first to proceed further");
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateMobileNumberAsync(PromptValidatorContext<string> pc, CancellationToken cancellationToken)
        {
            string mobileNumber = pc.Recognized.Value;

            if (!Regex.IsMatch(mobileNumber, "^[0-9]"))
            {
                await pc.Context.SendActivityAsync("Mobile Number can only contains digits. Please enter valid 10-Digit mobile number to proceed further.");
                return false;
            }
            if (!Regex.IsMatch(mobileNumber, "^(\\+\\d{1,3}[- ]?)?\\d{10}$"))
            {
                // await pc.Context.SendActivityAsync("Invalid Mobile Number. Please enter a valid 10-Digit number.");
                await pc.Context.SendActivityAsync("Please enter a valid 10-Digit number");

                return false;
            }
            return true;
        }

        public async Task<DialogTurnResult> AskforName(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var prompt = MessageFactory.Text("Please enter your name to get me started");

            return await stepContext.PromptAsync(DialogIds.AskforName, new PromptOptions
            {
                Prompt = prompt,
            }, cancellationToken);
        }

        public async Task<DialogTurnResult> AskPhoneNo(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string userName = stepContext.Result as string;
            stepContext.Values["UserName"] = stepContext.Result;
            string ApplicantName = userName.First().ToString().ToUpper() + userName.Substring(1);

            var prompt = MessageFactory.Text($"Thank you! {ApplicantName}, Could you please enter your phone number now?");
            return await stepContext.PromptAsync(DialogIds.AskPhoneNo, new PromptOptions
            {
                Prompt = prompt,
            }, cancellationToken);
        }

        public async Task<DialogTurnResult> EndOnboardingDialog(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string userPhoneNumber = stepContext.Result as string;
            stepContext.Values["UserPhoneNumber"] = stepContext.Result;
            string ApplicantPhoneNumber = userPhoneNumber;
            await stepContext.Context.SendActivityAsync($"Thanks {stepContext.Values["UserName"]} for providing all the information.\n  Feel free to ask me any question by typing below or clicking on the dynamic scroll bar options for specific suggestions.");
            //await stepContext.Context.SendActivityAsync("Please find the menu.");
            //await _responder.ReplyWith(stepContext.Context, MainResponses.ResponseIds.BuildWelcomeMenuCard);

            //var cs = "Server=localhost;Port=5432;Database=IndianBankDb;User Id=postgres;Password=postgres";

            var cs = _appSettings.ConnectionString;

            using (var dbContext = new AppDbContext(cs))
            {

                try
                {
                    var userName = stepContext.Values["UserName"].ToString();
                    var userInfo = new UserInfo
                    {
                        Id = 0,
                        Name = userName,
                        PhoneNumber = userPhoneNumber,
                        ConversationId = stepContext.Context.Activity.Conversation.Id,
                        CreatedOn = DateTime.Now,
                    };

                    var data = await dbContext.UserInfos.AddAsync(userInfo);

                    var result = await dbContext.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            BotChatActivityLogger.UpdateOnBoardingMessageFlag(stepContext.Context.Activity.Conversation.Id);

            return await stepContext.EndDialogAsync();
        }

        public class DialogIds
        {
            public const string AskforName = "AskforName";
            public const string AskPhoneNo = "AskPhoneNo";
            public const string EndOnboardingDialog = "EndOnboardingDialog";
        }
    }
}
