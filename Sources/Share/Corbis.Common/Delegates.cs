using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Common
{
    public delegate Tout ActionHandler<in Tin, out Tout>(Tin pmtr);

    public delegate Tout ActionHandler<in Tin1, in Tin2, out Tout>(Tin1 pmtr1, Tin2 pmtr2);

}
