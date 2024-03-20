using FluentValidation.TestHelper;
using Moq;
using WebAPI.Handlers.Document.GetDocument;
using WebAPI.Repositories;

namespace WebAPITests.Document.GetDocument
{
    public class GetDocumentValidatorTest
    {
        [Fact]
        public void GetDocument_WhenEmptyId_ShouldReturnValidationError()
        {
            //Arrange
            var arrangments = new Arrangments(0);

            //Act
            var result = arrangments.Validator.TestValidate(arrangments.Request);

            //Assert
            result.ShouldHaveValidationErrorFor("Id");
        }

        [Fact]
        public void GetDocument_WhenNotExistingId_ShouldReturnValidationError()
        {
            //Arrange
            var arrangments = new Arrangments(1, idExists: false);

            //Act
            var result = arrangments.Validator.TestValidate(arrangments.Request);

            //Assert
            result.ShouldHaveValidationErrorFor("Id");
        }

        [Fact]
        public void GetDocument_WhenValidId_ShouldSucceed()
        {
            //Arrange
            var arrangments = new Arrangments(1, idExists: true);

            //Act
            var result = arrangments.Validator.TestValidate(arrangments.Request);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
                
        private class Arrangments
        {
            public GetDocumentRequest Request { get; private set; }
            public Mock<IDocumentRepository> DocumentRepoMock { get; private set; }
            public GetDocumentRequestValidator Validator { get; private set; }

            public Arrangments(int documentId, bool idExists = false)
            {
                Request = new GetDocumentRequest()
                {
                    Id = documentId
                };

                DocumentRepoMock = new Mock<IDocumentRepository>();
                DocumentRepoMock.Setup(x => x.GetDocument(It.IsAny<int>()))
                    .ReturnsAsync(idExists ? new WebAPI.Data.Document() : null);

                Validator = new GetDocumentRequestValidator(DocumentRepoMock.Object);
            }
        }
    }
}
