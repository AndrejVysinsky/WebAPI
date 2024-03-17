using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebAPI.Data;

namespace WebAPI.Repositories
{
    public class JsonDocumentRepository : IDocumentRepository
    {
        private readonly string _filePath;
        private readonly IMemoryCache _cache;

        public JsonDocumentRepository(IMemoryCache cache, IOptions<JsonDocumentSettings> settings)
        {
            _cache = cache;

            var storagePath = settings.Value.StoragePath;
            var fileName = settings.Value.FileName;
            _filePath = Path.Combine(storagePath, $"{fileName}.json");

            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }
        }

        public async Task<Document?> GetDocument(int id)
        {
            var documents = await LoadDocuments();
            return documents?.Find(doc => doc.Id == id);
        }

        public async Task SaveDocument(Document document)
        {
            var documents = await LoadDocuments() ?? [];
            documents.Add(document);
            await SaveDocuments(documents);
        }

        public async Task UpdateDocument(Document document)
        {
            var documents = await LoadDocuments();
            var index = documents.FindIndex(doc => doc.Id == document.Id);
            if (index != -1)
            {
                documents[index] = document;
                await SaveDocuments(documents);
            }
        }

        private async Task<List<Document>> LoadDocuments()
        {
            if (_cache.TryGetValue("documents", out List<Document>? cachedDocuments))
            {
                return cachedDocuments ?? [];
            }

            if (!File.Exists(_filePath))
            {
                return [];
            }

            using var fileStream = File.OpenRead(_filePath);
            var documents = await JsonSerializer.DeserializeAsync<List<Document>>(fileStream);

            _cache.Set("documents", documents);

            return documents ?? [];
        }

        private async Task SaveDocuments(List<Document> documents)
        {
            using var fileStream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(fileStream, documents);

            //clear cache after updating file
            _cache.Remove("documents");
        }
    }
}
