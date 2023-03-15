using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevitas.AppFramework
{
    public interface IStreamObject
    {
        Guid Id { get; set; }
        string ObjectType { get; set; }
        string DisplayName { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
    }
}
