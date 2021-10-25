using System;

namespace ECommerce.Models
{
    [Serializable]
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public BaseEntity()
        {

        }

        protected BaseEntity(int id)
        {
            Id = id;
        }
    }

}
