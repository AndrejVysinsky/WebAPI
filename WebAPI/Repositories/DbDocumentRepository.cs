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

        public async Task<Domain.Document?> GetDocument(int id)
        {
            var document = await _dbContext.Documents.FirstOrDefaultAsync(x => x.DocumentId == id);
            if (document == null)
            {
                return null;
            }

            return new Domain.Document()
            {
                Id = document.DocumentId,
                Tags = document.Tags?.Split(",").ToList() ?? [],
                Data = document.Data ?? new object(),
            };
        }

        public async Task SaveDocument(Domain.Document document)
        {
            var documentToSave = new Data.Document()
            {
                DocumentId = document.Id,
                Tags = string.Join(",", document.Tags),
                Data = document.Data.ToString()
            };

            await _dbContext.Documents.AddAsync(documentToSave);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDocument(Domain.Document document)
        {
            var dbDocument = await _dbContext.Documents.FirstOrDefaultAsync(x => x.DocumentId == document.Id);
            dbDocument!.Tags = string.Join(",", document.Tags);
            dbDocument!.Data = document.Data.ToString();

            await _dbContext.SaveChangesAsync();
        }
    }
}
