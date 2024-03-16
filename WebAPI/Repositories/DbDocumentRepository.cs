using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebAPI.Data;

namespace WebAPI.Repositories
{
    public class DbDocumentRepository : IDocumentRepository
    {
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _dbContext;

        public DbDocumentRepository(IMemoryCache cache, AppDbContext dbContext)
        {
            _cache = cache;
            _dbContext = dbContext;
        }

        public async Task<Document?> GetDocument(int id)
        {
            var document = await _dbContext.Documents.FirstOrDefaultAsync(x => x.DocumentId == id);
            return document;
        }

        public async Task SaveDocument(Document document)
        {
            await _dbContext.Documents.AddAsync(document);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDocument(Document document)
        {
            _dbContext.Update(document);
            await _dbContext.SaveChangesAsync();
        }
    }
}
