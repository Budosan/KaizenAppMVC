using KaizenAppMVC.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using static KaizenAppMVC.Models.ProcessorModel;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using RouteAttribute = System.Web.Http.RouteAttribute;
using System.Web.Helpers;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;
using AllowAnonymousAttribute = System.Web.Mvc.AllowAnonymousAttribute;

namespace KaizenAppMVC.Controllers
{
    
    [AllowAnonymous]

    public class HomeController : Controller
    {
        //Inex page, a copy of Home/Kaizen
        public async Task<ActionResult> Index(KaizenModel model, LocalModel m, int? id)
        {

            if (ModelState.IsValid)

            {
                //Passing values from model to processor
                int recordsCreated = CreateKaizenAsync(model.KaizenSuggestion, model.KaizenSolution, model.Department, model.Status = "Submitted", model.WorkerID, model.WorkerName, model.WorkerLastname, m.UseName);
                //Manager Eamil
                try
                {

                    LocalModel managerEmail = new LocalModel();
                    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["KaizenDB"].ConnectionString))
                    {

                        managerEmail = db.Query<LocalModel>(
                             "SELECT [ManagerEmail] FROM [Kaizen].[dbo].[Department] where DepartmentName = '" + model.Department.ToString() + "'").SingleOrDefault();
                    }

                    //From Address    
                    string FromAddress = "GMBX-NVS12@lear.com";
                    string FromAdressTitle = "Kaizen";
                    //To Address    
                    string ToAddress = managerEmail.ManagerEmail;
                    string ToAdressTitle = "ACTION REQUIRED:  Approval for Kaizen Idea ";
                    string Subject = "ACTION REQUIRED:  Approval for Kaizen Idea";
                    string BodyContent = ("Kaizen Suggestion Submitted: " + model.KaizenSuggestion + "      by: " + m.UseName +"        Status:"+model.Status+ "       Visit  http://rsnvs-ds01/Manager/Approvallist for further details.");

                    //Smtp Server    
                    string SmtpServer = "eu.smtp.lear.com";
                    //Smtp Port Number    
                    int SmtpPortNumber = 25;

                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress
                                            (FromAdressTitle,
                                             FromAddress
                                             ));
                    mimeMessage.To.Add(new MailboxAddress
                                             (ToAdressTitle,
                                             ToAddress
                                             ));
                    mimeMessage.Subject = Subject; //Subject  
                    mimeMessage.Body = new TextPart("plain")
                    {
                        Text = BodyContent
                    };


                    using (var client = new SmtpClient())
                    {
                        client.Connect(SmtpServer, SmtpPortNumber, false);
                        client.LocalDomain = "corp.lear.com";

                        await Task.Run(() => client.Send(mimeMessage));
                        await Task.Run(() => client.Disconnect(true));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //User Email
                try
                {

                    LocalModel managerEmail = new LocalModel();
                    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["KaizenDB"].ConnectionString))
                    {

                        managerEmail = db.Query<LocalModel>(
                             "SELECT [ManagerEmail] FROM [Kaizen].[dbo].[Department] where DepartmentName = '" + model.Department.ToString() + "'").SingleOrDefault();
                    }

                    //From Address    
                    string FromAddress = "GMBX-NVS12@lear.com";
                    string FromAdressTitle = "Kaizen";
                    //To Address    
                    string ToAddress = m.UseName + "@lear.com";
                    string ToAdressTitle = "ACTION REQUIRED:  Approval for Kaizen Idea ";
                    string Subject = "KAIZEN Predlog uspešno podnet";
                    string BodyContent = ("Kaizen Suggestion Submitted: " + model.KaizenSuggestion + "      by: " + m.UseName +"        Status:"+model.Status+ "       Visit  http://rsnvs-ds01/Home/MyKaizen for further details.");

                    //Smtp Server    
                    string SmtpServer = "eu.smtp.lear.com";
                    //Smtp Port Number    
                    int SmtpPortNumber = 25;

                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress
                                            (FromAdressTitle,
                                             FromAddress
                                             ));
                    mimeMessage.To.Add(new MailboxAddress
                                             (ToAdressTitle,
                                             ToAddress
                                             ));
                    mimeMessage.Subject = Subject; //Subject  
                    mimeMessage.Body = new TextPart("plain")
                    {
                        Text = BodyContent
                    };


                    using (var client = new SmtpClient())
                    {
                        client.Connect(SmtpServer, SmtpPortNumber, false);
                        client.LocalDomain = "corp.lear.com";

                        await Task.Run(() => client.Send(mimeMessage));
                        await Task.Run(() => client.Disconnect(true));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ModelState.Clear();

                return View("Kaizen");
            }




            else
            {
                return View();
            }
        }
            public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakt tim";

            return View();
        }
        
        [AllowAnonymous]
        [Route("")]
        [Route("Home/Index")]
        [Route("Home/Kaizen")]
        public async Task<ActionResult> Kaizen(KaizenModel model, LocalModel m, int? id)
        {

            if (ModelState.IsValid)

            {
                //Passing values from model to processor
                int recordsCreated = CreateKaizenAsync(model.KaizenSuggestion, model.KaizenSolution, model.Department, model.Status, model.WorkerID, model.WorkerName, model.WorkerLastname, model.UserName);
                //Manager Eamil
                try
                {

                    LocalModel managerEmail = new LocalModel();
                    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["KaizenDB"].ConnectionString))
                    {

                        managerEmail = db.Query<LocalModel>(
                             "SELECT [ManagerEmail] FROM [Kaizen].[dbo].[Department] where DepartmentName = '" + model.Department.ToString() + "'").SingleOrDefault();
                    }

                    //From Address    
                    string FromAddress = "GMBX-NVS12@lear.com";
                    string FromAdressTitle = "Kaizen";
                    //To Address    
                    string ToAddress = managerEmail.ManagerEmail;
                    string ToAdressTitle = "ACTION REQUIRED:  Approval for Kaizen Idea ";
                    string Subject = "ACTION REQUIRED:  Approval for Kaizen Idea";
                    string BodyContent = ("Kaizen Suggestion Submitted: " + model.KaizenSuggestion + "      by: " + m.UseName + "       Visit  http://rsnvs-ds01/Manager/Approvallist for further details.");

                    //Smtp Server    
                    string SmtpServer = "eu.smtp.lear.com";
                    //Smtp Port Number    
                    int SmtpPortNumber = 25;

                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress
                                            (FromAdressTitle,
                                             FromAddress
                                             ));
                    mimeMessage.To.Add(new MailboxAddress
                                             (ToAdressTitle,
                                             ToAddress
                                             ));
                    mimeMessage.Subject = Subject; //Subject  
                    mimeMessage.Body = new TextPart("plain")
                    {
                        Text = BodyContent
                    };


                    using (var client = new SmtpClient())
                    {
                        client.Connect(SmtpServer, SmtpPortNumber, false);
                        client.LocalDomain = "corp.lear.com";

                        await Task.Run(() => client.Send(mimeMessage));
                        await Task.Run(() => client.Disconnect(true));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //User Email
                try
                {

                    LocalModel managerEmail = new LocalModel();
                    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["KaizenDB"].ConnectionString))
                    {

                        managerEmail = db.Query<LocalModel>(
                             "SELECT [ManagerEmail] FROM [Kaizen].[dbo].[Department] where DepartmentName = '" + model.Department.ToString() + "'").SingleOrDefault();
                    }

                    //From Address    
                    string FromAddress = "GMBX-NVS12@lear.com";
                    string FromAdressTitle = "Kaizen";
                    //To Address    
                    string ToAddress = m.UseName + "@lear.com";
                    string ToAdressTitle = "ACTION REQUIRED:  Approval for Kaizen Idea ";
                    string Subject = "KAIZEN Predlog uspešno podnet";
                    string BodyContent = ("Kaizen Suggestion Submitted: " + model.KaizenSuggestion + "      by: " + m.UseName + "       Visit  http://rsnvs-ds01/Home/MyKaizen for further details.");

                    //Smtp Server    
                    string SmtpServer = "eu.smtp.lear.com";
                    //Smtp Port Number    
                    int SmtpPortNumber = 25;

                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress
                                            (FromAdressTitle,
                                             FromAddress
                                             ));
                    mimeMessage.To.Add(new MailboxAddress
                                             (ToAdressTitle,
                                             ToAddress
                                             ));
                    mimeMessage.Subject = Subject; //Subject  
                    mimeMessage.Body = new TextPart("plain")
                    {
                        Text = BodyContent
                    };


                    using (var client = new SmtpClient())
                    {
                        client.Connect(SmtpServer, SmtpPortNumber, false);
                        client.LocalDomain = "corp.lear.com";

                        await Task.Run(() => client.Send(mimeMessage));
                        await Task.Run(() => client.Disconnect(true));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ModelState.Clear();
                
                return View("Kaizen");
            }




            else
            {
                return View();
            }




        }
        
        
        public ActionResult KaizenPreview(int? page)
        {
            ViewBag.Message = "Preview";


            var data = LoadKaizenData();

            List<KaizenPregled> kaizenPregleds = new List<KaizenPregled>();

            foreach (var row in data)
            {
                kaizenPregleds.Add(new KaizenPregled
                {

                    IdeaID = row.IdeaID,
                    DateTime = row.DateTime,
                    DateOfApproval = row.DateOfApproval,
                    DepartmentName = row.DepartmentName,
                    KaizenSuggestion = row.KaizenSuggestion,
                    KaizenSolution = row.KaizenSolution,
                    Status = row.Status,
                    UserName = row.UserName

                });

            }

            return View(kaizenPregleds.ToPagedList(page ?? 1, 100));
        }

        
        
        public ActionResult MyKaizen(int? page)
        {
            ViewBag.Message = "MY Kaizen Suggestion";

            var data = LoadKaizenUserData();

            List<KaizenUserEditModel> kaizenUserEdits = new List<KaizenUserEditModel>();

            foreach (var row in data)
            {
                kaizenUserEdits.Add(new KaizenUserEditModel
                {
                    IdeaID = row.IdeaID,
                    DepartmentName = row.DepartmentName,
                    DateTime = row.DateTime,
                    KaizenSolution = row.KaizenSolution,
                    KaizenSuggestion = row.KaizenSuggestion,
                    Status = row.Status,
                    WorkerID = row.WorkerID,
                    WorkerName = row.WorkerName,
                    WorkerLastname = row.WorkerLastname

                });
            }
            return View(kaizenUserEdits.ToPagedList(page ?? 1, 15));
        }

        
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            ViewBag.Message = "Your application description page.";
            KaizenUserEditModel kaizenUserEdit = new KaizenUserEditModel();
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["KaizenDB"].ConnectionString))
            {
                kaizenUserEdit = db.Query<KaizenUserEditModel>(
                     "SELECT * FROM dbo.KaizenIdea WHERE IdeaID = " + id).SingleOrDefault();
            }

            return View(kaizenUserEdit);
        }
        public ActionResult Edit(int id, KaizenUserEditModel model)
        {


            KaizenUserEditModel kaizenUserEdit = new KaizenUserEditModel();
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["KaizenDB"].ConnectionString))
            {
                kaizenUserEdit = db.Query<KaizenUserEditModel>(
                     "SELECT * FROM dbo.KaizenIdea WHERE IdeaID = " + id).SingleOrDefault();
            }
            if (ModelState.IsValid)
            {
                int recordsUpdated = UpdateKaizen(model.IdeaID, model.KaizenSuggestion, model.KaizenSolution);
                return RedirectToAction("Edit");

            }

            return View(kaizenUserEdit);
        }

        public async Task<ActionResult> Completed(int id, ConfirmCompleteModel model, LocalModel m)
        {


            ConfirmCompleteModel kaizenConfirmCompleted = new ConfirmCompleteModel();
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["KaizenDB"].ConnectionString))
            {
                kaizenConfirmCompleted = db.Query<ConfirmCompleteModel>(
                     "SELECT * FROM dbo.KaizenIdea WHERE IdeaID = " + id).SingleOrDefault();
            }
            if (ModelState.IsValid)
            {
                int recordsUpdated = ConfirmCompletedmKaizen(kaizenConfirmCompleted.IdeaID, model.Status);
                try
                {

                    LocalModel managerEmail = new LocalModel();
                    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["KaizenDB"].ConnectionString))
                    {

                        managerEmail = db.Query<LocalModel>(
                             "SELECT [ManagerEmail] FROM [Kaizen].[dbo].[Department] where DepartmentName = '" + kaizenConfirmCompleted.DepartmentName.ToString() + "'").SingleOrDefault();
                    }

                    //From Address    
                    string FromAddress = "GMBX-NVS12@lear.com";
                    string FromAdressTitle = "Kaizen";
                    //To Address    
                    string ToAddress = managerEmail.ManagerEmail;
                    string ToAdressTitle = "ACTION REQUIRED:  Approval for Kaizen Idea ";
                    string Subject = "ACTION REQUIRED:  Approval for Kaizen Idea";
                    string BodyContent = ("Kaizen Suggestion Confirmed Completion: " + kaizenConfirmCompleted.KaizenSuggestion + "      by: " + m.UseName + "        Status:" + model.Status + "       Visit  http://rsnvs-ds01/Manager/Approvallist for further details.");

                    //Smtp Server    
                    string SmtpServer = "eu.smtp.lear.com";
                    //Smtp Port Number    
                    int SmtpPortNumber = 25;

                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress
                                            (FromAdressTitle,
                                             FromAddress
                                             ));
                    mimeMessage.To.Add(new MailboxAddress
                                             (ToAdressTitle,
                                             ToAddress
                                             ));
                    mimeMessage.Subject = Subject; //Subject  
                    mimeMessage.Body = new TextPart("plain")
                    {
                        Text = BodyContent
                    };


                    using (var client = new SmtpClient())
                    {
                        client.Connect(SmtpServer, SmtpPortNumber, false);
                        client.LocalDomain = "corp.lear.com";

                        await Task.Run(() => client.Send(mimeMessage));
                        await Task.Run(() => client.Disconnect(true));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //User Email
                try
                {

                    

                    //From Address    
                    string FromAddress = "GMBX-NVS12@lear.coml";
                    string FromAdressTitle = "Kaizen";
                    //To Address    
                    string ToAddress = m.UseName + "@lear.com";
                    string ToAdressTitle = "ACTION REQUIRED:  Approval for Kaizen Idea ";
                    string Subject = "Potvrda o implementaciji KAIZEN Predlog uspešno podneta";
                    string BodyContent = ("Kaizen predlog sa ID: "+id+"  je uspešno završen         http://rsnvs-ds01/Home/details/"+id);

                    //Smtp Server    
                    string SmtpServer = "eu.smtp.lear.com";
                    //Smtp Port Number    
                    int SmtpPortNumber = 25;

                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress
                                            (FromAdressTitle,
                                             FromAddress
                                             ));
                    mimeMessage.To.Add(new MailboxAddress
                                             (ToAdressTitle,
                                             ToAddress
                                             ));
                    mimeMessage.Subject = Subject; //Subject  
                    mimeMessage.Body = new TextPart("plain")
                    {
                        Text = BodyContent
                    };


                    using (var client = new SmtpClient())
                    {
                        client.Connect(SmtpServer, SmtpPortNumber, false);
                        client.LocalDomain = "corp.lear.com";

                        await Task.Run(() => client.Send(mimeMessage));
                        await Task.Run(() => client.Disconnect(true));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return RedirectToAction("MyKaizen");

            }

            return View(kaizenConfirmCompleted);
        }

        //public ActionResult ManagerAdd(ManagerEditModel model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        int recordsCreated = CreateManager(model.DepartmentID, model.DepartmentName, model.DepartmentManager, model.ManagerEmail, model.ManagerUsername);
        //        return RedirectToAction("ManagerEdit");
        //    }
        //    else
        //    {
        //        return View();
        //    }

        //}
        //public ActionResult ManagerUpdate(int id, ManagerEditModel model)
        //{
        //    ViewBag.Message = "Your contact page.";
        //    if (ModelState.IsValid)
        //    {
        //        int recordsUpdated = UpdateManager(model.DepartmentID, model.DepartmentName, model.DepartmentManager, model.ManagerEmail, model.ManagerUsername);
        //    }
        //    ManagerEditModel kaizenManagerDetails = new ManagerEditModel();
        //    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["KaizenDB"].ConnectionString))
        //    {
        //        kaizenManagerDetails = db.Query<ManagerEditModel>(
        //             "SELECT * FROM dbo.Department WHERE DepartmentID = " + id).SingleOrDefault();
        //    }

        //    return View(kaizenManagerDetails);
        //}
    }
}