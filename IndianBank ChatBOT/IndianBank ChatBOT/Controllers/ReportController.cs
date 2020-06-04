using IndianBank_ChatBOT.ExcelExport;
using IndianBank_ChatBOT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndianBank_ChatBOT.Controllers
{
    //[Route("[controller]")]
    public class ReportController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ReportController(AppDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FrequentlyAskedQueries(ReportParams @params)
        {
            if (!IsValidReportParams(@params))
            {
                return View("_InvalidInputError");
            }
            var vm = GetFrequentlyAskedQueriesReport(@params);
            return View(vm);
        }

        [HttpGet]
        public ActionResult FrequentlyAskedQueries()
        {
            var @params = new ReportParams
            {
                From = null,
                To = null
            };

            var vm = GetFrequentlyAskedQueriesReport(@params);
            return View(vm);
        }

        [HttpPost]
        public ActionResult AppUsers(ReportParams @params)
        {
            if (!IsValidReportParams(@params))
            {
                return View("_InvalidInputError");
            }

            var vm = GetChatBotVisitorsViewModelReport(@params);
            return View(vm);
        }

        [HttpGet]
        public ActionResult AppUsers()
        {
            ReportParams @params = new ReportParams { From = null, To = null };
            var vm = GetChatBotVisitorsViewModelReport(@params);
            return View(vm);
        }

        //todo
        [HttpGet]
        public ActionResult AppVisitors(string from, string to)
        {
            var fromDate = from;
            var toDate = to;

            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else
            {
                if (IsValidDate(fromDate) && IsValidDate(toDate))
                {
                    fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                    toDate = Convert.ToDateTime(toDate).AddDays(1).ToString("yyyy-MM-dd");
                }
                else
                {
                    fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                    toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                }
            }

            var query = $"select Visitor, PhoneNumber, Sum(NumberOfQueriesAsked) as NumberOfQueries , Sum(NumberOfVisits) as NumberOfVisits, Max(LastVisit) as LastVisited from (select Visitor, PhoneNumber, LastVisit, Sum(CountOfConversation) as NumberOfQueriesAsked, Count(ConversationId) as NumberOfVisits from (select U.\"Name\" as Visitor, U.\"PhoneNumber\" as PhoneNumber, Count(U.\"Id\") as CountOfConversation, C.\"ConversationId\" as ConversationId, Max(C.\"TimeStamp\") as LastVisit from \"ChatLogs\" C inner join \"UserInfos\" U on C.\"ConversationId\" = U.\"ConversationId\" where C.\"IsOnBoardingMessage\" is null and C.\"ReplyToActivityId\" is null and coalesce(C.\"Text\", '') != '' and \"TimeStamp\" between '{fromDate}' AND '{toDate}' group by U.\"Name\", U.\"PhoneNumber\", C.\"ConversationId\") as tmp group by tmp.Visitor, tmp.PhoneNumber, tmp.LastVisit) as queryResult group by queryResult.Visitor, queryResult.PhoneNumber";

            var users = _dbContext.ChatBotVisitorDetails.FromSqlRaw(query).ToList();

            var vm = new ChatBotVisitorsViewModel
            {
                ChatBotVisitorDetails = users,
                From = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy"),
                To = Convert.ToDateTime(toDate).AddDays(-1).ToString("dd-MMM-yyyy")
            };

            return View(vm);
        }

        [HttpGet]
        public ActionResult UnAnsweredQueries()
        {
            var @params = new ReportParams { From = null, To = null };
            var vm = GetUnAnsweredQueriesReport(@params);
            return View(vm);
        }

        [HttpPost]
        public ActionResult UnAnsweredQueries(ReportParams @params)
        {
            if (!IsValidReportParams(@params))
            {
                return View("_InvalidInputError");
            }

            var vm = GetUnAnsweredQueriesReport(@params);
            return View(vm);
        }

        [HttpGet]
        public ActionResult UnSatisfiedVisitors()
        {
            var @params = new ReportParams { From = null, To = null };
            var vm = GetUnSatisfiedVisitorsReport(@params);
            return View(vm);
        }

        [HttpPost]
        public ActionResult UnSatisfiedVisitors(ReportParams @params)
        {
            if (!IsValidReportParams(@params))
            {
                return View("_InvalidInputError");
            }

            var vm = GetUnSatisfiedVisitorsReport(@params);
            return View(vm);
        }

        [HttpPost]
        public ActionResult LeadGenerationReport(ReportParams @params)
        {
            if (!IsValidReportParams(@params))
            {
                return View("_InvalidInputError");
            }

            var vm = GetLeadGenerationReport(@params);
            return View(vm);
        }

        [HttpPost]
        public IActionResult ExportLeadGenerationReport(ReportParams @params)
        {
            if (!IsValidReportParams(@params))
            {
                return View("_InvalidInputError");
            }

            var fromDate = @params.From;
            var toDate = @params.To;

            if (string.IsNullOrEmpty(@params.From) || string.IsNullOrEmpty(@params.To))
            {
                fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else
            {
                if (IsValidDate(fromDate) && IsValidDate(toDate))
                {
                    fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                    toDate = Convert.ToDateTime(toDate).AddDays(1).ToString("yyyy-MM-dd");
                }
                else
                {
                    fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                    toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                }
            }

            var vm = GenerateLeadGenerationReport(@params);

            var leadGenerationInfo1 = GetLeadGenerationInfos(vm);
            UpdateLeadGenerationInfos(leadGenerationInfo1);

            var leadGenerationInfos = GetLeadGenerationInfos(fromDate, toDate);

            var actions = _dbContext.LeadGenerationActions.ToList();

            var buffer = new LeadGenerationExportBridge().Export(leadGenerationInfos, actions,
                Convert.ToDateTime(@params.From).ToString("dd-MMM-yy"),
                Convert.ToDateTime(@params.To).ToString("dd-MMM-yy"));

            var fileName = @"Lead_Generation_Report-" + @params.From + " to " + @params.To + ".xlsx";

            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost]
        public ActionResult UpdateFeedback([FromBody] ActivityFeedback activityFeedback)
        {
            if (ModelState.IsValid)
            {
                bool isValidResponseFeedback = Enum.IsDefined(typeof(ResonseFeedback), activityFeedback.ResonseFeedback);
                if (!isValidResponseFeedback)
                    return BadRequest("Invalid Input");

                try
                {
                    var activity = _dbContext.ChatLogs.Where(l => l.ReplyToActivityId == activityFeedback.ActivityId.ToString()).FirstOrDefault();
                    if (activity != null)
                    {
                        activity.ResonseFeedback = activityFeedback.ResonseFeedback;
                        _dbContext.ChatLogs.Update(activity);
                        _dbContext.SaveChanges();
                    }
                    else
                        return NotFound($"Activity with the Id {activityFeedback.ActivityId} not found!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                    throw;
                }
                return Ok();
            }
            return BadRequest(ModelState);
        }

        //TODO Check for invalid request
        [HttpPost]
        public ActionResult UpdateLeadGenerationAction(LeadGenerationActionViewModel vm)
        {
            try
            {
                var lead = _dbContext.LeadGenerationInfos.Where(lgi => lgi.Id == vm.Id).FirstOrDefault();
                if (lead != null)
                {
                    lead.LeadGenerationActionId = vm.LeadGenerationActionId;
                    _dbContext.LeadGenerationInfos.Update(lead);
                    _dbContext.SaveChanges();
                }
                else
                    return NotFound($"Lead with the Id {vm.Id} not found!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
            return Ok();
        }

        private bool IsValidReportParams(ReportParams @params)
        {
            if (string.IsNullOrEmpty(@params.From) || string.IsNullOrEmpty(@params.To))
            {
                return false;
            }
            else
            {
                if (IsValidDate(@params.From) && IsValidDate(@params.To))
                {
                    return true;
                }
            }

            return false;
        }

        private UnSatisfiedVisitorsViewModel GetUnSatisfiedVisitorsReport(ReportParams @params)
        {
            var fromDate = @params.From;
            var toDate = @params.To;
            if (string.IsNullOrEmpty(@params.From) || string.IsNullOrEmpty(@params.To))
            {
                fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else
            {
                if (IsValidDate(fromDate) && IsValidDate(toDate))
                {
                    fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                    toDate = Convert.ToDateTime(toDate).AddDays(1).ToString("yyyy-MM-dd");
                }
                else
                {
                    fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                    toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                }
            }

            var query = $"select * from \"ChatLogs\" where \"ReplyToActivityId\" is not null and \"ResonseFeedback\" = -1 and \"IsOnBoardingMessage\" is null and \"TimeStamp\" between '{fromDate}' AND '{toDate}' order by \"TimeStamp\"";

            var chatLogs = _dbContext.ChatLogs.FromSqlRaw(query).ToList();

            var conversationIds = chatLogs.Select(c => c.ConversationId).Distinct().ToList();

            var groupedConversations = chatLogs
                                        .GroupBy(c => c.ReplyToActivityId)
                                        .Select(grp => grp.ToList())
                                        .ToList();

            var userInfo = _dbContext.UserInfos.Where(u => conversationIds.Contains(u.ConversationId)).ToList();

            List<UnAnsweredQueries> unAnsweredQueries = new List<UnAnsweredQueries>();

            foreach (var conversation in groupedConversations)
            {
                var log = conversation.FirstOrDefault();

                var texts = string.Empty;
                var activityId = conversation.FirstOrDefault().ReplyToActivityId;

                var data = _dbContext.ChatLogs.Where(c => c.ActivityId == activityId).FirstOrDefault();

                if (conversation.Count == 1)
                {
                    texts = conversation.FirstOrDefault().Text;
                }
                else if (conversation.Count > 1)
                {
                    texts = string.Join("\n", conversation.Select(c => c.Text).ToList());
                }

                if (data != null)
                {
                    UnAnsweredQueries unAnsweredQuery = new UnAnsweredQueries
                    {
                        Query = data.Text,
                        BotResponse = texts,
                        Name = userInfo.Where(u => u.ConversationId == log.ConversationId).Select(u => u.Name).FirstOrDefault(),
                        PhoneNumber = userInfo.Where(u => u.ConversationId == log.ConversationId).Select(u => u.PhoneNumber).FirstOrDefault(),
                        TimeStamp = userInfo.Where(u => u.ConversationId == log.ConversationId).Select(u => u.CreatedOn).FirstOrDefault(),
                    };
                    unAnsweredQueries.Add(unAnsweredQuery);
                }
            }

            var vm = new UnSatisfiedVisitorsViewModel
            {
                UnAnsweredQueries = unAnsweredQueries,
                From = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy"),
                To = Convert.ToDateTime(toDate).AddDays(-1).ToString("dd-MMM-yyyy")
            };

            return vm;
        }

        private LeadGenerationReportViewModel GenerateLeadGenerationReport(ReportParams @params)
        {
            var fromDate = @params.From;
            var toDate = @params.To;

            if (string.IsNullOrEmpty(@params.From) || string.IsNullOrEmpty(@params.To))
            {
                fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else
            {
                if (IsValidDate(fromDate) && IsValidDate(toDate))
                {
                    fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                    toDate = Convert.ToDateTime(toDate).AddDays(1).ToString("yyyy-MM-dd");
                }
                else
                {
                    fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                    toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                }
            }

            var query = $"select * from \"ChatLogs\" where \"RasaIntent\" not in ('about_us_intent','greet', 'bye_intent', 'lost_intent', 'scrollbar_intent','link_intent','capabilities_intent','services_intent') and \"TimeStamp\" between '{fromDate}' AND '{toDate}' order by \"TimeStamp\"";
            var chatLogs = _dbContext.ChatLogs.FromSqlRaw(query).ToList();
            var overallChatLogs = chatLogs;

            chatLogs = chatLogs.Where(c => c.Text != null && c.Text != "").ToList();

            List<List<ChatLog>> conversationList = chatLogs.GroupBy(item => item.RasaIntent)
                                             .Select(item => item.ToList())
                                             .ToList();

            List<ConversationByIntent> conversationByIntent = new List<ConversationByIntent>();

            var conversationIds = chatLogs.Select(c => c.ConversationId).Distinct().ToList();

            var userInfo = _dbContext.UserInfos.Where(u => conversationIds.Contains(u.ConversationId)).ToList();

            var leadGenerationInfos = _dbContext.LeadGenerationInfos.Where(lgi => conversationIds.Contains(lgi.ConversationId)).ToList();

            conversationList = conversationList.FindAll(a => a.All(b => b.RasaIntent != null && b.RasaIntent != "")).ToList();

            foreach (var list in conversationList)
            {
                var cbi = new ConversationByIntent
                {
                    Intent = list.FirstOrDefault().RasaIntent
                };

                var li =
                list.GroupBy(item => item.ConversationId)
                                             .Select(group => new ConversationByUser
                                             {
                                                 UserInfoId = userInfo.Where(u => u.ConversationId == group.Key).Select(u => u.Id).FirstOrDefault(),
                                                 ConversationId = group.Key,
                                                 Name = userInfo.Where(u => u.ConversationId == group.Key).Select(u => u.Name).FirstOrDefault(),
                                                 PhoneNumber = userInfo.Where(u => u.ConversationId == group.Key).Select(u => u.PhoneNumber).FirstOrDefault(),
                                                 TimeStamp = userInfo.Where(u => u.ConversationId == group.Key).Select(u => u.CreatedOn).FirstOrDefault(),
                                                 ChatLogs = group.ToList(),
                                                 LeadGenerationInfo = leadGenerationInfos.FirstOrDefault(lgi => lgi.ConversationId == group.Key && lgi.DomainName == cbi.Intent)
                                             }).
                                             OrderByDescending(o => o.LeadGenerationInfo?.LeadGenerationActionId == null).ToList();

                cbi.ConversationByUsers = li;
                conversationByIntent.Add(cbi);
            }

            foreach (var conv in conversationByIntent)
            {
                foreach (var userCov in conv.ConversationByUsers)
                {
                    var turnConversations = new List<TurnConversation>();
                    var botLogs = userCov.ChatLogs.Where(l => l.ReplyToActivityId != null).ToList();
                    var ReplyToActivityIds = botLogs.Select(r => r.ReplyToActivityId).Distinct().ToList();

                    foreach (var replyId in ReplyToActivityIds)
                    {
                        var botResponses = overallChatLogs.Where(c => c.ReplyToActivityId == replyId).ToList();
                        var botResponseText = new StringBuilder();
                        var botTimeStamp = botResponses.FirstOrDefault().TimeStamp?.ToString("ddd, dd MMM yyy HH:mm:ss");


                        for (int i = 0; i <= botResponses.Count - 1; i++)
                        {
                            var text = (string.IsNullOrEmpty(botResponses[i].Text)) ? botResponses[i].MainTitle : botResponses[i].Text;

                            botResponseText.Append(text);
                            if (i != botResponses.Count - 1)
                            {
                                botResponseText.Append(Environment.NewLine);
                            }
                        }

                        var userResponse = _dbContext.ChatLogs.Where(c => c.ActivityId == replyId).FirstOrDefault();
                        var userTimeStamp = userResponse.TimeStamp?.ToString("ddd, dd MMM yyy HH:mm:ss");
                        var turnConversation = new TurnConversation
                        {
                            ActivityId = replyId,
                            BotResponse = botResponseText.ToString(),
                            UserQuery = userResponse == null ? string.Empty : userResponse.Text,
                            BotTimeStamp = botTimeStamp,
                            UserTimeStamp = userTimeStamp
                        };
                        turnConversations.Add(turnConversation);
                    }
                    userCov.TurnConversations = turnConversations;
                }
            }

            conversationByIntent = conversationByIntent.OrderByDescending(conv => conv.ConversationByUsers.Sum(c => c.TurnConversations.Count)).ToList();
            var vm = new LeadGenerationReportViewModel
            {
                ConversationsByIntent = conversationByIntent,
                From = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy"),
                To = Convert.ToDateTime(toDate).AddDays(-1).ToString("dd-MMM-yyyy")
            };

            return vm;
        }

        public ActionResult LeadGenerationReport()
        {
            var @params = new ReportParams { From = null, To = null };
            var vm = GetLeadGenerationReport(@params);
            return View(vm);
        }

        private LeadGenerationReportViewModel GetLeadGenerationReport(ReportParams @params)
        {
            var leadGenerationActions = _dbContext.LeadGenerationActions.ToList();
            var vm = GenerateLeadGenerationReport(@params);
            var leadGenerationInfos = GetLeadGenerationInfos(vm);

            UpdateLeadGenerationInfos(leadGenerationInfos);
            vm = GenerateLeadGenerationReport(@params);
            vm.LeadGenerationActions = leadGenerationActions;
            return vm;
        }

        private List<LeadGenerationInfo> GetLeadGenerationInfos(string fromDate, string toDate)
        {
            var query = $"select * from \"LeadGenerationInfos\" where \"QueriedOn\" between '{fromDate}' AND '{toDate}' and \"DomainName\" not in  ('about_us_intent','greet', 'bye_intent', 'lost_intent', 'scrollbar_intent','link_intent','capabilities_intent','services_intent')";

            var leadGenerationInfos = _dbContext.LeadGenerationInfos.FromSqlRaw(query).ToList();
            return leadGenerationInfos;
        }

        private void UpdateLeadGenerationInfos(List<LeadGenerationInfo> leadInfos)
        {
            var conversationIds = leadInfos.Select(l => l.ConversationId).ToList();

            var existingConversations = _dbContext.LeadGenerationInfos.Where(l => conversationIds.Contains(l.ConversationId)).Select(l => l.ConversationId).ToList();

            var uniqueConversations = conversationIds.Except(existingConversations).ToList();

            var actualLeadInfos = leadInfos.Where(li => uniqueConversations.Contains(li.ConversationId)).ToList();

            _dbContext.LeadGenerationInfos.AddRange(actualLeadInfos);
            _dbContext.SaveChanges();
        }

        private List<LeadGenerationInfo> GetLeadGenerationInfos(LeadGenerationReportViewModel viewModel)
        {
            var vm = viewModel;

            var leadGenerationInfos = new List<LeadGenerationInfo>();

            foreach (var leadGenerationReport in vm.ConversationsByIntent)
            {
                foreach (var userConversation in leadGenerationReport.ConversationByUsers)
                {
                    var leadInfo = new LeadGenerationInfo
                    {
                        Id = 0,
                        DomainName = leadGenerationReport.Intent,
                        LeadGenerationActionId = null,
                        PhoneNumber = userConversation.PhoneNumber,
                        QueriedOn = userConversation.TimeStamp,
                        UserInfoId = userConversation.UserInfoId,
                        ConversationId = userConversation.ConversationId,
                        Visitor = userConversation.Name
                    };
                    leadGenerationInfos.Add(leadInfo);
                }
            }
            return leadGenerationInfos;
        }

        private void CleanChatLog()
        {
            var conversationIds = _dbContext.ChatLogs.Where(c => c.IsOnBoardingMessage == null).Select(c => c.ConversationId).Distinct().ToList();

            foreach (var conversationId in conversationIds)
            {
                var userInfo = _dbContext.UserInfos.Where(u => u.ConversationId == conversationId).FirstOrDefault();
                if (userInfo != null)
                {
                    var phoneNumber = userInfo.PhoneNumber.ToLower();
                    var chatInfo = _dbContext.ChatLogs.Where(c => c.ConversationId == conversationId && c.Text.ToLower() == phoneNumber).FirstOrDefault();
                    if (chatInfo != null)
                    {
                        chatInfo.IsOnBoardingMessage = true;
                        _dbContext.ChatLogs.Update(chatInfo);
                        _dbContext.SaveChanges();
                    }
                }
            }
        }

        private bool IsValidDate(string dateString)
        {
            DateTime dateValue;
            var isValid = (DateTime.TryParse(dateString, out dateValue));
            return isValid;
        }

        private FrequentlyAskedQueriesViewModel GetFrequentlyAskedQueriesReport(ReportParams @params)
        {
            var fromDate = @params.From;
            var toDate = @params.To;
            if (string.IsNullOrEmpty(@params.From) || string.IsNullOrEmpty(@params.To))
            {
                fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else
            {
                if (IsValidDate(fromDate) && IsValidDate(toDate))
                {
                    fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                    toDate = Convert.ToDateTime(toDate).AddDays(1).ToString("yyyy-MM-dd");
                }
                else
                {
                    fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                    toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                }
            }

            CleanChatLog();
            //var query = $"select* from \"ChatLogs\" where \"ReplyToActivityId\" is null and \"IsOnBoardingMessage\" is null and coalesce(\"Text\", '') != '' and \"RasaIntent\" not in ('about_us_intent','greet', 'bye_intent') order by \"TimeStamp\" asc";
            var query = $"select* from \"ChatLogs\" where \"ReplyToActivityId\" is null and \"IsOnBoardingMessage\" is null and coalesce(\"Text\", '') != '' and \"RasaIntent\" not in ('about_us_intent','greet', 'bye_intent') and \"TimeStamp\" between '{fromDate}' AND '{toDate}' order by \"TimeStamp\" asc";

            var chatLogs = _dbContext.ChatLogs.FromSqlRaw(query).ToList();
            chatLogs = chatLogs.Where(c => c.Text != null && c.Text != "").ToList();
            var frequentlyAskedQueries = chatLogs.GroupBy(item => item.Text)
                                             .Select(c => new FrequentlyAskedQueries
                                             {
                                                 ActivityIds = c.Select(j => j.ActivityId).ToList(),
                                                 Count = c.ToList().Count(),
                                                 Query = c.Key
                                             }).OrderByDescending(c => c.Count)
                                             .AsQueryable();

            var frequentlyAskedQueryList = frequentlyAskedQueries.Where(faq => faq.Count > 1).ToList();

            foreach (var queryItem in frequentlyAskedQueryList)
            {
                var records = _dbContext.ChatLogs.Where(c => c.ResonseFeedback.HasValue && queryItem.ActivityIds.Contains(c.ReplyToActivityId)).ToList();
                queryItem.NegetiveFeedback = records.Where(c => c.ResonseFeedback == ResonseFeedback.ThumbsDown).Skip(1).Count();
                queryItem.PositiveFeedback = records.Where(c => c.ResonseFeedback == ResonseFeedback.ThumbsUp).Skip(1).Count();
            }

            var vm = new FrequentlyAskedQueriesViewModel
            {
                FrequentlyAskedQueries = frequentlyAskedQueryList,
                From = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy"),
                To = Convert.ToDateTime(toDate).AddDays(-1).ToString("dd-MMM-yyyy")
            };

            return vm;
        }

        private ChatBotVisitorsViewModel GetChatBotVisitorsViewModelReport(ReportParams @params)
        {
            var fromDate = @params.From;
            var toDate = @params.To;
            if (string.IsNullOrEmpty(@params.From) || string.IsNullOrEmpty(@params.To))
            {
                fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else
            {
                if (IsValidDate(fromDate) && IsValidDate(toDate))
                {
                    fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                    toDate = Convert.ToDateTime(toDate).AddDays(1).ToString("yyyy-MM-dd");
                }
                else
                {
                    fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                    toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                }
            }

            var query = $"select Visitor, PhoneNumber, Sum(NumberOfQueriesAsked) as NumberOfQueries , Sum(NumberOfVisits) as NumberOfVisits, Max(LastVisit) as LastVisited from (select Visitor, PhoneNumber, LastVisit, Sum(CountOfConversation) as NumberOfQueriesAsked, Count(ConversationId) as NumberOfVisits from (select U.\"Name\" as Visitor, U.\"PhoneNumber\" as PhoneNumber, Count(U.\"Id\") as CountOfConversation, C.\"ConversationId\" as ConversationId, Max(C.\"TimeStamp\") as LastVisit from \"ChatLogs\" C inner join \"UserInfos\" U on C.\"ConversationId\" = U.\"ConversationId\" where C.\"IsOnBoardingMessage\" is null and C.\"ReplyToActivityId\" is null and coalesce(C.\"Text\", '') != '' and \"TimeStamp\" between '{fromDate}' AND '{toDate}' group by U.\"Name\", U.\"PhoneNumber\", C.\"ConversationId\") as tmp group by tmp.Visitor, tmp.PhoneNumber, tmp.LastVisit) as queryResult group by queryResult.Visitor, queryResult.PhoneNumber";

            var users = _dbContext.ChatBotVisitorDetails.FromSqlRaw(query).ToList().OrderByDescending(u => u.NumberOfVisits).ThenByDescending(u => u.NumberOfQueries).ToList();

            var vm = new ChatBotVisitorsViewModel
            {
                ChatBotVisitorDetails = users,
                From = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy"),
                To = Convert.ToDateTime(toDate).AddDays(-1).ToString("dd-MMM-yyyy"),
                TotalQueries = users.Sum(u => u.NumberOfQueries),
                TotalVisits = users.Sum(u => u.NumberOfVisits)
            };

            return vm;
        }

        private UnAnsweredQueriesViewModel GetUnAnsweredQueriesReport(ReportParams @params)
        {
            var fromDate = @params.From;
            var toDate = @params.To;
            if (string.IsNullOrEmpty(@params.From) || string.IsNullOrEmpty(@params.To))
            {
                fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else
            {
                if (IsValidDate(fromDate) && IsValidDate(toDate))
                {
                    fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                    toDate = Convert.ToDateTime(toDate).AddDays(1).ToString("yyyy-MM-dd");
                }
                else
                {
                    fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                    toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                }
            }

            var query = $"select * from \"ChatLogs\" where ( \"ActivityId\" is not null and \"ReplyToActivityId\" is null and coalesce(\"RasaIntent\", '') != '' and \"RasaIntent\" in ('bye_intent')) or (\"Text\" = 'Sorry,I could not understand. Could you please rephrase the query.' ) and \"TimeStamp\" between '{fromDate}' AND '{toDate}' order by \"TimeStamp\"";

            var chatLogs = _dbContext.ChatLogs.FromSqlRaw(query).ToList();

            var conversationIds = chatLogs.Select(c => c.ConversationId).Distinct().ToList();

            var userInfo = _dbContext.UserInfos.Where(u => conversationIds.Contains(u.ConversationId)).ToList();

            List<UnAnsweredQueries> unAnsweredQueries = new List<UnAnsweredQueries>();
            foreach (var log in chatLogs)
            {
                var data = _dbContext.ChatLogs.Where(c => c.ReplyToActivityId == log.ActivityId).FirstOrDefault();
                UnAnsweredQueries unAnsweredQuery = new UnAnsweredQueries
                {
                    Query = log.Text,
                    BotResponse = (data == null) ? string.Empty : data.Text,
                    Name = userInfo.Where(u => u.ConversationId == log.ConversationId).Select(u => u.Name).FirstOrDefault(),
                    PhoneNumber = userInfo.Where(u => u.ConversationId == log.ConversationId).Select(u => u.PhoneNumber).FirstOrDefault(),
                    TimeStamp = userInfo.Where(u => u.ConversationId == log.ConversationId).Select(u => u.CreatedOn).FirstOrDefault(),
                };
                unAnsweredQueries.Add(unAnsweredQuery);
            }

            var vm = new UnAnsweredQueriesViewModel
            {
                UnAnsweredQueries = unAnsweredQueries,
                From = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy"),
                To = Convert.ToDateTime(toDate).AddDays(-1).ToString("dd-MMM-yyyy")
            };

            return vm;
        }
    }
}
