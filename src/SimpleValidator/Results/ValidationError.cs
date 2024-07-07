namespace SimpleValidator.Results
{
    public class ValidationError
    {
        public ValidationError()
        {
            Field = "";
            Message = "";
        }

        public string Field { get; set; }
        public string Message { get; set; }

        #region " Helpers "

        public static ValidationError Create(string name, string message)
        {
            ValidationError error = new ValidationError()
            {
                Field = name,
                Message = message
            };

            return error;
        }

        #endregion
    }
}
