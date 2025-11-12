using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GymManagementBLL.Helper
{
    public interface IAttachmentService
    {
        string? Upload(string folderName, IFormFile file);

        bool Delete(string folderName, string fileName);
    }
}
