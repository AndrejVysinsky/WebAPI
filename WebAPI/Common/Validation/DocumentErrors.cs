namespace WebAPI.Common.Validation
{
    public static class DocumentErrors
    {
        public const string IdNotFound = "Document id does not exist.";
        public const string IdAlreadyExists = "Document id already exists.";
        public const string IdCannotBeEmpty = "Document id cannot be empty.";
        public const string TagsCannotBeEmpty = "Document tags cannot be empty.";
        public const string DataCannotBeEmpty = "Document data cannot be empty.";
    }
}
