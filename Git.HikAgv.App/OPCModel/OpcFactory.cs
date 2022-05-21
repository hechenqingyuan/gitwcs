using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.HikAgv.App.OPCModel
{
    public partial class OpcFactory
    {
        private static OpcProvider Server = null;

        public static OpcProvider Create()
        {
            if (Server != null)
            {
                return Server;
            }

            Server = new OpcProvider();

            return Server;
        }
    }
}
