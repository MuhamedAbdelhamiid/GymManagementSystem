using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GymManagementBLL.Helper
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];
        private readonly int _maxSize = 5 * 1024 * 1024;
        private readonly IWebHostEnvironment _webHost;

        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(folderName) || file is null || file.Length == 0)
                    return null;

                if (file.Length > _maxSize)
                    return null;

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_allowedExtensions.Contains(extension))
                    return null;

                var folderPath = Path.Combine(_webHost.WebRootPath, "images", folderName);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid().ToString() + file.FileName;
                var filePath = Path.Combine(folderPath, fileName);

                var stream = new FileStream(filePath, FileMode.Create);

                file.CopyTo(stream);

                return fileName;
            }
            catch (Exception)
            {
                Console.WriteLine("File Failed To Upload");
                return null;
            }
        }

        public bool Delete(string folderName, string fileName)
        {
            try
            {
                if (
                    string.IsNullOrWhiteSpace(folderName)
                    || string.IsNullOrWhiteSpace(fileName)
                    || folderName.Length == 0
                    || fileName.Length == 0
                )
                    return false;

                var filePath = Path.Combine(
                    _webHost.ContentRootPath,
                    "images",
                    folderName,
                    fileName
                );
                if (!File.Exists(filePath))
                    return false;

                File.Delete(filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
