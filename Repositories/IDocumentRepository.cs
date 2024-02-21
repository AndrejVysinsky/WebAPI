using WebAPI.Models;

namespace WebAPI.Repositories
{
    public interface IDocumentRepository
    {
        Task<Document?> GetDocument(int id);
        Task SaveDocument(Document document);
        Task UpdateDocument(Document document);
    }
}
