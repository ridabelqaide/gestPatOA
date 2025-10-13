namespace PATOA.CORE.Interfaces
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
} 