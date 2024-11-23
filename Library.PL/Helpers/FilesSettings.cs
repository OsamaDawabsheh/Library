namespace Library.PL.Helpers
{
    public class FilesSettings
    {
        public static string UploadFile(IFormFile file , string folderName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            string filePath = Path.Combine(folderPath, fileName);

            var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            return fileName;
        }
        public static void DeleteFile(string fileName , string folderName)
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", folderName, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred while trying to delete the file: {e.Message}");
            }


        }


    }
}
