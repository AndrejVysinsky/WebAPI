﻿using FluentValidation;
using WebAPI.Domain;
using WebAPI.Repositories;

namespace WebAPI.Handlers.Document.SaveDocument
{
    public class SaveDocumentRequestValidator : AbstractValidator<SaveDocumentRequest>
    {
        private readonly IDocumentRepository _repository;

        public SaveDocumentRequestValidator(IDocumentRepository documentRepository)
        {
            _repository = documentRepository;

            RuleFor(x => x.DocumentId)
                .NotEmpty()
                .Must(NotExists)
                .WithMessage("Document id already exists.");

            RuleFor(x => x.Tags).NotEmpty();

            RuleFor(x => x.Data).NotEmpty();
        }

        private bool NotExists(int id)
        {
            var document = _repository.GetDocument(id).Result;
            return document == null;
        }
    }
}
