using FluentValidation.Results;

namespace WebAPI.Handlers
{
    public class Response
    {
        public List<ValidationFailure> Faults { get; set; }
        public bool IsSuccess => Faults.Count == 0;

        public Response()
        {
            Faults = [];
        }
    }
}
