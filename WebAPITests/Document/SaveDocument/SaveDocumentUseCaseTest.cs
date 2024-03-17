using FluentValidation.TestHelper;
using Moq;
using WebAPI.Handlers.Document.SaveDocument;
using WebAPI.Domain;
using WebAPI.Repositories;
using Xunit;
using FluentAssertions;

namespace WebAPITests.Document.SaveDocument
{
    public class SaveDocumentUseCaseTest
    {
        [Fact]
        public async void SaveDocument_ShouldBeSaved()
        {
            //Arrange
            var arrangments = new Arrangments(documentId: 1);

            //Act
            var validationResult = arrangments.Validator.TestValidate(arrangments.Request);
            var response = await arrangments.Handler.Handle(arrangments.Request, CancellationToken.None);

            //Assert
            validationResult.IsValid.Should().BeTrue();
            arrangments.DocumentRepoMock.Verify(repo => repo.SaveDocument(arrangments.Document), Times.Once);
            response.IsError.Should().BeFalse();
            response.Value.Should().Be(1);
        }

        [Fact]
        public async void SaveDocument_ShouldFailOn_InvalidId()
        {
            //Arrange
            var arrangments = new Arrangments(documentId: 0);

            //Act
            var validationResult = arrangments.Validator.TestValidate(arrangments.Request);
            var response = await arrangments.Handler.Handle(arrangments.Request, CancellationToken.None);

            //Assert
            validationResult.IsValid.Should().BeFalse();
            arrangments.DocumentRepoMock.Verify(repo => repo.SaveDocument(arrangments.Document), Times.Once);
            response.IsError.Should().BeTrue();
        }

        //[Fact]
        //public void SaveDocument_ShouldFailOn_DuplicateId()
        //{
        //    //Arrange
        //    var arrangments = new Arrangments(documentId: 1, existingDocumentId: 1);

        //    //Act
        //    var validationResult = arrangments.Validator.TestValidate(arrangments.Document, x => x.IncludeRuleSets("OnSave"));
        //    var response = arrangments.Handler.Handle(arrangments.Request, CancellationToken.None).Result;

        //    //Assert
        //    Assert.IsFalse(validationResult.IsValid);
        //    Assert.IsTrue(response.IsSuccess);
        //    arrangments.DocumentRepoMock.Verify(repo => repo.SaveDocument(arrangments.Document), Times.Once);
        //}

        //[Fact]
        //public void UpdateDocument_ShouldBeUpdated()
        //{
        //    //Arrange
        //    var arrangments = new Arrangments(documentId: 1, existingDocumentId: 1, operationType: OperationType.Update);

        //    //Act
        //    var validationResult = arrangments.Validator.TestValidate(arrangments.Document, x => x.IncludeRuleSets("OnUpdate"));
        //    var response = arrangments.Handler.Handle(arrangments.Request, CancellationToken.None).Result;

        //    //Assert
        //    Assert.IsTrue(validationResult.IsValid);
        //    Assert.IsTrue(response.IsSuccess);
        //    arrangments.DocumentRepoMock.Verify(repo => repo.UpdateDocument(arrangments.Document), Times.Once);
        //}

        private class Arrangments
        {
            public SaveDocumentRequest Request { get; private set; }
            public WebAPI.Data.Document Document { get; private set; }
            public Mock<IDocumentRepository> DocumentRepoMock { get; private set; }
            public SaveDocumentRequestValidator Validator { get; private set; }
            public SaveDocumentHandler Handler { get; private set; }

            public Arrangments(int documentId, int existingDocumentId = 0, OperationType operationType = OperationType.Add)
            {
                Request = new SaveDocumentRequest()
                {
                    DocumentId = documentId,
                    Tags = ["tag1", "tag2"],
                    Data = "Some data"
                };

                Document = new WebAPI.Data.Document()
                {
                    DocumentId = documentId,
                    Tags = "tag1,tag2",
                    Data = "Some data"
                };

                DocumentRepoMock = new Mock<IDocumentRepository>();
                if (existingDocumentId > 0)
                {
                    DocumentRepoMock.Setup(x => x.GetDocument(It.IsAny<int>())).ReturnsAsync(new WebAPI.Data.Document());
                }

                Validator = new SaveDocumentRequestValidator(DocumentRepoMock.Object);
                Handler = new SaveDocumentHandler(DocumentRepoMock.Object);
            }
        }
    }
}
