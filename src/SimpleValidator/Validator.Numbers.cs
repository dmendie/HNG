using SimpleValidator.Extensions;
using System.Numerics;

namespace SimpleValidator
{
    public partial class Validator
    {
        #region " IsNotZero "

        public Validator IsNotZero<T>(T value) where T : INumber<T>
        {
            return IsNotZero("", value);
        }

        public Validator IsNotZero<T>(string name, T value) where T : INumber<T>
        {
            return IsNotZero(name, value, string.Format(MessageContainer.IsNotZeroMessage, name));
        }

        public Validator IsNotZero<T>(string name, T value, string message) where T : INumber<T>
        {
            // do the check
            if (value.IsNotZero())
            {
                return NoError();
            }
            else
            {
                return AddError(name, message);
            }
        }
        #endregion
    }
}
