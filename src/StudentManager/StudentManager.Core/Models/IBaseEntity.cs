namespace StudentManager.Core.Models
{
    public interface IBaseEntity<TId>
    {
        public TId Id { get; }
    }
}