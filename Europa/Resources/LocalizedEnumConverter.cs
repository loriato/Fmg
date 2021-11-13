using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Resources
{
    public class LocalizedEnumConverter : ResourcesEnumConverter
    {
        public LocalizedEnumConverter(Type type) : base(type, Europa.Resources.GlobalMessages.ResourceManager)
        {
        }
    }
}
