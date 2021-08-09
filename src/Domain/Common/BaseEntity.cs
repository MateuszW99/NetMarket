using System;

namespace Domain.Common
{
    public abstract class BaseEntity
    {
        public DateTime Created { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid? LastModifiedBy { get; set; }
    }
}