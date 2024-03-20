using FluentValidation.TestHelper;
using Moq;
using WebAPI.Handlers.Document.UpdateDocument;
using WebAPI.Repositories;

namespace WebAPITests.Document.UpdateDocument
{
    public class UpdateDocumentValidatorTest
    {
        [Fact]
        public void UpdateDocument_WhenEmptyId_ShouldReturnValidationError()
        {
            //Arrange
            var arrangments = new Arrangments(0);

            //Act
            var result = arrangments.Validator.TestValidate(arrangments.Request);

            //Assert
            result.ShouldHaveValidationErrorFor("DocumentId");
        }

        [Fact]
        public void UpdateDocument_WhenNotExistingId_ShouldReturnValidationError()
        {
            //Arrange
            var arrangments = new Arrangments(1, idExists: false);

            //Act
            var result = arrangments.Validator.TestValidate(arrangments.Request);

            //Assert
            result.ShouldHaveValidationErrorFor("DocumentId");
        }

        [Fact]
        public void UpdateDocument_WhenEmptyTags_ShouldReturnValidationError()
        {
            //Arrange
            var arrangments = new Arrangments(1, idExists: true);

            //Act
            var result = arrangments.Validator.TestValidate(arrangments.Request);

            //Assert
            result.ShouldHaveValidationErrorFor("Tags");
        }

        [Fact]
        public void UpdateDocument_WhenEmptyData_ShouldReturnValidationError()
        {
            //Arrange
            var arrangments = new Arrangments(1, tags: ["tag1", "tag2"], idExists: true);

            //Act
            var result = arrangments.Validator.TestValidate(arrangments.Request);

            //Assert
            result.ShouldHaveValidationErrorFor("Data");
        }

        [Fact]
        public void UpdateDocument_WhenValid_ShouldNotReturnValidationError()
        {
            //Arrange
            var arrangments = new Arrangments(1, tags: ["tag1", "tag2"], data: "document data", idExists: true);

            //Act
            var result = arrangments.Validator.TestValidate(arrangments.Request);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        private class Arrangments
        {
            public UpdateDocumentRequest Request { get; private set; }
            public Mock<IDocumentRepository> DocumentRepoMock { get; private set; }
            public UpdateDocumentRequestValidator Validator { get; private set; }

            public Arrangments(int documentId, List<string>? tags = null, string data = "", bool idExists = false)
            {
                Request = new UpdateDocumentRequest()
                {
                    DocumentId = documentId,
                    Tags = tags ?? [],
                    Data = data
                };

                DocumentRepoMock = new Mock<IDocumentRepository>();
                DocumentRepoMock.Setup(x => x.GetDocument(It.IsAny<int>()))
                    .ReturnsAsync(idExists ? new WebAPI.Data.Document() : null);

                Validator = new UpdateDocumentRequestValidator(DocumentRepoMock.Object);
            }
        }
    }
}
