using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class JsonDocumentRepository : IDocumentRepository
    {
        private readonly string _filePath;
        private readonly IMemoryCache _cache;

        public JsonDocumentRepository(IMemoryCache cache, IConfiguration configuration)
        {
            _cache = cache;

            var storagePath = configuration["JsonStoragePath"] ?? string.Empty;
            var fileName = configuration["JsonFileName"] ?? string.Empty;
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
