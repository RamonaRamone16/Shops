using EntityFrameworkCore.CommonTools;

namespace Shop.Models.Entities
{
    public interface IEntityBase<TKey> : IEntityBase
    {
        TKey Id { get; set; }
    }
    public interface IEntityBase : IDeletionTrackable, IModificationTrackable, ICreationTrackable
    {
    }
}
