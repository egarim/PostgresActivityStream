using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra.ActivityStream.Contracts.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(Stream Data,string FileName);
    }
}
