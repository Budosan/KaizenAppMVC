using KaizenAppMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace KaizenAppMVC.Models
{
    public static class ProcessorModel
    {
        public static int CreateKaizenAsync(string KaizenSuggestion, string KaizenSolution, string Department, string Status, string WorkerID, string WorkerName, string WorkerLastname, string UserName)
        {
            
            KaizenModel data = new KaizenModel
            {
                Status ="Submitted",
                KaizenSuggestion = KaizenSuggestion,
                KaizenSolution = KaizenSolution,
                Department = Department,                
                WorkerID = WorkerID,
                WorkerName = WorkerName,
                WorkerLastname = WorkerLastname,
                UserName = HttpContext.Current.User.Identity.Name.Substring(HttpContext.Current.User.Identity.Name.IndexOf("\\") + 1).ToString(),
        };

            string sql = @"USE [Kaizen] INSERT INTO [dbo].[KaizenIdea] ([WorkerLastname], [WorkerName], [WorkerID], [DepartmentName], [KaizenSuggestion], [KaizenSolution], [Status], [UserName]) VALUES (@WorkerLastname, @WorkerName, @WorkerID, @Department, @KaizenSuggestion, @KaizenSolution, @Status, @UserName)";
            return SqlDataAccess.SaveData(sql, data);
            
        }
        public static List<KaizenPregled> LoadKaizenData()
        {
            
            string sql = @"SELECT * FROM [Kaizen].[dbo].[KaizenIdea] ORDER BY IdeaID DESC";
            return SqlDataAccess.LoadData<KaizenPregled>(sql);
        }
        public static List<KaizenUserEditModel> LoadKaizenUserData()
        {
            string UserName = HttpContext.Current.User.Identity.Name.Substring(HttpContext.Current.User.Identity.Name.IndexOf("\\") + 1).ToString();
            string sql = @"USE [Kaizen] SELECT IdeaID, DepartmentName, KaizenSuggestion, KaizenSolution, DateTime, WorkerID, WorkerLastname, WorkerName, Status, Comment, EmailSentTo FROM dbo.KaizenIdea WHERE KaizenIdea.UserName = '"+UserName+"' ORDER BY IdeaID DESC";
            return SqlDataAccess.LoadData<KaizenUserEditModel>(sql);
        }

        public static int CreateManager(int DepartmentID, string DepartmentName, string DepartmentManager, string ManagerEmail, string ManagerUsername)
        {
            ManagerEditModel data = new ManagerEditModel
            {
                DepartmentID = DepartmentID,
                DepartmentName = DepartmentName,
                DepartmentManager = DepartmentManager,
                ManagerEmail = ManagerEmail,
                ManagerUsername = ManagerUsername

            };
            string sql = @"USE [Kaizen] INSERT INTO [dbo].[Department] ([DepartmentID], [DepartmentName], [DepartmentManager], [ManagerEmail], [ManagerUsername]) VALUES (@DepartmentID, @DepartmentName, @DepartmentManager, @ManagerEmail, @ManagerUsername)";
            return SqlDataAccess.SaveData(sql, data);
        }
        public static List<AuthorizationManager> LoadManagerUserData()
        {
            string sql = @"SELECT * 
                         FROM [Kaizen].[dbo].[Department].[Username]";
            return SqlDataAccess.LoadData<AuthorizationManager>(sql);

        }

        public static int UpdateManager(int DepartmentID, string DepartmentName, string DepartmentManager, string ManagerEmail, string ManagerUsername)
        {
            ManagerEditModel data = new ManagerEditModel
            {
                DepartmentID = DepartmentID,
                DepartmentName = DepartmentName,
                DepartmentManager = DepartmentManager,
                ManagerEmail = ManagerEmail,
                ManagerUsername = ManagerUsername
            };
            string sql = @"USE [Kaizen] UPDATE [dbo].[Department] SET DepartmentID = @DepartmentID, DepartmentManager = @DepartmentManager, ManagerEmail = @ManagerEmail, ManagerUsername = @ManagerUsername WHERE DepartmentName = @DepartmentName";
            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<ManagerEditModel> LoadManagerData()
        {
            string sql = @"SELECT * FROM [Kaizen].[dbo].[Department]";
            return SqlDataAccess.LoadData<ManagerEditModel>(sql);
        }
        public static List<KaizenApproval> LoadKaizenApproval()
        {
            string UserName = HttpContext.Current.User.Identity.Name.Substring(HttpContext.Current.User.Identity.Name.IndexOf("\\") + 1).ToString();
            string sql = @"USE [Kaizen] Select * from KaizenIdea as k inner join Department as t on t.DepartmentName=k.DepartmentName where t.ManagerUsername='"+UserName+ "'  and k.Status='Submitted' ORDER BY IdeaID DESC";
            return SqlDataAccess.LoadData<KaizenApproval>(sql);
        }
        public static int UpdateKaizen(int IdeaID, string KaizenSuggestion, string KaizenSolution)
        {
            KaizenUserEditModel data = new KaizenUserEditModel
            {
                IdeaID = IdeaID,
                KaizenSolution = KaizenSolution,
                KaizenSuggestion = KaizenSuggestion
            };
            string sql = @"USE [Kaizen] UPDATE [dbo].[KaizenIdea] SET KaizenSolution = @KaizenSolution, KaizenSuggestion = @KaizenSuggestion WHERE IdeaID = @IdeaID";
            return SqlDataAccess.SaveData(sql, data);

        }

        public static int ApproveKaizen(int IdeaID, string DepartmentName, string Status, string Comment, string DateOfApproval, string ApprovedBy)
        {
            KaizenApproval data = new KaizenApproval
            {
                IdeaID = IdeaID,
                DepartmentName = DepartmentName,
                Status = Status,
                Comment = Comment,
                DateOfApproval = DateOfApproval,
                ApprovedBy = ApprovedBy
            };
            string sql = @"USE [Kaizen] UPDATE [dbo].[KaizenIdea] SET DepartmentName = @DepartmentName, Status = @Status, Comment = @Comment, DateOfApproval = @DateOfApproval, ApprovedBy = @ApprovedBy WHERE IdeaID = @IdeaID";
            return SqlDataAccess.SaveData(sql, data);

        }
        public static List<AuthorizationManager> LoadManagerUsernameData()
        {
            string UserName = HttpContext.Current.User.Identity.Name.Substring(HttpContext.Current.User.Identity.Name.IndexOf("\\") + 1).ToString();
            string sql = @"SELECT ManagerUsername FROM dbo.Department WHERE Department.ManagerUsername ='"+UserName+"'";
            return SqlDataAccess.LoadData<AuthorizationManager>(sql);
        }
        public static List<KaizenApproval> LoadManagerApprovedData()
        {
            string UserName = HttpContext.Current.User.Identity.Name.Substring(HttpContext.Current.User.Identity.Name.IndexOf("\\") + 1).ToString();
            string sql = @"USE [Kaizen] Select * from KaizenIdea as k inner join Department as t on t.DepartmentName=k.DepartmentName where t.ManagerUsername='" + UserName + "'  and k.Status='Approved' ORDER BY IdeaID DESC";
            return SqlDataAccess.LoadData<KaizenApproval>(sql);
        }

        public static List<KaizenModel> LoadDepartmentData()
        {
            string sql = @"SELECT DepartmentName FROM [Kaizen].[dbo].[Department]";
            return SqlDataAccess.LoadData<KaizenModel>(sql);
        }
        public static int ConfirmKaizen(int IdeaID, string Status, string CiComment, string DateCompleted)
        {
            KaizenApproval data = new KaizenApproval
            {
                DateCompleted = DateCompleted,
                CiComment = CiComment,
                IdeaID = IdeaID,
                Status = Status
            };
            string sql = @"USE [Kaizen] UPDATE [dbo].[KaizenIdea] SET Status = @Status, CiComment = @CiComment, DateCompleted = @DateCompleted WHERE IdeaID = @IdeaID";
            return SqlDataAccess.SaveData(sql, data);

        }
        public static int ConfirmCompletedmKaizen(int IdeaID, string Status)
        {
            ConfirmCompleteModel data = new ConfirmCompleteModel
            {
                IdeaID = IdeaID,
                Status = Status
            };
            string sql = @"USE [Kaizen] UPDATE [dbo].[KaizenIdea] SET Status = @Status WHERE IdeaID = @IdeaID";
            return SqlDataAccess.SaveData(sql, data);

        }
        public static List<KaizenApproval> LoadCIDash()
        {
            string UserName = HttpContext.Current.User.Identity.Name.Substring(HttpContext.Current.User.Identity.Name.IndexOf("\\") + 1).ToString();
            string sql = @"USE [Kaizen] Select * from KaizenIdea ORDER BY IdeaID DESC";
            return SqlDataAccess.LoadData<KaizenApproval>(sql);
        }
    }
}
