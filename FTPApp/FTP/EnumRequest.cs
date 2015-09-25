using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp.FTP
{
    internal enum EnumRequest
    {
        GetCurrentPath,
        GetFeature,
        GetList,
        GetSystem,
        SetType,
        PassiveMode,
        SetPassword,
        SetUser,
        SetPort,
        ChangeDirectory,
        GetFile,
        UploadFile
    }
}
