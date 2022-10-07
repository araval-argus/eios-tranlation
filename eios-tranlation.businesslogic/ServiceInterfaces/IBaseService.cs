using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_translation.businesslogic.ServiceInterfaces
{
    /// <summary>
    /// Only a Marker Interface to be used in DI.
    /// If the service is inherited from this class it would be easy to use ID as through reflection we can fetch the service using this base interface.
    /// </summary>
    public interface IBaseService
    {
    }
}
