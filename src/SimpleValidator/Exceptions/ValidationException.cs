namespace SimpleValidator.Exceptions
{
    public class ValidationException : Exception
    {
        public Validator Validator = new();

        private ValidationException()
        {
        }

        public ValidationException(Validator validator) : base()
        {
            Validator = validator;
        }
    }
}
