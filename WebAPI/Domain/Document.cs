namespace WebAPI.Domain
{
    [Serializable]
    public class Document
    {
        public int Id { get; set; }
        public List<string> Tags { get; set; } = [];
        public object Data { get; set; } = new();
    }
}
