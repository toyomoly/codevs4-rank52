using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codevs4 {
    class WorkerFactory : UnitCore {

        public Resource nearResource;

        public WorkerFactory(int x, int y, Resource r)
            : base(x, y, UnitType.WORKER_FACTORY) {
            
            this.nearResource = r;
        }
    }
}
