using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp.FTP
{
    
    public class FtpResponse
    {
        public string Message { get; set; }

        public string Code { get; set; }

        public FtpResponse(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}

