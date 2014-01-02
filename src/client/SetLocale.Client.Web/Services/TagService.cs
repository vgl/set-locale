using System.Collections.Generic;
using System.Threading.Tasks;

using SetLocale.Client.Web.Entities;

namespace SetLocale.Client.Web.Services
{
    public interface ITagService
    {
        Task<List<Word>> GetWords(string tagId);
    }

    public class TagService : ITagService
    {
        public Task<List<Word>> GetWords(string tagId)
        {
            throw new System.NotImplementedException();
        }
    }
}