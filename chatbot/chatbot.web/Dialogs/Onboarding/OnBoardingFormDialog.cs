using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using IndianBank_ChatBOT.Dialogs.Shared;
using IndianBank_ChatBOT.Models;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace IndianBank_ChatBOT.Dialogs.Onboarding
{
    public class OnBoardingFormDialog : EnterpriseDialog
    {
        private readonly AppDbContext dbContext;

        public OnBoardingFormDialog(BotServices services, AppDbContext dbContext)
        : base(services, nameof(OnBoardingFormDialog))
        {
            InitialDialogId = nameof(OnBoardingFormDialog);

            this.dbContext = dbContext;

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
            Regex r = new Regex("^[a-zA-Z .]+$");

            if (!r.IsMatch(name))
            {
                await pc.Context.SendActivityAsync("Name can have only alphabets. Please enter a valid name.");
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
            if (!Regex.IsMatch(mobileNumber, "^(\\+\\d{1,3}[- ]?)?\\d{10}$") | (Regex.IsMatch(mobileNumber, "^[0]{10}$") | Regex.IsMatch(mobileNumber, "^[1]{10}$") | Regex.IsMatch(mobileNumber, "^[2]{10}$") |
            Regex.IsMatch(mobileNumber, "^[3]{10}$") | Regex.IsMatch(mobileNumber, "^[4]{10}$") | Regex.IsMatch(mobileNumber, "^[5]{10}$") | Regex.IsMatch(mobileNumber, "^[6]{10}$") | Regex.IsMatch(mobileNumber, "^[7]{10}$") |
            Regex.IsMatch(mobileNumber, "^[8]{10}$") | Regex.IsMatch(mobileNumber, "^[9]{10}$")))
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

            stepContext.Values["UserPhoneNumber"] = userPhoneNumber;

            var msg = $@"Thanks {stepContext.Values["UserName"]} for providing all the information.
Feel free to ask me any question by typing below or clicking on the dynamic scroll bar options for specific suggestions.";

            var onboardingCompletedEvent = Activity.CreateEventActivity();
            onboardingCompletedEvent.Value = "OnboardingCompleted";

            await stepContext.Context.SendActivitiesAsync(
                new[] { MessageFactory.Text(msg), onboardingCompletedEvent },
                cancellationToken
            );

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

                await dbContext.UserInfos.AddAsync(userInfo);

                // Update all messages exchanged so far as Onboarding Messages, so that none of the reports or analytics consider these messages as chatbot queries by the user.

                var conversationId = stepContext.Context.Activity.Conversation.Id;

                var chatLogs = dbContext.ChatLogs.Where(c => c.ConversationId == conversationId).ToList();
                chatLogs.ForEach(c => c.IsOnBoardingMessage = true);
                dbContext.ChatLogs.UpdateRange(chatLogs);

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

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

