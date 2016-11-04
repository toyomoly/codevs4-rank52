using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codevs4 {

    class Worker : UnitCore {

        public List<Pos> targets;
        public WorkerPattern pattern;

        public Worker(int x, int y)
            : base(x, y, UnitType.WORKER) {

        }

        public void setPattern(WorkerPattern p, List<Pos> targets) {
            this.pattern = p;
            this.targets = targets;
        }

        public void thinkOrder() {
            if (targets.Count > 0) {
                var o = Util.getMoveOrder(this.targets[0], this.x, this.y);
                if (o == "") {
                    // 移動不要
                    this.targets.RemoveAt(0);
                    // 再帰
                    this.thinkOrder();
                } else {
                    this.order = o;
                }
            } else {
                this.order = "";
                if (this.pattern == WorkerPattern.Search) {
                    this.pattern = WorkerPattern.Free;
                }
            }
        }
    }

    enum WorkerPattern {
        Free,
        WarriorFactoryMaker,
        Search,
        StayResource
    }
}
