namespace HNG.Abstractions.Exceptions
{
    public class PermissionDeniedException : Exception
    {
        public string PermissionName { get; }

        public PermissionDeniedException(string permissionName)
        {
            PermissionName = permissionName;
        }

        public PermissionDeniedException(string permissionName, string message)
            : base(message)
        {
            PermissionName = permissionName;
        }

        public PermissionDeniedException(string permissionName, string message, Exception inner)
            : base(message, inner)
        {
            PermissionName = permissionName;
        }
    }
}
