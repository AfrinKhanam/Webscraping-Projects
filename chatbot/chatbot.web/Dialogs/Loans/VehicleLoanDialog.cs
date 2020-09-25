using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using IndianBank_ChatBOT.Dialogs.Main;
using IndianBank_ChatBOT.Dialogs.Shared;
using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.Utils;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace IndianBank_ChatBOT.Dialogs.Loans
{
    public class VehicleLoanDialog : EnterpriseDialog
    {
        private BotServices _services;
        private MainResponses _responder = new MainResponses();

        public VehicleLoanDialog(BotServices services) : base(services, nameof(VehicleLoanDialog))
        {
            InitialDialogId = nameof(VehicleLoanDialog);

            _services = services ?? throw new ArgumentNullException(nameof(services));

            var steps = new WaterfallStep[]
            {
                AskVehicleLoanType,
                AskOccupationType,
                AskMonthlySalary,
                AskLoanFor,
                DisplayVehicleLoanForm,

                AskApplicantName,
                AskApplicantMobileNumber,
                CollectOthersInformation
            };

            AddDialog(new WaterfallDialog(InitialDialogId, steps));
            AddDialog(new ChoicePrompt(DialogIds.AskVehicleLoanType));
            AddDialog(new ChoicePrompt(DialogIds.AskOccupationType));
            AddDialog(new TextPrompt(DialogIds.AskMonthlySalary, ValidateMonthlySalary));
            AddDialog(new ChoicePrompt(DialogIds.AskLoanFor));
            AddDialog(new ChoicePrompt(DialogIds.DisplayVehicleLoanForm));

            AddDialog(new TextPrompt(DialogIds.AskApplicantName));
            AddDialog(new TextPrompt(DialogIds.AskApplicantMobileNumber, ValidateMobileNumberAsync));
            // AddDialog(new TextPrompt(DialogIds.AskApplicantEmailID,ValidateEmailAsync));

            WaterfallStep[] applyVehicleLoanForSelf = new WaterfallStep[]
            {
                AskApplicantEmailID,
                 AskApplicantArea,
                AskApplicantAddress,
                AskVehicleLoanAmount,
                AskApplicantState,
                AskApplicantCity,
                AskApplicantBranch,
                SendConfirmEmail,
            };

            AddDialog(new WaterfallDialog(DialogIds.ApplyVehicleLoanForSelf, applyVehicleLoanForSelf));
            AddDialog(new TextPrompt(DialogIds.AskApplicantEmailID, ValidateEmailAsync));
            AddDialog(new TextPrompt(DialogIds.AskApplicantArea));
            AddDialog(new TextPrompt(DialogIds.AskApplicantAddress));
            AddDialog(new TextPrompt(DialogIds.AskVehicleLoanAmount, ValidateVehicleLoanAmount));
            AddDialog(new TextPrompt(DialogIds.AskApplicantState));
            AddDialog(new TextPrompt(DialogIds.AskApplicantCity));
            AddDialog(new TextPrompt(DialogIds.AskApplicantBranch));
            AddDialog(new TextPrompt(DialogIds.SendConfirmEmail));
        }
        private async Task<DialogTurnResult> AskVehicleLoanType(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choices = new List<string> { "Two Wheeler Loan", "Four Wheeler Loan" };

            var actions = choices.Select(i => new CardAction(text: i, value: i, type: ActionTypes.ImBack)).ToList();

            var prompt = MessageFactory.Text("What kind of vehicle loan are you looking for");

            prompt.SuggestedActions = new SuggestedActions(actions: actions);

            return await stepContext.PromptAsync(DialogIds.AskVehicleLoanType, new PromptOptions
            {
                Prompt = prompt,
                Choices = new List<Choice>
                {
                   new Choice { Value = "Two Wheeler Loan",Synonyms=new List<string>{"2 wheeler","two wheeler", "bike", "scooter", "scooty", "2 wheel", "2 w", "two  wheeler", "2", "two" } },
                   new Choice { Value = "Four Wheeler Loan",Synonyms=new List<string>{ "4 wheeler", "four wheeler", "car", "truck", "motor vehicle", "4 wheel", "4 w", "four  wheeler", "4", "four","jeep","bus","motor car","van" } }
                }
            }, cancellationToken);

        }

        private async Task<DialogTurnResult> AskOccupationType(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var selectedLoanType = (stepContext.Result as FoundChoice)?.Value;

            stepContext.Values["LoanType"] = selectedLoanType;

            var langCode = string.Empty;

            if (selectedLoanType.Equals("Two Wheeler Loan"))
            {
                var choices = new[] { "Salaried", "Professional/Business" };

                var actions = choices.Select(i => new CardAction(text: i, value: i, type: ActionTypes.ImBack)).ToList();

                var prompt = MessageFactory.Text("Please select the type of Occupation");

                prompt.SuggestedActions = new SuggestedActions(actions: actions);

                return await stepContext.PromptAsync(DialogIds.AskOccupationType, new PromptOptions
                {
                    Prompt = prompt,
                    Choices = ChoiceFactory.ToChoices(choices)
                }, cancellationToken);
            }
            else
            {
                var choices = new[] { "Salaried", "Professional/Business" };

                var actions = choices.Select(i => new CardAction(text: i, value: i, type: ActionTypes.ImBack)).ToList();

                var prompt = MessageFactory.Text("Please select the type of Occupation.");

                prompt.SuggestedActions = new SuggestedActions(actions: actions);

                return await stepContext.PromptAsync(DialogIds.AskOccupationType, new PromptOptions
                {
                    Prompt = prompt,
                    Choices = new List<Choice>
                     {
                        new Choice { Value = "Salaried",Synonyms=new List<string>{"job","i have a job", "am working", "working", "office", "paid", "employeed", "office", "employee", "day job" } },
                        new Choice { Value = "Professional/Business",Synonyms=new List<string>{ "professional", "business", "i have my own business", "executive", "marketing", "am an artist" } }
                     }
                }, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> AskMonthlySalary(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["AskLoanLookingFor"] = stepContext.Result;

            var prompt = MessageFactory.Text("Please specify your gross monthly income.");

            return await stepContext.PromptAsync(DialogIds.AskMonthlySalary, new PromptOptions
            {
                Prompt = prompt,
            }, cancellationToken);
        }

        private async Task<bool> ValidateMonthlySalary(PromptValidatorContext<string> pc, CancellationToken cancellationToken)
        {
            string monthlySalary = pc.Recognized.Value;
            int grossMonthlySalary = AmountUtils.Parse(monthlySalary);
            pc.Context.TurnState["MonthlyGrossSalary"] = grossMonthlySalary;
            if (grossMonthlySalary == 0)
            {
                await pc.Context.SendActivityAsync("Invalid Amount \n Please specify your gross monthly salary.");
                return false;
            }
            else
            {
                return true;
            }
        }

        private async Task<DialogTurnResult> AskLoanFor(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            int monthlyGrossIncome = (int)stepContext.Context.TurnState["MonthlyGrossSalary"];
            var prompt = MessageFactory.Text("Are you applying loan for.");
            if (monthlyGrossIncome >= 20000)
            {
                return await stepContext.PromptAsync(DialogIds.AskLoanFor, new PromptOptions
                {
                    Prompt = prompt,
                    Choices = new List<Choice>
                {
                   new Choice { Value = "Self",Synonyms=new List<string>{"myself","me","i" } },
                   new Choice { Value = "Others",Synonyms=new List<string>{ "my friend", "my colleague", "my relative"} }
                }
                }, cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync("Sorry, We coundn't proceed your loan, as your gross monthly salary is less than 20,000 rs.\n Thanks for using Iva ChatBot.");
                await _responder.ReplyWith(stepContext.Context, MainResponses.ResponseIds.FeedBack);
                return await stepContext.EndDialogAsync(cancellationToken);
            }
        }

        private async Task<DialogTurnResult> DisplayVehicleLoanForm(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var loanApplicant = (stepContext.Result as FoundChoice)?.Value;
            var info = stepContext.Options as VehicleLoanDetails;
            VehicleLoanDetails vehicleLoanDetails = new VehicleLoanDetails
            {
                LoanFor = Convert.ToString(loanApplicant),
                Name = info.Name,
                //   Email = info.Email,
                Mobile = info.Mobile,
                LoanType = stepContext.Values["LoanType"] as string

            };

            stepContext.Values["LoanFor"] = loanApplicant;
            if (loanApplicant.Equals("Self"))
            {
                //return await stepContext.ContinueDialogAsync(cancellationToken);
                return await stepContext.BeginDialogAsync(DialogIds.ApplyVehicleLoanForSelf, vehicleLoanDetails, cancellationToken);
            }
            else
            {
                // var info = stepContext.Options as VehicleLoanDetails;
                return await stepContext.ContinueDialogAsync();

            }

        }

        private async Task<DialogTurnResult> AskApplicantName(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var prompt = MessageFactory.Text("Please enter applicant name.");
            return await stepContext.PromptAsync(DialogIds.AskApplicantName, new PromptOptions
            {
                Prompt = prompt,
            }, cancellationToken);
        }

        private async Task<DialogTurnResult> AskApplicantMobileNumber(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string applicantName = stepContext.Result as string;
            stepContext.Values["ApplicantName"] = stepContext.Result;
            var prompt = MessageFactory.Text("Please enter applicant Mobile Number.");
            return await stepContext.PromptAsync(DialogIds.AskApplicantMobileNumber, new PromptOptions
            {
                Prompt = prompt,
            }, cancellationToken);
        }

        /// <summary>
        /// Validates the mobile number asynchronous.
        /// </summary>
        /// <param name="pc">The pc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task<bool> ValidateMobileNumberAsync(PromptValidatorContext<string> pc, CancellationToken cancellationToken)
        {
            string mobileNumber = pc.Recognized.Value;

            if (!Regex.IsMatch(mobileNumber, "^[0-9]"))
            {
                await pc.Context.SendActivityAsync("Mobile Number can only contains digits. Please enter valid Mobile Number to proceed further.");
                return false;
            }
            if (!Regex.IsMatch(mobileNumber, "^(\\+\\d{1,3}[- ]?)?\\d{10}$"))
            {
                await pc.Context.SendActivityAsync("Invalid Mobile Number. Please enter valid Mobile Number.");
                return false;
            }
            return true;
        }

        //private async Task<DialogTurnResult> AskApplicantEmailID(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //{
        //    string applicantMobileNumber = stepContext.Result as string;
        //    stepContext.Values["ApplicantMobileNumber"] = stepContext.Result;
        //    var prompt = MessageFactory.Text("Please enter applicant Email ID.");
        //    return await stepContext.PromptAsync(DialogIds.AskApplicantEmailID, new PromptOptions
        //    {
        //        Prompt = prompt
        //    }, cancellationToken);
        //}

        private async Task<bool> ValidateEmailAsync(PromptValidatorContext<string> pc, CancellationToken cancellationToken)
        {
            var email = pc.Recognized.Value;

            if (!Regex.IsMatch(email, "^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$"))
            {
                await pc.Context.SendActivityAsync("Invalid Email ID.\n Please enter valid Email ID");
                return false;
            }

            return true;
        }

        private async Task<DialogTurnResult> CollectOthersInformation(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // string applicantEmailID = stepContext.Result as string;
            //stepContext.Values["ApplicantEmailID"] = stepContext.Result;

            string applicantMobileNumber = stepContext.Result as string;
            stepContext.Values["ApplicantMobileNumber"] = stepContext.Result;
            VehicleLoanDetails vehicleLoanDetails = new VehicleLoanDetails
            {
                Name = Convert.ToString(stepContext.Values["ApplicantName"]),
                Mobile = Convert.ToString(stepContext.Values["ApplicantMobileNumber"]),
                // Email= Convert.ToString(stepContext.Values["ApplicantEmailID"]),
                LoanFor = Convert.ToString(stepContext.Values["LoanFor"]),
                LoanType = Convert.ToString(stepContext.Values["LoanType"])
            };
            return await stepContext.ReplaceDialogAsync(DialogIds.ApplyVehicleLoanForSelf, vehicleLoanDetails, cancellationToken);
        }

        private async Task<DialogTurnResult> AskApplicantEmailID(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var info = stepContext.Options as VehicleLoanDetails;

            var prompt = MessageFactory.Text(" Please enter Email ID.");
            return await stepContext.PromptAsync(DialogIds.AskApplicantEmailID, new PromptOptions
            {
                Prompt = prompt
            }, cancellationToken);
        }

        private async Task<DialogTurnResult> AskApplicantArea(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string applicantEmailID = stepContext.Result as string;
            stepContext.Values["ApplicantEmailID"] = stepContext.Result;
            var info = stepContext.Options as VehicleLoanDetails;

            var prompt = MessageFactory.Text(" Please enter the area where you reside in.");
            return await stepContext.PromptAsync(DialogIds.AskApplicantArea, new PromptOptions
            {
                Prompt = prompt
            }, cancellationToken);
        }

        private async Task<DialogTurnResult> AskApplicantAddress(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string applicantArea = stepContext.Result as string;
            stepContext.Values["ApplicantArea"] = stepContext.Result;
            var prompt = MessageFactory.Text("Please enter your address");
            return await stepContext.PromptAsync(DialogIds.AskApplicantAddress, new PromptOptions
            {
                Prompt = prompt
            }, cancellationToken);
        }

        private async Task<DialogTurnResult> AskVehicleLoanAmount(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string applicantAddress = stepContext.Result as string;
            stepContext.Values["ApplicantAddress"] = stepContext.Result;
            var prompt = MessageFactory.Text("Please enter loan amount you are looking for.");
            return await stepContext.PromptAsync(DialogIds.AskVehicleLoanAmount, new PromptOptions
            {
                Prompt = prompt
            }, cancellationToken);
        }

        private async Task<bool> ValidateVehicleLoanAmount(PromptValidatorContext<string> pc, CancellationToken cancellationToken)
        {
            string principalAmount = pc.Recognized.Value;
            int vehicleLoanAmount = AmountUtils.Parse(principalAmount);
            pc.Context.TurnState["VehicleLoanAmount"] = vehicleLoanAmount;
            if (vehicleLoanAmount == 0)
            {
                await pc.Context.SendActivityAsync("Invalid Amount. Please specify the correct loan amount");
                await pc.Context.SendActivityAsync("Please enter your gross monthly income.");
                return false;
            }
            else
            {
                return true;
            }

        }

        private async Task<DialogTurnResult> AskApplicantState(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            int applicantLoanAmount = (int)stepContext.Context.TurnState["VehicleLoanAmount"];
            stepContext.Values["applicantLoanAmount"] = applicantLoanAmount;
            var prompt = MessageFactory.Text("Please enter the state you reside in.");
            return await stepContext.PromptAsync(DialogIds.AskApplicantState, new PromptOptions
            {
                Prompt = prompt
            }, cancellationToken);
        }

        private async Task<DialogTurnResult> AskApplicantCity(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string applicantState = stepContext.Result as string;
            stepContext.Values["ApplicantState"] = stepContext.Result;
            var prompt = MessageFactory.Text("Please enter the city you reside in");
            return await stepContext.PromptAsync(DialogIds.AskApplicantCity, new PromptOptions
            {
                Prompt = prompt
            }, cancellationToken);
        }

        private async Task<DialogTurnResult> AskApplicantBranch(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string applicantCity = stepContext.Result as string;
            stepContext.Values["ApplicantCity"] = stepContext.Result;
            var prompt = MessageFactory.Text("Please enter the branch.");
            return await stepContext.PromptAsync(DialogIds.AskApplicantBranch, new PromptOptions
            {
                Prompt = prompt
            }, cancellationToken);
        }

        private async Task<DialogTurnResult> SendConfirmEmail(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var info = stepContext.Options as VehicleLoanDetails;
            string applicantBranch = stepContext.Result as string;
            stepContext.Values["ApplicantBranch"] = stepContext.Result;
            VehicleLoanDetails vehicleLoanDetails = new VehicleLoanDetails
            {
                Name = info.Name,
                Mobile = info.Mobile,
                LoanFor = info.LoanFor,
                LoanType = info.LoanType,
                Email = Convert.ToString(stepContext.Values["ApplicantEmailID"]),
                Area = Convert.ToString(stepContext.Values["ApplicantArea"]),
                Address = Convert.ToString(stepContext.Values["ApplicantAddress"]),
                LoanAmount = Convert.ToString(stepContext.Values["applicantLoanAmount"]),
                State = Convert.ToString(stepContext.Values["ApplicantState"]),
                City = Convert.ToString(stepContext.Values["ApplicantCity"]),
                Branch = Convert.ToString(stepContext.Values["ApplicantBranch"]),


            };


            //Sending an Welcome Email to applicant
            if (!string.IsNullOrEmpty(vehicleLoanDetails.Email))
            {
                EmailDetails welcomeEmailDetails = new EmailDetails
                {
                    To = vehicleLoanDetails.Email,
                    From = "ravih5826@gmail.com",
                    Subject = $"Request for the {vehicleLoanDetails.LoanType} is submitted.",
                    Body = $"Dear {vehicleLoanDetails.Name},<br><br> Your request for the {vehicleLoanDetails.LoanType} is submittd and we have started processing the loan request. Our loan management team will get back to you soon.<br> <br>  Thanks & Regards, <br>Iva Loan Bot Team"
                };

                EmailUtility.SendMail(welcomeEmailDetails);
            }

            //Sending an email to loan bot team
            EmailDetails loanTeamEmailDetails = new EmailDetails
            {
                To = vehicleLoanDetails.Email,
                From = "ravih5826@gmail.com",
                Subject = $"New Lead for the {vehicleLoanDetails.LoanType} is created by the Iva BOT.",
                Body = $"<html> Dear Loan management Team, <br> Iva Bot found a new request for a loan. Below are the details for the loan.<br> <body> <table column=2> <tr> <th>Name</th> <td> {vehicleLoanDetails.Name}</td> </tr> <tr> <th>Mobile</th> <td> {vehicleLoanDetails.Mobile}</td> </tr> <tr> <th>Email</th> <td> {vehicleLoanDetails.Email}</td> </tr> <tr> <th>Area</th> <td> {vehicleLoanDetails.Area}</td> </tr> <tr> <th>Address</th> <td> {vehicleLoanDetails.Address}</td> </tr> <tr> <th>LoanType</th> <td> {vehicleLoanDetails.LoanType}</td> </tr> <tr> <th>LoanAmount</th> <td> {vehicleLoanDetails.LoanAmount}</td> </tr> <tr> <th>State</th> <td> {vehicleLoanDetails.State}</td> </tr> <tr> <th>City</th> <td> {vehicleLoanDetails.City}</td> </tr> <tr> <th>Branch</th> <td> {vehicleLoanDetails.Branch}</td> </tr> </table> <br> <br>  Thanks & Regards, <br>Iva Loan Bot Team </body> </html>"
            };

            EmailUtility.SendMail(loanTeamEmailDetails);

            await stepContext.Context.SendActivityAsync("Thanks for filling up the form, an Indian Bank representative will be reaching out to you at the earliest.");
            await stepContext.Context.SendActivityAsync("Thank you for talking to me. Hope you found this vehicle loan service useful.");
            await _responder.ReplyWith(stepContext.Context, MainResponses.ResponseIds.FeedBack);
            return await stepContext.CancelAllDialogsAsync();
        }
        protected override async Task<InterruptionStatus> OnInterruptingIntent(String intent, DialogContext dc, CancellationToken cancellationToken)
        {
            switch (intent)
            {
                case "Cancel":
                    await dc.Context.SendActivityAsync("meow");
                    return InterruptionStatus.Interrupted;
                case "down_payment_amount":
                    await dc.Context.SendActivityAsync("* 15% for New Vehicle.\n * 40% for used vehicle (Four wheeler).");
                    return InterruptionStatus.Interrupted;
                case "repayment_period_of_the_vehicle_loan":
                    await dc.Context.SendActivityAsync("* Four wheeler: Maximum 84 EMIs (No holiday period) \n * For 2 wheeler: Maximum 60 EMIs(No holiday period)*Used Four wheeler: Repayment period based on the age of the vehicle subject to a maximum of 60 months(conditions apply)");
                    return InterruptionStatus.Interrupted;
                case "processing_fee_for_vehicle_loan":
                    await dc.Context.SendActivityAsync("0.230% on loan amount with a max. of Rs.10236/-");
                    return InterruptionStatus.Interrupted;
                case "interest_rates_on_vehicle_loan":
                    await dc.Context.SendActivityAsync("meow");
                    return InterruptionStatus.Interrupted;

            }

            return await base.OnInterruptingIntent(intent.ToString(), dc, cancellationToken);
        }

        public class DialogIds
        {
            public const string AskVehicleLoanType = "AskVehicleLoanType";
            public const string AskOccupationType = "AskOccupationType";
            public const string AskMonthlySalary = "AskMonthlySalary";
            public const string AskLoanFor = "AskLoanFor";
            public const string DisplayVehicleLoanForm = "DisplayVehicleLoanForm";

            public const string ApplyVehicleLoanForSelf = "ApplyVehicleLoanForSelf";
            public const string AskApplicantArea = "AskApplicantArea";
            public const string AskApplicantAddress = "AskApplicantAddress";
            public const string AskVehicleLoanAmount = "AskVehicleLoanAmount";
            public const string AskApplicantState = "AskApplicantState";
            public const string AskApplicantCity = "AskApplicantCity";
            public const string AskApplicantBranch = "AskApplicantBranch";
            public const string SendConfirmEmail = "SendConfirmEmail";

            //public const string ApplyVehicleLoanForOthers = "ApplyVehicleLoanForOthers";

            public const string AskApplicantName = "AskApplicantName";
            public const string AskApplicantMobileNumber = "AskApplicantMobileNumber";
            public const string AskApplicantEmailID = "AskApplicantEmailID";
        }
    }

}

