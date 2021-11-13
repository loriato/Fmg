using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.Shared.Models
{
    public class PhysicalDirectoryFileDTO
    {
        public string Processed { get; set; }
        public string Unprocessed { get; set; }
        public string Error { get; set; }
        public string Logs { get; set; }
    }
}
