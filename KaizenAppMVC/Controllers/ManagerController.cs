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
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using MimeKit;
using PagedList;
using PagedList.Mvc;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;

namespace KaizenAppMVC.Controllers
{
    public class ManagerController : Controller
    {
        


        public ActionResult List()
        {
            ViewBag.Message = "Preview";

            var data = LoadManagerData();

            List<ManagerEditModel> managerList = new List<ManagerEditModel>();

            foreach (var row in data)
            {
                managerList.Add(new ManagerEditModel
                {

                    DepartmentID = row.DepartmentID,
                    DepartmentName = row.DepartmentName,
                    DepartmentManager = row.DepartmentManager,
                    ManagerEmail = row.ManagerEmail,
                    ManagerUsername = row.ManagerUsername


                });
            }
            return View(managerList);
        }


        
        
        public ActionResult Edit(int id, ManagerEditModel model)
        {
            ManagerEditModel kaizenManagerDetails = new ManagerEditModel();
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["KaizenDB"].ConnectionString))
            {
                kaizenManagerDetails = db.Query<ManagerEditModel>(
                     "SELECT * FROM dbo.Department WHERE DepartmentID = " + id).SingleOrDefault();
            }
            if (ModelState.IsValid)
            {

                int recordsUpdated = UpdateManager(model.DepartmentID, model.DepartmentName, model.DepartmentManager, model.ManagerEmail, model.ManagerUsername);
                return RedirectToAction("List");
            }
            return View(kaizenManagerDetails);
        }




        // GET: Manager/Create
        public ActionResult Add(ManagerEditModel model)
        {

            if (ModelState.IsValid)
            {
                int recordsCreated = CreateManager(model.DepartmentID, model.DepartmentName, model.DepartmentManager, model.ManagerEmail, model.ManagerUsername);
                return RedirectToAction("ManagerEdit");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Approvallist(int? page)
        {

            {
                ViewBag.Message = "MY Kaizen Suggestion";

                var data = LoadKaizenApproval();

                List<KaizenApproval> kaizenApproval = new List<KaizenApproval>();

                foreach (var row in data)
                {
                    kaizenApproval.Add(new KaizenApproval
                    {
                        IdeaID = row.IdeaID,
                        KaizenSuggestion = row.KaizenSuggestion,
                        KaizenSolution = row.KaizenSolution,
                        WorkerName = row.WorkerName,
                        WorkerID = row.WorkerID,
                        DateTime = row.DateTime,
                        UserName = row.UserName,
                        WorkerLastname = row.WorkerLastname,
                        DepartmentName = row.DepartmentName,
                        Status = row.Status


                    });
                }
                return View(kaizenApproval.ToPagedList(page ?? 1, 15));
            }
        }


        public ActionResult ApprovedList(int? page)
        {
            ViewBag.Message = "Preview";

            var data = LoadManagerApprovedData();

            List<KaizenApproval> managerApprovedList = new List<KaizenApproval>();

            foreach (var row in data)
            {
                managerApprovedList.Add(new KaizenApproval
                {

                    IdeaID = row.IdeaID,
                    KaizenSuggestion = row.KaizenSuggestion,
                    KaizenSolution = row.KaizenSolution,
                    WorkerName = row.WorkerName,
                    WorkerID = row.WorkerID,
                    DateTime = row.DateTime,
                    UserName = row.UserName,
                    WorkerLastname = row.WorkerLastname,
                    DepartmentName = row.DepartmentName,
                    DateOfApproval = row.DateOfApproval,
                    Status = row.Status


                });
            }
            return View(managerApprovedList.ToPagedList(page ?? 1, 100));
        }







        public async Task<ActionResult> MyApprovals(int id, KaizenApproval model, LocalModel localModel)
        {
            KaizenApproval kaizenApprovals = new KaizenApproval();
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["KaizenDB"].ConnectionString))
            {
                kaizenApprovals = db.Query<KaizenApproval>(
                     "SELECT * FROM dbo.KaizenIdea WHERE IdeaID = " + id).SingleOrDefault();
            }

            if (ModelState.IsValid)
            {
                if (model.Status != null)
                {
                    int recordsUpdated = ApproveKaizen(model.IdeaID, model.DepartmentName, model.Status, model.Comment, model.DateOfApproval = DateTime.Now.ToString(), model.ApprovedBy = localModel.UseName);
                }
                else
                {
                    int recordsUpdated = ApproveKaizen(model.IdeaID, model.DepartmentName, model.Status = "Submitted", model.Comment, model.DateOfApproval = null, model.ApprovedBy = null);
                }

                LocalModel ManagerEmail = new LocalModel();
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["KaizenDB"].ConnectionString))
                {
                    ManagerEmail = db.Query<LocalModel>(
                         "SELECT ManagerEmail, DepartmentManager FROM [Kaizen].[dbo].[Department] WHERE Departmentname = '" + model.DepartmentName + "'").SingleOrDefault();
                }




                if (model.Status == "Submitted")
                {
                    //ManagerEmail on forward
                    try
                    {


                        //From Address    
                        string FromAddress = "GMBX-NVS12@lear.com";
                        string FromAdressTitle = "Kaizen";
                        //To Address    
                        string ToAddress = ManagerEmail.ManagerEmail;
                        string ToAdressTitle = "Kaizen Predlog za odobrenje";
                        string Subject = "ACTION REQUIRED:  Approval for Kaizen Idea";
                        string BodyContent = ("Predlog je prebačen vama od strane : " + localModel.UseName + "     Kaizen Predlog: " + kaizenApprovals.KaizenSuggestion + "          Id naloga: " + id + "             Status:  " + model.Status + "      Visit http://rsnvs-ds01/manager/approvallist/" + id);

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
                        mimeMessage.Body = new TextPart("plain", "html")
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
                    //confirm forwarded request mail
                    try
                    {


                        //From Address    
                        string FromAddress = "GMBX-NVS12@lear.com";
                        string FromAdressTitle = "Kaizen";
                        //To Address    
                        string ToAddress = ManagerEmail.UseName+"@lear.com";
                        string ToAdressTitle = "Kaizen Predlog za odobrenje";
                        string Subject = "Kaizen predlog uspešno prebačen na"+model.DepartmentName;
                        string BodyContent = ("Kaizen Predlog: " + kaizenApprovals.KaizenSuggestion + "          Id naloga: " + id + "             je uspešno prebačen"+ "      Visit http://rsnvs-ds01/home/details/" + id);

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
                        mimeMessage.Body = new TextPart("plain", "html")
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
                    //User Email on status update

                    try
                    {


                        //From Address    
                        string FromAddress = "GMBX-NVS12@lear.com";
                        string FromAdressTitle = "Kaizen";
                        //To Address    
                        string ToAddress = kaizenApprovals.UserName + "@lear.com";
                        string ToAdressTitle = "Kaizen Predlog za odobrenje";
                        string Subject = "Kaizen predlog je prebačen da drugi department";
                        string BodyContent = ("Predlog je prebačen na:"+model.DepartmentName+"        od strane : " + ManagerEmail.DepartmentManager + "     Kaizen Predlog: " + kaizenApprovals.KaizenSuggestion + "          Id naloga: " + id + "             Status:  " + model.Status + "      Visit http://rsnvs-ds01/manager/approvallist/" + id);

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
                        mimeMessage.Body = new TextPart("plain", "html")
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

                }
                if (model.Status == "Approved")
                {
                    try
                    {


                        //From Address    
                        string FromAddress = "GMBX-NVS12@lear.com";
                        string FromAdressTitle = "Kaizen";
                        //To Address    
                        string ToAddress = kaizenApprovals.UserName + "@lear.com";
                        string ToAdressTitle = "Kaizen Predlog za odobrenje";
                        string Subject = "Kaizen Predlog: Status ažuriran";
                        string BodyContent = ("Kaizen Predlog: " + model.KaizenSuggestion + "          Id naloga: " + id + "             Status:  " + model.Status + "      Komentar:" + model.Comment + "      http://rsnvs-ds01/home/details/" + id);

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
                        mimeMessage.Body = new TextPart("plain", "html")
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
                    try
                    {


                        //From Address    
                        string FromAddress = "GMBX-NVS12@lear.com";
                        string FromAdressTitle = "Kaizen";
                        //To Address    
                        string ToAddress = "mindjin@lear.com";
                        string ToAdressTitle = "Kaizen Predlog za odobrenje";
                        string Subject = "Novi Kaizen predlgog je odobren";
                        string BodyContent = ("Kaizen Predlog: " + model.KaizenSuggestion + "          Id naloga: " + id + "             Status:  " + model.Status + "       Komentar:" + model.Comment + "      http://rsnvs-ds01/manager/CI/" + id);

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
                        mimeMessage.Body = new TextPart("plain", "html")
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
                }
                else
                {
                    try
                    {


                        //From Address    
                        string FromAddress = "GMBX-NVS12@lear.com";
                        string FromAdressTitle = "Kaizen";
                        //To Address    
                        string ToAddress = kaizenApprovals.UserName + "@lear.com";
                        string ToAdressTitle = "Kaizen Predlog za odobrenje";
                        string Subject = "Kaizen Predlog: Status ažuriran";
                        string BodyContent = ("Kaizen Predlog: " + model.KaizenSuggestion + "     Kaizen Predlog: " + kaizenApprovals.KaizenSuggestion + "          Id naloga: " + id + "             Status:  " + model.Status + "      Komentar:" + model.Comment + "      http://rsnvs-ds01/home/details/" + id);

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
                        mimeMessage.Body = new TextPart("plain", "html")
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
                }


                return RedirectToAction("Approvallist");
            }
            return View(kaizenApprovals);
        }

        public async Task<ActionResult> CI(int id, CompletedKaizenModel model, LocalModel m)
        {
            CompletedKaizenModel kaizenApprovals = new CompletedKaizenModel();
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["KaizenDB"].ConnectionString))
            {
                kaizenApprovals = db.Query<CompletedKaizenModel>(
                     "SELECT * FROM dbo.KaizenIdea WHERE IdeaID = " + id).SingleOrDefault();
            }
            if (ModelState.IsValid)
            {
                int recordsUpdated = ConfirmKaizen(kaizenApprovals.IdeaID, model.Status, model.CiComment, model.DateCompleted = DateTime.Now.ToString());

                if (model.Status == "Completed")
                {
                    try
                    {


                        //From Address    
                        string FromAddress = "GMBX-NVS12@lear.com";
                        string FromAdressTitle = "Kaizen";
                        //To Address    
                        string ToAddress = kaizenApprovals.UserName + "@lear.com";
                        string ToAdressTitle = "Kaizen Predlog za odobrenje";
                        string Subject = "Kaizen predlog je implementiran";
                        string BodyContent = ("Kaizen predlog sa ID:" + id + " je implementiran posetite http://rsnvs-ds01/home/completed/" + id + "  kako bi ste potvrdili status predloga");

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
                        mimeMessage.Body = new TextPart("plain", "html")
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
                    
                }

            }

            
            return View(kaizenApprovals);
        }
        public ActionResult CIDash(int? page)
        {

            {
                ViewBag.Message = "MY Kaizen Suggestion";

                var data = LoadCIDash();

                List<CompletedKaizenModel> kaizenApproval = new List<CompletedKaizenModel>();

                foreach (var row in data)
                {
                    kaizenApproval.Add(new CompletedKaizenModel
                    {
                        IdeaID = row.IdeaID,
                        KaizenSuggestion = row.KaizenSuggestion,
                        KaizenSolution = row.KaizenSolution,
                        WorkerName = row.WorkerName,
                        WorkerID = row.WorkerID,
                        DateTime = row.DateTime,
                        UserName = row.UserName,
                        WorkerLastname = row.WorkerLastname,
                        DepartmentName = row.DepartmentName,
                        Status = row.Status,
                        ApprovedBy = row.ApprovedBy,
                        DateCompleted = row.DateCompleted,
                        DateOfApproval = row.DateOfApproval
                        

                    });
                }
                return View(kaizenApproval.ToPagedList(page ?? 1, 30000));
            }
        }

    }
}
