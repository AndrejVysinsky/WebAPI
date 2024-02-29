using FluentValidation.TestHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Handlers.Document.SaveDocument;
using WebAPI.Handlers.Document.Validation;
using WebAPI.Domain;
using WebAPI.Repositories;

namespace WebAPITests
{
    [TestClass]
    public class SaveDocumentUseCaseTest
    {
        [TestMethod]
        public void SaveDocument_ShouldBeSaved()
        {
            //Arrange
            var arrangments = new Arrangments(documentId: 1);

            //Act
            var validationResult = arrangments.Validator.TestValidate(arrangments.Document, x => x.IncludeRuleSets("OnSave"));
            var response = arrangments.Handler.Handle(arrangments.Request, CancellationToken.None).Result;

            //Assert
            Assert.IsTrue(validationResult.IsValid);
            Assert.IsTrue(response.IsSuccess);
            arrangments.DocumentRepoMock.Verify(repo => repo.SaveDocument(arrangments.Document), Times.Once);
        }

        [TestMethod]
        public void SaveDocument_ShouldFailOn_InvalidId()
        {
            //Arrange
            var arrangments = new Arrangments(documentId: 0);

            //Act
            var validationResult = arrangments.Validator.TestValidate(arrangments.Document, x => x.IncludeRuleSets("OnSave"));
            var response = arrangments.Handler.Handle(arrangments.Request, CancellationToken.None).Result;

            //Assert
            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(response.IsSuccess);
            arrangments.DocumentRepoMock.Verify(repo => repo.SaveDocument(arrangments.Document), Times.Once);
        }

        [TestMethod]
        public void SaveDocument_ShouldFailOn_DuplicateId()
        {
            //Arrange
            var arrangments = new Arrangments(documentId: 1, existingDocumentId: 1);

            //Act
            var validationResult = arrangments.Validator.TestValidate(arrangments.Document, x => x.IncludeRuleSets("OnSave"));
            var response = arrangments.Handler.Handle(arrangments.Request, CancellationToken.None).Result;

            //Assert
            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(response.IsSuccess);
            arrangments.DocumentRepoMock.Verify(repo => repo.SaveDocument(arrangments.Document), Times.Once);
        }

        [TestMethod]
        public void UpdateDocument_ShouldBeUpdated()
        {
            //Arrange
            var arrangments = new Arrangments(documentId: 1, existingDocumentId: 1, operationType: OperationType.Update);

            //Act
            var validationResult = arrangments.Validator.TestValidate(arrangments.Document, x => x.IncludeRuleSets("OnUpdate"));
            var response = arrangments.Handler.Handle(arrangments.Request, CancellationToken.None).Result;

            //Assert
            Assert.IsTrue(validationResult.IsValid);
            Assert.IsTrue(response.IsSuccess);
            arrangments.DocumentRepoMock.Verify(repo => repo.UpdateDocument(arrangments.Document), Times.Once);
        }

        private class Arrangments
        {
            public SaveDocumentRequest Request { get; private set; }
            public Mock<IDocumentRepository> DocumentRepoMock { get; private set; }
            public DocumentValidator Validator { get; private set; }
            public SaveDocumentHandler Handler { get; private set; }

            public Document Document => Request.Document.Object;

            public Arrangments(int documentId, int existingDocumentId = 0, OperationType operationType = OperationType.Add)
            {
                Request = new SaveDocumentRequest()
                {
                    Document = new Editable<Document>()
                    {
                        OperationType = operationType,
                        Object = new Document { Id = documentId, Tags = ["tag1", "tag2"], Data = "Some data" }
                    }
                };

                DocumentRepoMock = new Mock<IDocumentRepository>();
                if (existingDocumentId > 0)
                {
                    DocumentRepoMock.Setup(x => x.GetDocument(It.IsAny<int>())).ReturnsAsync(new Document());
                }

                Validator = new DocumentValidator(DocumentRepoMock.Object);
                Handler = new SaveDocumentHandler(DocumentRepoMock.Object);
            }
        }
    }
}
