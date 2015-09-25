using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp.FTP
{
    internal class FtpRequest
    {
        private string command;
        private string args;

        public string Command
        {
            get
            {
                return BuildStringCommand();
            }
            set
            {
                Command = value;
            }
        }

        public FtpRequest(string command, string args)
        {
            this.command = command;
            this.args = args;
        }

        private string BuildStringCommand()
        {
            string finalCommand = command;
            if (args != null)
                finalCommand = finalCommand + " " + args;

            return finalCommand;
        }
    }
}
