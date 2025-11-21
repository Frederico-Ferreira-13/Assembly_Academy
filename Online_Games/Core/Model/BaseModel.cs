
namespace Core.Model
{
    public abstract class BaseModel<TKey> where TKey : IEquatable<TKey>
    {       
        public TKey Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? LastUpdatedAt { get; protected set; }
        public bool IsActive { get; protected set; }

        protected BaseModel(TKey id)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        protected BaseModel(TKey id, DateTime createdAt, DateTime ? lastUpdatedAt, bool isActive)
        {
            Id = id;
            CreatedAt = createdAt;
            LastUpdatedAt = lastUpdatedAt;
            IsActive = isActive;
        }

        public virtual void Deactive()
        {
            if (!IsActive)
            {
                return;
            }
            IsActive = false;
            LastUpdatedAt = DateTime.UtcNow;
        }

        public virtual void Activate()
        {
            if (IsActive)
            {
                return;
            }

            IsActive = true;
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void SetId(TKey newId)
        {
            Id = newId;
        }
    }
}
