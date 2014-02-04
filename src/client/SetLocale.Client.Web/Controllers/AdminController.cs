using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IAppService _appService;

        public AdminController(
            IUserService userService, 
            IFormsAuthenticationService formsAuthenticationService, 
            IAppService appService)
            : base(userService, formsAuthenticationService)
        {
            _appService = appService;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CurrentUser.RoleId != SetLocaleRole.Admin.Value)
            {
                filterContext.Result = RedirectToHome();    
            }

            base.OnActionExecuting(filterContext); 
        }

        [HttpGet]
        public ActionResult Index()
        {
            return Redirect("/admin/apps");
        }

        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Importexcel()
        {
            var excelFileBase = Request.Files["import_excel"];
            if (excelFileBase != null && excelFileBase.ContentLength > 0)
            {
                var excelFile  = Request.Files["import_excel"];
                if (excelFile != null)
                {
                    string extension = System.IO.Path.GetExtension(excelFile.FileName);
                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        var date = DateTime.Now.Date.ToString("dd-MM-yy");
                        var excelName = date + "-" + Guid.NewGuid();

                        string path1 = string.Format("{0}/{1}", Server.MapPath("~/Public/files"), excelName + extension);

                        if (System.IO.File.Exists(path1))
                            System.IO.File.Delete(path1);

                        excelFile.SaveAs(path1);

                        string excelConnectionString = string.Empty;

                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    if (extension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (extension == ".xlsx")
                    {
                        
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }

                    var excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    var dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }

                    var excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    var excelConnection1 = new OleDbConnection(excelConnectionString);
                    var ds = new DataSet();

                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (var dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                   
                       var word = new Word
                       {
                           Key = ds.Tables[0].Rows[i]["key"].ToString(),
                           Description = ds.Tables[0].Rows[i]["Description"].ToString(),
                           Translation_TR = ds.Tables[0].Rows[i]["Translation_TR"].ToString(),
                           Translation_EN = ds.Tables[0].Rows[i]["Translation_EN"].ToString(),
                           Translation_AZ = ds.Tables[0].Rows[i]["Translation_AZ"].ToString(),
                           Translation_CN = ds.Tables[0].Rows[i]["Translation_CN"].ToString(),
                           Translation_FR = ds.Tables[0].Rows[i]["Translation_FR"].ToString(),
                           Translation_GR = ds.Tables[0].Rows[i]["Translation_GR"].ToString(),
                           Translation_IT = ds.Tables[0].Rows[i]["Translation_IT"].ToString(),
                           Translation_KZ = ds.Tables[0].Rows[i]["Translation_KZ"].ToString(),
                           Translation_RU = ds.Tables[0].Rows[i]["Translation_RU"].ToString(),
                           Translation_SP = ds.Tables[0].Rows[i]["Translation_SP"].ToString(),
                           Translation_TK = ds.Tables[0].Rows[i]["Translation_TK"].ToString()
                       };
                        //word.Tag = ds.Tables[0].Rows[i]["tags"].ToString();

                        var streampost = new MemoryStream();
                        var clientpost = new WebClient();
                        clientpost.Headers["Content-type"] = "application/json";

                        //DataContractJsonSerializer serializerpost = new
                        // DataContractJsonSerializer(typeof(WordModel));
                        //serializerpost.WriteObject(streampost, model);

                        //string url1 = string.Format("{0}SavePlayerDetailsAPI", serviceUrl);
                        //byte[] datapost = clientpost.UploadData(url1, "Post", streampost.ToArray());

                        //streampost = new MemoryStream(datapost);
                        //serializerpost = new DataContractJsonSerializer(typeof(string));
                        //var result = (string)serializerpost.ReadObject(streampost);

                    }
                    ViewBag.message = "Information saved successfully.";
                }
                else
                {
                    ModelState.AddModelError("", "Plese select Excel File.");
                }
            }
                     ModelState.AddModelError("", "Plese select Excel File.");  
                    }
            return Redirect("/admin/import");
        }
      

            
        [HttpGet]
        public ViewResult NewTranslator()
        {
            var model = new UserModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> NewTranslator(UserModel model)
        {
            if (!model.IsValidForNewTranslator())
            {
                model.Msg = "bir sorun oluştu...";
                return View(model);
            }

            model.Password = Guid.NewGuid().ToString().Replace("-", string.Empty);
            model.Language = Thread.CurrentThread.CurrentUICulture.Name;
            var userId = await _userService.Create(model, SetLocaleRole.Translator.Value);
            if (userId == null)
            {
                model.Msg = "bir sorun oluştu...";
                return View(model);
            }

            //todo:send mail to translator to welcome and ask for reset password

            return Redirect("/admin/users");
        }
         
        [HttpGet]
        public async Task<ActionResult> Users(int id = 0, int page = 1)
        {
            var pageNumber = page;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            PagedList<User> users;

            ViewBag.RoleId = id;
            if (SetLocaleRole.IsValid(id))
            {
                users = await _userService.GetAllByRoleId(id, pageNumber);
            }
            else
            {
                users = await _userService.GetUsers(pageNumber);
            }

            var list = users.Items.Select(UserModel.MapUserToUserModel).ToList();

            var model = new PageModel<UserModel>
            {
                Items = list,
                HasNextPage = users.HasNextPage,
                HasPreviousPage = users.HasPreviousPage,
                Number = users.Number,
                TotalCount = users.TotalCount,
                TotalPageCount = users.TotalPageCount
            };
             
            return View(model);
        }
          
        [HttpGet]
        public async Task<ActionResult> Apps(int id = 0)
        {
            var pageNumber = id; 
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            var apps = await _appService.GetApps(pageNumber);

            var list = apps.Items.Select(AppModel.MapFromEntity).ToList();

            var model = new PageModel<AppModel>
            {
                Items = list,
                HasNextPage = apps.HasNextPage,
                HasPreviousPage = apps.HasPreviousPage,
                Number = apps.Number,
                TotalCount = apps.TotalCount,
                TotalPageCount = apps.TotalPageCount
            };

            return View(model);
        }
    }
}