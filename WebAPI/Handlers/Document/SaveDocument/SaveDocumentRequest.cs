﻿using MediatR;

namespace WebAPI.Handlers.Document.SaveDocument
{
    public class SaveDocumentRequest : IRequest<int>
    {
        public int DocumentId { get; set; }
        public List<string> Tags { get; set; } = [];
        public string Data { get; set; } = string.Empty;
    }
}
