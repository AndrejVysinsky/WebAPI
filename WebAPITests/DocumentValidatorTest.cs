using FluentValidation.TestHelper;
using Moq;
using WebAPI.Handlers.Document.Validation;
using WebAPI.Domain;
using WebAPI.Repositories;

namespace WebAPITests
{
    [TestClass]
    public class DocumentValidatorTest
    {
        [TestMethod]
        public void Validate_WithValidDocument_ShouldNotHaveValidationError()
        {
            //Arrange
            var document = new Document { Id = 1, Tags = ["tag1", "tag2"], Data = "Some data" };

            var documentRepositoryMock = new Mock<IDocumentRepository>();
            documentRepositoryMock.Setup(repo => repo.GetDocument(It.IsAny<int>())).ReturnsAsync(document);

            var validator = new DocumentValidator(documentRepositoryMock.Object);

            //Act
            var result = validator.TestValidate(document, x => x.IncludeRuleSets("OnGet"));

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
