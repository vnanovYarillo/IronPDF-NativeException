using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleProject.ViewModels.Home
{
    public class PdfGeneraitonResult
    {
        public byte[] FileBytes { get; set; }
        public bool IsSuccessfull { get; set; }
        public Exception Exception { get; set; }
    }
}