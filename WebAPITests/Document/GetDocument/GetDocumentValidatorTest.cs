using FluentValidation.TestHelper;
using Moq;
using WebAPI.Handlers.Document.GetDocument;
using WebAPI.Repositories;
using Xunit;

namespace WebAPITests.Document.GetDocument
{
    public class GetDocumentValidatorTest
    {
        [Fact]
        public void Validate_WithValidDocument_ShouldNotHaveValidationError()
        {
            //Arrange
            var request = new GetDocumentRequest() { Id = 1 };
            var document = new WebAPI.Data.Document { Id = 1, Tags = "tag1,tag2", Data = "Some data" };

            var documentRepositoryMock = new Mock<IDocumentRepository>();
            documentRepositoryMock.Setup(repo => repo.GetDocument(It.IsAny<int>())).ReturnsAsync(document);

            var validator = new GetDocumentRequestValidator(documentRepositoryMock.Object);

            //Act
            var result = validator.TestValidate(request);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
