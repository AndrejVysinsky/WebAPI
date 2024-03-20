using FluentValidation.TestHelper;
using Moq;
using WebAPI.Handlers.Document.SaveDocument;
using WebAPI.Repositories;

namespace WebAPITests.Document.SaveDocument
{
    public class SaveDocumentValidatorTest
    {
        [Fact]
        public void SaveDocument_WhenEmptyId_ShouldReturnValidationError()
        {
            //Arrange
            var arrangments = new Arrangments(0);

            //Act
            var result = arrangments.Validator.TestValidate(arrangments.Request);

            //Assert
            result.ShouldHaveValidationErrorFor("DocumentId");
        }

        [Fact]
        public void SaveDocument_WhenExistingId_ShouldReturnValidationError()
        {
            //Arrange
            var arrangments = new Arrangments(1, idExists: true);

            //Act
            var result = arrangments.Validator.TestValidate(arrangments.Request);

            //Assert
            result.ShouldHaveValidationErrorFor("DocumentId");
        }

        [Fact]
        public void SaveDocument_WhenEmptyTags_ShouldReturnValidationError()
        {
            //Arrange
            var arrangments = new Arrangments(1);

            //Act
            var result = arrangments.Validator.TestValidate(arrangments.Request);

            //Assert
            result.ShouldHaveValidationErrorFor("Tags");
        }

        [Fact]
        public void SaveDocument_WhenEmptyData_ShouldReturnValidationError()
        {
            //Arrange
            var arrangments = new Arrangments(1, tags: ["tag1", "tag2"]);

            //Act
            var result = arrangments.Validator.TestValidate(arrangments.Request);

            //Assert
            result.ShouldHaveValidationErrorFor("Data");
        }

        [Fact]
        public void SaveDocument_WhenValid_ShouldNotReturnValidationError()
        {
            //Arrange
            var arrangments = new Arrangments(1, tags: ["tag1", "tag2"], data: "document data");

            //Act
            var result = arrangments.Validator.TestValidate(arrangments.Request);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        private class Arrangments
        {
            public SaveDocumentRequest Request { get; private set; }
            public Mock<IDocumentRepository> DocumentRepoMock { get; private set; }
            public SaveDocumentRequestValidator Validator { get; private set; }

            public Arrangments(int documentId, List<string>? tags = null, string data = "", bool idExists = false)
            {
                Request = new SaveDocumentRequest()
                {
                    DocumentId = documentId,
                    Tags = tags ?? [],
                    Data = data
                };

                DocumentRepoMock = new Mock<IDocumentRepository>();
                DocumentRepoMock.Setup(x => x.GetDocument(It.IsAny<int>()))
                    .ReturnsAsync(idExists ? new WebAPI.Data.Document() : null);

                Validator = new SaveDocumentRequestValidator(DocumentRepoMock.Object);
            }
        }
    }
}
