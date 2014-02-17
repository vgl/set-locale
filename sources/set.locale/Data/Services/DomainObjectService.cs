using System.Linq;
using System.Threading.Tasks;

using set.locale.Data.Entities;
using set.locale.Helpers;

namespace set.locale.Data.Services
{
    public class DomainObjectService : BaseService, IDomainObjectService
    {
        public Task<bool> Create(string name, string updatedBy)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == updatedBy);
            if (user == null) return Task.FromResult(false);

            var model = new DomainObject { Name = name, CreatedBy = user.Id };

            Context.DomainObjects.Add(model);

            return Task.FromResult(Context.SaveChanges() > 0);
        }

        public Task<PagedList<DomainObject>> GetDomainObjects(int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var query = Context.Set<DomainObject>();

            var count = query.Count();
            var items = query.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize).ToList();

            return Task.FromResult(new PagedList<DomainObject>(pageNumber, ConstHelper.PageSize, count, items));
        }

        public Task<DomainObject> Get(string id)
        {
            return Task.FromResult(Context.Set<DomainObject>().Find(id));
        }
    }

    public interface IDomainObjectService
    {
        Task<bool> Create(string name, string updatedBy);

        Task<PagedList<DomainObject>> GetDomainObjects(int pageNumber);

        Task<DomainObject> Get(string id);
    }
}