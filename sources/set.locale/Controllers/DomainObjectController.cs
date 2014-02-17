using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using set.locale.Data.Services;
using set.locale.Helpers;
using set.locale.Models;

namespace set.locale.Controllers
{
    public class DomainObjectController : BaseController
    {
        private readonly IDomainObjectService _domainObjectService;

        public DomainObjectController(IDomainObjectService domainObjectService)
        {
            _domainObjectService = domainObjectService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult New()
        {
            return View(new DomainObjectModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> New(DomainObjectModel model)
        {
            SetPleaseTryAgain(model);

            if (model.IsNotValid())
            {
                return View(model);
            }

            model.IsOk = await _domainObjectService.Create(model.Name, User.Identity.GetId());
            if (!model.IsOk) return View(model);

            if (!model.IsButtonSaveAndNew) return RedirectToAction("list");

            model.IsOk = true;
            model.Name = string.Empty;
            model.Msg = "data_saved_successfully_msg".Localize();
            return View(model);
        }

        [HttpGet]
        public async Task<ViewResult> List(int id = 1)
        {
            var result = await _domainObjectService.GetDomainObjects(id);
            var list = result.Items.Select(DomainObjectModel.Map).ToList();
            var model = new PageModel<DomainObjectModel>
            {
                Items = list,
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage,
                Number = result.Number,
                TotalCount = result.TotalCount,
                TotalPageCount = result.TotalPageCount
            };
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string id)
        {
            var result = await _domainObjectService.Get(id);

            if (result == null) return RedirectToHome();

            var model = DomainObjectModel.Map(result);
            return View(model);
        }
    }
}