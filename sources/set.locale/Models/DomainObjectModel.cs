using set.locale.Data.Entities;

namespace set.locale.Models
{
    public class DomainObjectModel : BaseModel
    {
        public string Name { get; set; }

        public bool IsButtonSaveAndNew { get; set; }

        internal bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        internal bool IsNotValid()
        {
            return !IsValid();
        }

        public static DomainObjectModel Map(DomainObject entity)
        {
            var model = new DomainObjectModel
            {
                Name = entity.Name
            };
            return model;
        }
    }
}