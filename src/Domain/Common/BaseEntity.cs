using System;

namespace Domain.Common
{
    public abstract class BaseEntity
    {
        public DateTime Created { get; init; }
        public Guid CreatedBy { get; init; }
        public DateTime? LastModified { get; set; }
        public Guid? LastModifiedBy { get; set; }
    }
}