namespace SimpleValidator.Messages
{
    public class MessageContainer
    {
        public string IsNotNullMessage { get; set; } = null!;
        public string IsNotNullOrEmptyMessage { get; set; } = null!;
        public string IsNotNullOrWhiteSpaceMessage { get; set; } = null!;
        public string IsNotZeroMessage { get; set; } = null!;
        public string IsPasswordMessage { get; set; } = null!;
        public string IsMinLengthMessage { get; set; } = null!;
        public string IsMaxLengthMessage { get; set; } = null!;
        public string IsExactLengthMessage { get; set; } = null!;
        public string IsBetweenLengthMessage { get; set; } = null!;
        public string IsMessage { get; set; } = null!;
        public string IsNotMessage { get; set; } = null!;
        public string IsEmailMessage { get; set; } = null!;
        public string IsRegexMessage { get; set; } = null!;
        public string IsMatchMessage { get; set; } = null!;
        public string IsDateMessage { get; set; } = null!;
        public string IsRuleMessage { get; set; } = null!;

        #region " Dates "

        public string IsGreaterThanMessage { get; set; } = null!;
        public string IsGreaterThanOrEqualToMessage { get; set; } = null!;
        public string IsLessThanMessage { get; set; } = null!;
        public string IsLessThanOrEqualToMessage { get; set; } = null!;
        public string IsEqualToMessage { get; set; } = null!;
        public string IsBetweenInclusiveMessage { get; set; } = null!;
        public string IsBetweenExclusiveMessage { get; set; } = null!;

        #endregion
    }
}
