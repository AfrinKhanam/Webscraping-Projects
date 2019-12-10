using IndianBank_ChatBOT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Controllers
{
   
    [Route("[controller]")]
    public class FaqController : Controller
    {
        private readonly AppSettings appSettings;
        private readonly LogDataContext data;
       
        public FaqController(LogDataContext _dbContext, IOptions<AppSettings> appsettings)
        {
            data = _dbContext;
            appSettings = appsettings.Value;
           
        }
        public ActionResult Index()
        {
            return RedirectToAction("Display");
        }
        
        [Route("Display")]
        [HttpGet]
        public ActionResult Display()
        {
            return View(data.Faq.ToList());
        }

        [Route("Delete")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                Faq faq = data.Faq.Where(a => a.Id == id).FirstOrDefault();
                data.Faq.Remove(faq);
                data.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }

        [Route("Update")]
        [HttpGet]
        public ActionResult Update(int id)
        {
            var faq = data.Faq.Find(id);
            return View(faq);

        }

        [HttpPost]
        [Route("Update")]
        public IActionResult Update(Faq faq)
        {
            data.Faq.Update(faq);
            data.SaveChanges();
            return RedirectToAction("display");
        }

        [Route("Create")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [ HttpPost]
        public ActionResult Create(Faq faq)
        {
            faq.GuidID = Guid.NewGuid();
            data.Faq.Add(faq);
            data.SaveChanges();

            ViewBag.InsertFAQStatus = "New FAQ is added successfully.";
            ModelState.Clear();

            return RedirectToAction("display");
        }
        

        [HttpGet("TrainFaq")]
        public IActionResult TrainFaq()
        {
            
            object result = null;
            DataTable dt = new DataTable();

            try
            {
                dt.Columns.Add("Question", typeof(string));
                dt.Columns.Add("Answer", typeof(string));
                foreach (Faq faq in data.Faq)
                {
                    if (faq.Answer.Contains(System.Environment.NewLine))
                    {
                        string ans = faq.Answer.Replace(System.Environment.NewLine, "<br>");
                        dt.Rows.Add(faq.Question, ans);
                    }
                    else
                    {
                        dt.Rows.Add(faq.Question, faq.Answer);
                    }
                }

                IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                      Select(column => column.ColumnName);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Join(",", columnNames));
                foreach (DataRow row in dt.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                    ToArray();
                    sb.AppendLine(string.Join(",", fields));
                }

                string deepPavlovTrainigPath = appSettings.DeepPavlovPath;

                if (!Directory.Exists(deepPavlovTrainigPath))
                {
                    var obj = new { Value = new { Success = false, Message = $"Folder path not found {deepPavlovTrainigPath}", ExceptionType = $"deepPavlovTrainigPath" } };

                    return Ok(obj);
                }


                if (!Directory.Exists(deepPavlovTrainigPath + Path.DirectorySeparatorChar + "TrainedModel"))
                {
                    var obj = new { Value = new { Success = false, Message = $"Folder path not found {deepPavlovTrainigPath + Path.DirectorySeparatorChar + "TrainedModel"}" } };
                    return Ok(obj);
                }

                var writePath = deepPavlovTrainigPath + Path.DirectorySeparatorChar + "TrainedModel" + Path.DirectorySeparatorChar + "TrainingData.csv";

                System.IO.File.WriteAllText(writePath, sb.ToString());


                if (System.IO.File.Exists(writePath))
                {
                    //train
                    result = GetFAQResult();
                    return Ok(result);
                }
                else
                {
                    var obj = new { Value = new { Success = false, Message = $"File path not found {writePath}" } };
                    return Ok(obj);
                }
            }
            catch (Exception ex)
            {
               
                var obj = new { Value = new { Success = false, Message = ex.Message } };
                return Ok(obj);
            }
        }
        private object GetFAQResult()
        {
            object result = null;
            var uri = appSettings.TrainingEndPoint;
            try
            {
                var client = new RestClient(uri);

                var request = new RestRequest()
                {
                    RequestFormat = DataFormat.Json
                };

                var response = client.Execute(request);

                

                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    result = new { Success = true };
                }
                else
                {
                    result = new { Success = false };
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                result = new { Success = false, Message = ex };
                return Ok(result);
            }
        }

       
        [HttpGet("TestFaq")]
        public IActionResult TestFaq(string testdata)
        {
            object result = null;
            try
            {
                string deepPavlovPath = appSettings.DeepPavlovPath;
               
                string modalPath = deepPavlovPath + Path.DirectorySeparatorChar + "TrainedModel";
              

                string trainedModelPath = deepPavlovPath + Path.DirectorySeparatorChar + "TrainedModel" + Path.DirectorySeparatorChar + "model";
               

                if (!Directory.Exists(modalPath))
                {
                    var obj = new { Success = false, Message = $"Folder path not found {modalPath}" };
                    return Ok(obj);
                }

                if (!Directory.Exists(trainedModelPath))
                {
                    var obj = new { Success = false, Message = $"Folder path not found {trainedModelPath}" };
                    return Ok(obj);
                }

                string question = testdata;
                object FAQData = null;

                var uri = appSettings.TestingEndPoint + question;

                var client = new RestClient(uri);

                var request = new RestRequest()
                {
                    RequestFormat = DataFormat.Json
                };

                var response = client.Execute(request);

                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                   

                    FAQData = JsonConvert.DeserializeObject(response.Content);
                    result = new { Success = true, Data = FAQData };
                }
                else
                {
                   
                    result = new { Success = false, Data = "" };
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                
                result = new { Success = false, Message = ex.Message };
                return Ok(result);
            }
        }
        [HttpGet]
        [Route("DeployFaq")]
        public IActionResult DeployFaq()
        {

            try
            {
                //Copy all the files & Replaces any files with the same name
                string deepPavlovPath = appSettings.DeepPavlovPath;
                string trainedModelPath = deepPavlovPath + Path.DirectorySeparatorChar + "TrainedModel";

                if (!Directory.Exists(trainedModelPath))
                {
                    var obj = new { Success = false, Message = $"Folder path not found {trainedModelPath}" };
                    return Ok(obj);
                }

                string sourcePath = trainedModelPath;
                string destinationPath = deepPavlovPath + Path.DirectorySeparatorChar + "DeployedModel";

                //Now Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(sourcePath, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

                foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",
                    SearchOption.AllDirectories))
                    System.IO.File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);

                var result = new { Success = true, Message = "Successfully Deployed" };



                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };

                return Ok(result);
            }
        }


    }



}