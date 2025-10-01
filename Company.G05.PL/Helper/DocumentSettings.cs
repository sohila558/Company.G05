namespace Company.G05.PL.Helper
{
    public class DocumentSettings
    {
        // 1. Upload
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Folder Location

            //string folderPath = "C:\\Users\\Mahmoud Rayan\\source\\repos\\Company.G05\\Company.G05.PL\\wwwroot\\Files\\" + folderName;

            //var folderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\" + folderName;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName);

            // 2. Get File Name And Make It Unique
            
            var fileName = $"{Guid.NewGuid()}{ file.FileName}";

            // File Path

            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }

        // 2. Delete

        public static void DeleteFile(string fileName, string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

        }

    }
}
