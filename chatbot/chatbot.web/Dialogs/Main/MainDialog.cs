using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using IndianBank_ChatBOT.Dialogs.EMI;
using IndianBank_ChatBOT.Dialogs.Loans;
using IndianBank_ChatBOT.Dialogs.Onboarding;
using IndianBank_ChatBOT.Dialogs.Shared;
using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.Utils;
using IndianBank_ChatBOT.ViewModel;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
// using System.Net.WebUtility;

namespace IndianBank_ChatBOT.Dialogs.Main
{
    public class MainDialog : RouterDialog
    {
        private readonly BotServices _services;
        private readonly AppSettings appSettings;
        private readonly MainResponses _responder = new MainResponses();

        private readonly AppDbContext dbContext;
        private readonly IHttpClientFactory clientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainDialog"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="conversationState">State of the conversation.</param>
        /// <param name="userState">State of the user.</param>
        /// <exception cref="ArgumentNullException">services</exception>
        public MainDialog(BotServices services, AppSettings appSettings, AppDbContext dbContext, IHttpClientFactory clientFactory)
            : base(nameof(MainDialog))
        {
            this.dbContext = dbContext;
            this.clientFactory = clientFactory;
            _services = services ?? throw new ArgumentNullException(nameof(services));
            this.appSettings = appSettings;
            AddDialog(new VehicleLoanDialog(_services));
            AddDialog(new OnBoardingFormDialog(_services, dbContext));
            AddDialog(new EMICalculatorDialog(_services));
        }

        #region methods

        /// <summary>
        /// Called when /[start asynchronous].
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected override async Task OnStartAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            var view = new MainResponses();
            await dc.Context.SendActivityAsync("Hi! My name is ADYA \U0001F603.\n Welcome to Indian Bank.\n I am your virtual assistant, here to assist you with all your banking queries 24x7");
            //  await view.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);

            await dc.BeginDialogAsync(nameof(OnBoardingFormDialog));
        }

        /// <summary>
        /// Routes the asynchronous.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// The specified LUIS Model could not be found in your Bot Services configuration.
        /// or
        /// The specified QnA Maker Service could not be found in your Bot Services configuration.
        /// or
        /// The specified QnA Maker Service could not be found in your Bot Services configuration.
        /// </exception>
        protected override async Task RouteAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            string utterance = dc.Context.Activity.Text;
            int utterance_word_count = utterance.Split(" ").Length;

            _services.LuisServices.TryGetValue("general", out var luisService);

            if (luisService == null)
            {
                throw new Exception("The specified LUIS Model could not be found in your Bot Services configuration.");

            }
            else if (luisService != null)
            {
                string entityName = string.Empty;
                string entityType = string.Empty;

                var conversationID = dc.Context.Activity.Conversation.Id;

                var userInfo = dbContext.UserInfos.FirstOrDefault(e => e.ConversationId == conversationID);

                var result = await luisService.RecognizeAsync(dc.Context, CancellationToken.None);

                var entityTypes = new List<string>
                {
                    "what_entity",
                    "why_entity",
                    "can_entity",
                    "how_entity",
                    "when_entity",
                    "where_entity",
                    "which_entity",
                    "who_entity",
                    "scrollbar_entity",
                    "aboutus_entity",
                    "product_entity",
                    "services_entity",
                    "rates_entity",
                    "customersupport_entity",
                    "link_entity",
                    "atm_entity",
                    "lost_entity"
                };

                foreach (var entity in entityTypes)
                {
                    if (result.Entities[entity] != null)
                    {
                        entityType = entity;
                        entityName = result.Entities[entity].Values<string>().FirstOrDefault();
                        break;
                    }
                }

                var generalIntent = result.GetTopScoringIntent().intent;
                var generalIntentScore = result.GetTopScoringIntent().score;

                Console.WriteLine(generalIntent, generalIntentScore);

                if (entityType == "scrollbar_entity")
                {
                    ScrollBarDialog.DisplayScrollBarMenu(dc, entityName, appSettings.QAEndPoint, clientFactory);
                    await dc.EndDialogAsync();
                }
                else if (generalIntent == "thankyouintent")
                {
                    var messageData = result.Text.First().ToString().ToUpper() + result.Text.Substring(1);
                    await dc.Context.SendActivityAsync($"{messageData}!!! {userInfo.Name}. It was nice talking to you today.");
                }
                else if (utterance.Split(" ")[utterance_word_count - 1].Equals("services") || (utterance.Split(" ")[utterance_word_count - 1].Equals("plus")) || (utterance.Split(" ")[utterance_word_count - 1].Equals("banking")) || (utterance.Split(" ")[utterance_word_count - 1].Equals("payment")) || (utterance.Split(" ")[utterance_word_count - 1].Equals("trust")))
                {
                    if (utterance.Trim() == "premium services" || utterance.Trim() == "insurance services" || utterance.Trim() == "cms plus" || utterance.Trim() == "doorstep banking" || utterance.Trim() == "tax payment" || utterance.Trim() == "debenture trust")
                    {
                        SampleFAQDialog.DisplaySampleFAQ(dc, entityType, entityName, appSettings.QAEndPoint, clientFactory);
                        await dc.EndDialogAsync();
                    }
                    else
                    {
                        await SearchKB(dc, appSettings.QAEndPoint, clientFactory);
                    }
                }
                else if (utterance.Split(" ")[utterance_word_count - 1].Equals("products"))
                {
                    if (utterance.Trim() == "loan products" || utterance.Trim() == "deposit products" || utterance.Trim() == "digital products" || utterance.Trim() == "feature products")
                    {
                        SampleFAQDialog.DisplaySampleFAQ(dc, entityType, entityName, appSettings.QAEndPoint, clientFactory);
                        await dc.EndDialogAsync();
                    }
                    else
                    {
                        await SearchKB(dc, appSettings.QAEndPoint, clientFactory);

                    }
                }
                else if (utterance.Split(" ")[utterance_word_count - 1].Equals("rates") || utterance.Split(" ")[utterance_word_count - 1].Equals("charges"))
                {
                    if (utterance.Trim() == "deposit rates" || utterance.Trim() == "lending rates" || utterance.Trim() == "service charges")
                    {
                        SampleFAQDialog.DisplaySampleFAQ(dc, entityType, entityName, appSettings.QAEndPoint, clientFactory);
                        await dc.EndDialogAsync();
                    }
                    else
                    {
                        await SearchKB(dc, appSettings.QAEndPoint, clientFactory);

                    }
                }
                else if (utterance.Split(" ")[utterance_word_count - 1].Equals("profiles") || utterance.Split(" ")[utterance_word_count - 1].Equals("mission") || utterance.Split(" ")[utterance_word_count - 1].Equals("management") || utterance.Split(" ")[utterance_word_count - 1].Equals("governance") || utterance.Split(" ")[utterance_word_count - 1].Equals("fund") || utterance.Split(" ")[utterance_word_count - 1].Equals("report"))
                {
                    if (utterance.Trim() == "profiles" || utterance.Trim() == "vision and mission" || utterance.Trim() == "management" || utterance.Trim() == "management" || utterance.Trim() == "corporate governance" || utterance.Trim() == "mutual fund" || utterance.Trim() == "annual report")
                    {
                        SampleFAQDialog.DisplaySampleFAQ(dc, entityType, entityName, appSettings.QAEndPoint, clientFactory);
                        await dc.EndDialogAsync();
                    }
                    else
                    {
                        await SearchKB(dc, appSettings.QAEndPoint, clientFactory);

                    }
                }
                else if (utterance.Split(" ")[utterance_word_count - 1].Equals("service") || utterance.Split(" ")[utterance_word_count - 1].Equals("sites") || utterance.Split(" ")[utterance_word_count - 1].Equals("alliances"))
                {
                    if (utterance.Trim() == "online service" || utterance.Trim() == "related sites" || utterance.Trim() == "alliances")
                    {
                        SampleFAQDialog.DisplaySampleFAQ(dc, entityType, entityName, appSettings.QAEndPoint, clientFactory);
                        await dc.EndDialogAsync();
                    }
                    else
                    {
                        await SearchKB(dc, appSettings.QAEndPoint, clientFactory);

                    }
                }
                else if (utterance.Trim().ToLower() == "hi" || utterance.Trim().ToLower() == "hello" || utterance.Trim().ToLower() == "hey" || utterance.Trim().ToLower() == "good morning" || (utterance.Trim().ToLower() == "hii") || utterance.Trim().ToLower() == "greetings" || utterance.Trim().ToLower() == "whats up")
                {
                    var messageData = result.Text.First().ToString().ToUpper() + result.Text.Substring(1);
                    await dc.Context.SendActivityAsync($"{messageData}!!! {userInfo.Name}. How may I help you today?");
                }
                else if (utterance.Trim().ToLower() == "bye" || utterance.Trim().ToLower() == "bye bye" || utterance.Trim().ToLower() == "good bye" || utterance.Trim().ToLower() == "take care" || (utterance.Trim().ToLower() == "tata"))
                {
                    var messageData = result.Text.First().ToString().ToUpper() + result.Text.Substring(1);
                    await dc.Context.SendActivityAsync($"{messageData}!!! {userInfo.Name}. It was nice talking to you today.");

                }
                else if (generalIntentScore > 0.75)
                {
                    var messageData = result.Text.First().ToString().ToUpper() + result.Text.Substring(1);

                    if (generalIntent == "small_talks_intent")
                    {
                        await dc.Context.SendActivityAsync($"Hello!! I'm IVA, your Indian Bank Virtual Assistant");
                    }
                    else if (generalIntent == "capabilities_intent")
                    {
                        await dc.Context.SendActivityAsync($"I am here to assist you with all your banking queries 24x7. \n Feel free to ask me any question by typing below or clicking on the dynamic scroll bar options for specific suggestions.");
                    }
                    else if (generalIntent == "bye_intent")
                    {
                        await dc.Context.SendActivityAsync($"{messageData}!!! {userInfo.Name}. It was nice talking to you today.");
                    }
                    else if (entityType == "atm_entity")
                    {
                        await dc.Context.SendActivityAsync("Please click on the URL below to find all Indian Bank ATM/Branch Locations. \n\n https://www.indianbank.in/branch-atm/");
                    }
                    else if (entityType == "lost_entity")
                    {
                        await dc.Context.SendActivityAsync($"Looks like your query requires futher assistance. Please contact customer care immediately on the following number's : \n\n <tel:180042500000> /  <tel:18004254422>");
                    }
                    else
                    {
                        await SearchKB(dc, appSettings.QAEndPoint, clientFactory);
                    }
                }
                else
                {
                    await SearchKB(dc, appSettings.QAEndPoint, clientFactory);
                }
            }
        }

        public static async Task SearchKB(DialogContext dc, string qaEndPoint, IHttpClientFactory clientFactory)
        {
            var query = dc.Context.Activity.Text;

            var context = string.Empty;

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{qaEndPoint}?query={System.Net.WebUtility.UrlEncode(query)}&context={context}"))
            {
                using (var client = clientFactory.CreateClient())
                {
                    var response = await client.SendAsync(request);

                    var data = string.Empty;

                    if (response.IsSuccessStatusCode)
                    {
                        data = await response.Content.ReadAsStringAsync();
                    }

                    await DisplayBackendResult(dc, context, data);
                }
            }
        }

        /// <summary>
        /// Called when [event asynchronous].
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected override async Task OnEventAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Check if there was an action submitted from intro card
            if (dc.Context.Activity.Value != null)
            {
                dynamic value = dc.Context.Activity.Value;

                if (dc.Context.Activity.Value != null)
                {
                    if (value.action == "submit")
                    {
                        string name = value.name;
                        string phone = value.phone;
                        string email = value.email;
                        //checks if no fields are emplty
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(email))
                        {
                            int phoneLength = phone.Length;
                            if (phoneLength == 10)
                            {
                                bool result = Int64.TryParse(phone, out long number);
                                if (result)
                                {
                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (isEmail)
                                    {
                                        await StoreUserDataAndDisplayFormAsync(dc, name, phone, email);
                                    }
                                    else
                                    {
                                        await dc.Context.SendActivityAsync("Please enter valid Email ID.");
                                        await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
                                    }
                                }
                                else
                                {
                                    await dc.Context.SendActivityAsync("Phone Number cannot contain alphabets. Please enter a vaild mobile number");
                                    await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
                                }

                            }
                            else
                            {
                                await dc.Context.SendActivityAsync("Your Mobile Number doesn't contain 10 digits. Please enter valid Mobile Number.");
                                await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
                            }

                        }
                        else if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(phone) && string.IsNullOrEmpty(email))
                        {
                            await dc.Context.SendActivityAsync("Please fill the form");
                            await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
                        }
                        else
                        {
                            //checks if either one of the fields is empty
                            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email))
                            {
                                if (string.IsNullOrEmpty(name))
                                {
                                    await dc.Context.SendActivityAsync("Name field cannot be empty.");
                                    await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
                                }
                                else if (string.IsNullOrEmpty(phone))
                                {
                                    await dc.Context.SendActivityAsync("Phone field cannot be empty.");
                                    await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
                                }
                                else
                                {
                                    await dc.Context.SendActivityAsync("Email field cannot be empty.");
                                    await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
                                }
                            }
                        }
                    }

                    else if (value.action == "VehicleLoanSubmit")
                    {
                        dynamic LoanDetails = dc.Context.Activity.Value;

                        VehicleLoanDetails vehicleLoanDetails = new VehicleLoanDetails
                        {
                            Name = Convert.ToString(LoanDetails.name),
                            Mobile = Convert.ToString(LoanDetails.mobile),
                            Email = Convert.ToString(LoanDetails.email),
                            Area = Convert.ToString(LoanDetails.area),
                            Address = Convert.ToString(LoanDetails.address),
                            LoanType = Convert.ToString(LoanDetails.loanType),
                            LoanAmount = Convert.ToString(LoanDetails.loanAmount),
                            State = Convert.ToString(LoanDetails.state),
                            City = Convert.ToString(LoanDetails.city),
                            Branch = Convert.ToString(LoanDetails.branch)
                        };

                        //Sending an Welcome Email to applicant
                        if (!string.IsNullOrEmpty(vehicleLoanDetails.Email))
                        {
                            EmailDetails welcomeEmailDetails = new EmailDetails
                            {
                                To = vehicleLoanDetails.Email,
                                From = "xxxxx",
                                Subject = $"Request for the {vehicleLoanDetails.LoanType} is submitted.",
                                Body = $"Your request for the {vehicleLoanDetails.LoanType} is submittd and we have started processing the loan request. Our loan team will get back to you soon."
                            };

                            EmailUtility.SendMail(welcomeEmailDetails);
                        }

                        //Sending an email to loan bot team
                        EmailDetails loanTeamEmailDetails = new EmailDetails
                        {
                            To = "xxxxxx",
                            From = "xxxxx",
                            Subject = $"New Lead for the {vehicleLoanDetails.LoanType} is created by the Iva BOT.",
                            Body = $"<html> <body> <table column=2> <tr> <th>Name</th> <td> {vehicleLoanDetails.Name}</td> </tr> <tr> <th>Mobile</th> <td> {vehicleLoanDetails.Mobile}</td> </tr> <tr> <th>Email</th> <td> {vehicleLoanDetails.Email}</td> </tr> <tr> <th>Area</th> <td> {vehicleLoanDetails.Area}</td> </tr> <tr> <th>Address</th> <td> {vehicleLoanDetails.Address}</td> </tr> <tr> <th>LoanType</th> <td> {vehicleLoanDetails.LoanType}</td> </tr> <tr> <th>LoanAmount</th> <td> {vehicleLoanDetails.LoanAmount}</td> </tr> <tr> <th>State</th> <td> {vehicleLoanDetails.State}</td> </tr> <tr> <th>City</th> <td> {vehicleLoanDetails.City}</td> </tr> <tr> <th>Branch</th> <td> {vehicleLoanDetails.Branch}</td> </tr> </table> </body> </html>"
                        };

                        EmailUtility.SendMail(loanTeamEmailDetails);

                        await dc.Context.SendActivityAsync("Thanks for filling up the form, an Indian Bank representative will be reaching out to you at the earliest.");
                        await dc.Context.SendActivityAsync("Thank you for talking to me. Hope you found this vehicle loan service useful.");
                        await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.FeedBack);

                    }
                    else if (value.action == "feedback")
                    {
                        await dc.Context.SendActivityAsync("Thank you for your response.");
                        await dc.Context.SendActivityAsync("Please find the menu as below");
                        await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.BuildWelcomeMenuCard);
                    }
                }
            }
        }

        public static async Task DisplayBackendResult(DialogContext dialogContext, string context, string backendResult)
        {
            if (string.IsNullOrEmpty(backendResult))
            {
                await dialogContext.Context.SendActivityAsync("Sorry,I could not understand. Could you please rephrase the query.");
            }
            else
            {
                var jsonObject = JsonConvert.DeserializeObject<JsonObject>(backendResult);

                if (jsonObject.DOCUMENTS.Count >= 1)
                {
                    dialogContext.Context.Activity.Conversation.Properties.Add(nameof(ExtendedLogData), JToken.FromObject(new ExtendedLogData
                    {
                        IntentName = jsonObject.DOCUMENTS[0].main_title,
                        SubTitle = jsonObject.DOCUMENTS[0].title,
                        ResponseJson = backendResult,
                        ResponseSource = ResponseSource.ElasticSearch
                    }));
                }

                var firstDoc = jsonObject.DOCUMENTS.First();

                if (!String.IsNullOrEmpty(jsonObject.FILENAME))
                {
                    var heroCard = new HeroCard
                    {
                        Text = $@"For further details please visit the page [{firstDoc.url}]({firstDoc.url})"
                    };

                    if (!string.IsNullOrWhiteSpace(firstDoc.value))
                    {
                        heroCard.Title = firstDoc.main_title;
                        heroCard.Subtitle = firstDoc.title;
                        heroCard.Text = $@"{firstDoc.value}

{heroCard.Text}";
                    }

                    if (HeroCardImageMapping.MAPPING.TryGetValue(jsonObject.FILENAME, out var imgPath))
                    {
                        heroCard.Images = new List<CardImage> { new CardImage(imgPath) };
                    }

                    var activity = MessageFactory.Attachment(heroCard.ToAttachment());

                    await dialogContext.Context.SendActivityAsync(activity);
                }
                else
                {
                    if (jsonObject.WORD_SCORE == 0)
                    {
                        await dialogContext.Context.SendActivityAsync($"Your query seems to require further assistance. Please feel free to contact customer support on the following toll free numbers: <tel:180042500000> /  <tel:18004254422> \n\n Please click on the link below for futher contact details: \n\n https://indianbank.in/departments/quick-contact/ ");
                        await dialogContext.Context.SendActivityAsync($"Please feel free to ask me anything else about Indian Bank");
                    }
                    else if (jsonObject.WORD_SCORE < 0.6)
                    {
                        await dialogContext.Context.SendActivityAsync("I did not find an exact answer but here is something similar");
                        await dialogContext.Context.SendActivityAsync($"{firstDoc.main_title}\n\n{firstDoc.title}\n\n{firstDoc.value}\n\n For further details please click on the link below:\n {firstDoc.url}");
                    }
                    else
                    {
                        await dialogContext.Context.SendActivityAsync($"This is what I found on \"{jsonObject.AUTO_CORRECT_QUERY}\"");
                        if (jsonObject.WORD_COUNT > 100)
                        {
                            await dialogContext.Context.SendActivityAsync($"{firstDoc.main_title}\n\n{firstDoc.title}\n\n{firstDoc.value} \n\n {firstDoc.url}");
                        }
                        else if (jsonObject.WORD_COUNT < 25)
                        {
                            await dialogContext.Context.SendActivityAsync($"{firstDoc.value}\n\n For further details please click on the link below:\n {firstDoc.url}");
                        }
                        else
                        {
                            var message = string.Empty;
                            var documentCount = jsonObject.DOCUMENTS.Count();

                            if (documentCount > 0)
                            {
                                message = $"{firstDoc.value}\n\n For further details please click on the link below:\n {firstDoc.url}";
                            }

                            if (documentCount > 1 && documentCount <= 2 && firstDoc.url != jsonObject.DOCUMENTS[1].url)
                            {
                                message += $"\n\n\n For additional info:\n\n{jsonObject.DOCUMENTS[1].url}";
                            }

                            if (documentCount > 2 && jsonObject.DOCUMENTS[1].url != jsonObject.DOCUMENTS[2].url)
                            {
                                message += $"\n\n{jsonObject.DOCUMENTS[2].url}";
                            }

                            await dialogContext.Context.SendActivityAsync(message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Displays User Form
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task StoreUserDataAndDisplayFormAsync(DialogContext dc, string name, string phone, string email)
        {
            var localeChangedEventActivity = dc.Context.Activity.CreateReply();
            localeChangedEventActivity.Type = ActivityTypes.Event;
            localeChangedEventActivity.ValueType = "cardsubmission";
            localeChangedEventActivity.Value = "somedata";
            await dc.Context.SendActivityAsync(localeChangedEventActivity);
            await dc.Context.SendActivityAsync($"Hi " + name + ", welcome to Indian Bank");
            await dc.Context.SendActivityAsync("Please find the menu.");
            await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.BuildWelcomeMenuCard);
        }

        public async Task CalculateEmiCalculateFormAsync(DialogContext dc, int principal, int rate, int time)
        {
            var localeChangedEventActivity = dc.Context.Activity.CreateReply();
            localeChangedEventActivity.Type = ActivityTypes.Event;
            localeChangedEventActivity.ValueType = "cardsubmission";
            localeChangedEventActivity.Value = "somedata";
            await dc.Context.SendActivityAsync(localeChangedEventActivity);

            double loanM = (rate / 1200.0);
            double numberMonths = time * 12;
            double negNumberMonths = 0 - numberMonths;
            double monthlyPayment = principal * loanM / (1 - System.Math.Pow((1 + loanM), negNumberMonths));
            await dc.Context.SendActivityAsync($"EMI per month : " + monthlyPayment.ToString());

        }

        /// <summary>
        /// Completes the asynchronous.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected override async Task CompleteAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            // The active dialog's stack ended with a complete status
            await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Completed);
        }

        #endregion


        class JsonObject
        {
            public List<DOCUMENT> DOCUMENTS { get; set; }
            public int WORD_COUNT { get; set; }
            public double WORD_SCORE { get; set; }
            public string FILENAME { get; set; }
            public string AUTO_CORRECT_QUERY { get; set; }
            public string CORRECT_QUERY { get; set; }
        }

        class DOCUMENT
        {
            public string main_title { get; set; }
            public string title { get; set; }
            public string url { get; set; }
            public string value { get; set; }

        }

        public class ImageData
        {

            public string ImagePath { get; set; }
        }

        #region Class

        public class Details
        {
            public string Name { get; set; }

            public string PhoneNumber { get; set; }
            public string EmailId { get; set; }

            public override string ToString()
            {
                return ("Name :" + Name + "\t\t" + "PhoneNumber :" + "\t\t" + PhoneNumber + "\t\t" + "EmailId :" + EmailId);
            }
        }

        #endregion
    }

    public class CustomChannelData
    {
        [JsonProperty("context")]
        public string Context { get; set; }
    }
}