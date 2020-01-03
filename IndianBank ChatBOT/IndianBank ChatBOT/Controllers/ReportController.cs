using IndianBank_ChatBOT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IndianBank_ChatBOT.Controllers
{
    //[Route("[controller]")]
    public class ReportController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _dbContext;
        private readonly string _connectionString;

        public ReportController(AppDbContext _dbContext, IConfiguration configuration, IOptions<AppSettings> appsettings)
        {
            this._dbContext = _dbContext;
            _appSettings = appsettings.Value;
            _connectionString = configuration.GetConnectionString("connString");
        }

        public ActionResult Index()
        {
            return View();
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

        public ActionResult FrequentlyAskedQueries()
        {
            CleanChatLog();
            
            //var query = $"select * from \"ChatLogs\" where \"FromId\"='IndianBank_ChatBOT' and coalesce(\"RasaIntent\", '') != '' and \"RasaIntent\" not in ('about_us_intent','greet')";
            var query = $"select* from \"ChatLogs\" where \"ReplyToActivityId\" is null and \"IsOnBoardingMessage\" is null and coalesce(\"Text\", '') != '' and \"RasaIntent\" not in ('about_us_intent','greet', 'bye_intent') order by \"TimeStamp\" asc";

            var chatLogs = _dbContext.ChatLogs.FromSql(query).ToList();
            chatLogs = chatLogs.Where(c => c.Text != null && c.Text != "").ToList();
            var frequentlyAskedQueries = chatLogs.GroupBy(item => item.Text)
                                             .Select(c => new FrequentlyAskedQueries
                                             {
                                                 Count = c.ToList().Count(),
                                                 NegetiveFeedback = c.ToList().Where(j => j.ResonseFeedback == ResonseFeedback.ThumbsDown).Count(),
                                                 PositiveFeedback = c.ToList().Where(j => j.ResonseFeedback == ResonseFeedback.ThumbsUp).Count(),
                                                 Query = c.Key
                                             }).OrderByDescending(c => c.Count)
                                             .ToList();

            return View(frequentlyAskedQueries);
        }

        public ActionResult AppUsers()
        {
            var users = _dbContext.UserInfos.ToList();
            return View(users);
        }

        public ActionResult UnAnsweredQueries()
        {
            //var query = $"select \"Text\" as Query  from \"ChatLogs\" where \"FromId\"='IndianBank_ChatBOT' and coalesce(\"RasaIntent\", '') != '' and \"RasaIntent\" in ('bye_intent')";
            //var query = $"select * from \"ChatLogs\" where \"FromId\"='IndianBank_ChatBOT' and coalesce(\"RasaIntent\", '') != '' and \"RasaIntent\" in ('bye_intent') order by \"TimeStamp\"";
            //var query = $"select * from \"ChatLogs\" where (\"ReplyToActivityId\" is null and coalesce(\"RasaIntent\", '') != '') or (\"Text\" = 'Sorry,I could not understand. Could you please rephrase the query.' ) order by \"TimeStamp\"";
            var query = $"select * from \"ChatLogs\" where ( \"ActivityId\" is not null and \"ReplyToActivityId\" is null and coalesce(\"RasaIntent\", '') != '' and \"RasaIntent\" in ('bye_intent')) or (\"Text\" = 'Sorry,I could not understand. Could you please rephrase the query.' ) order by \"TimeStamp\"";

            var chatLogs = _dbContext.ChatLogs.FromSql(query).ToList();

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

            return View(unAnsweredQueries);
        }

        public ActionResult UnSatisfiedVisitors()
        {
            var query = $"select * from \"ChatLogs\" where \"ReplyToActivityId\" is not null and \"ResonseFeedback\" = '-1'";

            var chatLogs = _dbContext.ChatLogs.FromSql(query).ToList();

            var conversationIds = chatLogs.Select(c => c.ConversationId).Distinct().ToList();

            var userInfo = _dbContext.UserInfos.Where(u => conversationIds.Contains(u.ConversationId)).ToList();

            List<UnAnsweredQueries> unAnsweredQueries = new List<UnAnsweredQueries>();
            foreach (var log in chatLogs)
            {
                var data = _dbContext.ChatLogs.Where(c => c.ActivityId == log.ReplyToActivityId).FirstOrDefault();
                UnAnsweredQueries unAnsweredQuery = new UnAnsweredQueries
                {
                    Query = (data == null) ? string.Empty : data.Text,
                    BotResponse = log.Text,
                    Name = userInfo.Where(u => u.ConversationId == log.ConversationId).Select(u => u.Name).FirstOrDefault(),
                    PhoneNumber = userInfo.Where(u => u.ConversationId == log.ConversationId).Select(u => u.PhoneNumber).FirstOrDefault(),
                    TimeStamp = userInfo.Where(u => u.ConversationId == log.ConversationId).Select(u => u.CreatedOn).FirstOrDefault(),
                };
                unAnsweredQueries.Add(unAnsweredQuery);
            }
            return View(unAnsweredQueries);
        }

        public ActionResult LeadGenerationReport()
        {
            var query = $"select * from \"ChatLogs\" where \"RasaIntent\" not in ('about_us_intent','greet') order by \"TimeStamp\"";
            var chatLogs = _dbContext.ChatLogs.FromSql(query).ToList();
            var overallChatLogs = chatLogs;

            chatLogs = chatLogs.Where(c => c.Text != null && c.Text != "").ToList();

            List<List<ChatLog>> conversationList = chatLogs.GroupBy(item => item.RasaIntent)
                                             .Select(item => item.ToList())
                                             .ToList();

            List<ConversationByIntent> conversationByIntent = new List<ConversationByIntent>();

            var conversationIds = chatLogs.Select(c => c.ConversationId).Distinct().ToList();

            var userInfo = _dbContext.UserInfos.Where(u => conversationIds.Contains(u.ConversationId)).ToList();

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
                                                 ConversationId = group.Key,
                                                 Name = userInfo.Where(u => u.ConversationId == group.Key).Select(u => u.Name).FirstOrDefault(),
                                                 PhoneNumber = userInfo.Where(u => u.ConversationId == group.Key).Select(u => u.PhoneNumber).FirstOrDefault(),
                                                 TimeStamp = userInfo.Where(u => u.ConversationId == group.Key).Select(u => u.CreatedOn).FirstOrDefault(),
                                                 ChatLogs = group.ToList()
                                             })
                                             .ToList();
                cbi.ConversationByUsers = li;
                conversationByIntent.Add(cbi);
            }

            foreach (var conv in conversationByIntent)
            {
                foreach (var userCov in conv.ConversationByUsers)
                {
                    var turnConversations = new List<TurnConversation>();
                    var botLogs = userCov.ChatLogs.Where(l => l.ReplyToActivityId != null).ToList();
                    foreach (var log in botLogs)
                    {
                        var botResponse = overallChatLogs.Where(c => c.ReplyToActivityId == log.ReplyToActivityId).FirstOrDefault();
                        var userResponse = overallChatLogs.Where(c => c.ActivityId == log.ReplyToActivityId).FirstOrDefault();
                        var turnConversation = new TurnConversation
                        {
                            ActivityId = log.ReplyToActivityId,
                            BotResponse = botResponse == null ? string.Empty : (string.IsNullOrEmpty(botResponse.Text) ? botResponse.MainTitle : botResponse.Text),
                            UserQuery = userResponse == null ? string.Empty : userResponse.Text,
                        };
                        turnConversations.Add(turnConversation);
                    }
                    userCov.TurnConversations = turnConversations;
                }
            }

            return View(conversationByIntent);
        }

        [HttpPost]
        public ActionResult UpdateFeedback([FromBody] ActivityFeedback activityFeedback)
        {
            try
            {
                var activity = _dbContext.ChatLogs.Where(l => l.ReplyToActivityId == activityFeedback.ActivityId).FirstOrDefault();
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
    }
}
