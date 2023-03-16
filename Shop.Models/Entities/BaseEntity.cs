namespace Shop.Models.Entities
{
    public class BaseEntity<TKey> : IEntityBase<TKey>
    {
        public TKey Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
