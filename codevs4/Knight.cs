using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codevs4 {
    class Knight : UnitCore {

        public List<Pos> targets;
        
        public Knight(int x, int y)
            : base(x, y, UnitType.KNIGHT) {

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
            }
        }

        public void notifyCastle(UnitCore enCastle) {
            var t = new List<Pos>();
            t.Add(new Pos(enCastle.x, enCastle.y));
            this.targets = t;
        }
    }

}
