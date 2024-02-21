namespace WebAPI.Models
{
    public enum OperationType
    {
        Add,
        Update,
        Delete,
        None
    }

    public class Editable<T> where T : new()
    {
        public OperationType OperationType { get; set; }
        public T Object { get; set; } = new();
    }
}
