namespace HNG.Web.Common.Helpers
{
    public static class DocumentHelpers
    {
        private static readonly Dictionary<string, byte[]> MagicNumbers = new Dictionary<string, byte[]>
        {
            { ".pdf", new byte[] { 0x25, 0x50, 0x44, 0x46 } }, // PDF
            { ".docx", new byte[] { 0x50, 0x4B, 0x03, 0x04 } }, // DOCX (This is a general ZIP signature)
            { ".xlsx", new byte[] { 0x50, 0x4B, 0x03, 0x04 } }, // XLSX (This is a general ZIP signature)
            { ".csv", new byte[] { 0x2C } }, // CSV (First byte could be any ASCII character, but a comma is common)
            { ".jpg", new byte[] { 0xFF, 0xD8 } }, // JPG
            { ".jpeg", new byte[] { 0xFF, 0xD8 } }, // JPEG
            { ".png", new byte[] { 0x89, 0x50, 0x4E, 0x47 } }, // PNG
            { ".gif", new byte[] { 0x47, 0x49, 0x46, 0x38 } }, // GIF
            { ".txt", new byte[] { 0xEF, 0xBB, 0xBF } } // TXT (This is the UTF-8 BOM; plain text files may not have a consistent magic number)
        };


        private static bool ValidateFileType(Stream fileStream, string fileExtension)
        {
            if (MagicNumbers.TryGetValue(fileExtension.ToLower(), out var magicNumber))
            {
                byte[] buffer = new byte[magicNumber.Length];
                fileStream.Read(buffer, 0, magicNumber.Length);
                return magicNumber.SequenceEqual(buffer);
            }
            return false;
        }

        public static void ValidateDocumentUploadRequirements(string? fileExtension, string? contentType, long fileSize, string? fileName)
        {
            var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx", ".csv", ".jpg", ".jpeg", ".png", ".gif", ".txt" };
            var allowedContentTypes = new[] { "application/pdf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "text/csv", "image/jpeg", "image/png", "text/plain" };
            const int fileSizeInMB = 5;
            var maxFileSize = fileSizeInMB * 1024 * 1024; // 5 MB
            const int maxFileNameLength = 255; // Set a filename length limit

            // Validate file extension
            if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension.ToLower()))
            {
                throw new Exception("Unsupported file extension.");
            }

            // Validate content type (though this can be spoofed)
            if (string.IsNullOrEmpty(contentType) || !allowedContentTypes.Contains(contentType))
            {
                throw new Exception("Unsupported content type.");
            }

            // Validate file size
            if (fileSize > maxFileSize)
            {
                throw new Exception($"File size exceeds the limit ({fileSizeInMB} MB).");
            }

            // Validate file name length and characters
            if (string.IsNullOrEmpty(fileName) || fileName.Length > maxFileNameLength || fileName.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
            {
                throw new Exception("Invalid file name.");
            }
        }

        public static void ValidateDocumentContents(string fileExtension, Stream fileStream)
        {
            // Validate file type using magic numbers
            if (!ValidateFileType(fileStream, fileExtension))
            {
                throw new Exception("Invalid file type.");
            }
        }
    }
}
