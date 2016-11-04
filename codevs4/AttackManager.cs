using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codevs4 {
    class AttackManager {

        public UnitCore enCastle;
        public int currentTurn;
        int r;

        public AttackManager() {
            this.enCastle = null;
            this.r = 0;
        }

        public void findEnemyCastle(UnitCore castle){
            this.enCastle = castle;
        }

        public List<Pos> getTarget() {
            if (this.enCastle != null) {
                return this.getCasleTarget();
            } else {
                return this.getSearchTarget();
            }
        }

        private List<Pos> getCasleTarget() {
            this.r++;
            if (this.r > 2) {
                this.r = 0;
            }

            var t = new List<Pos>();

            switch (this.r) {
                case 0:
                    var i = 20;
                    while (i > 0) {
                        t.Add(new Pos(this.enCastle.x - i, this.enCastle.y - i));
                        i--;
                    }
                    break;
                case 1:
                    t.Add(new Pos(this.enCastle.x, 60));
                    break;
                case 2:
                    t.Add(new Pos(60, this.enCastle.y));
                    break;
            }
            t.Add(new Pos(this.enCastle.x, this.enCastle.y));

            return t;
        }

        private List<Pos> getSearchTarget(){
            this.r++;
            if (this.r > 4) {
                this.r = 0;
            }

            var t = new List<Pos>();

            switch (this.r) {
                case 0:
                    // 真ん中へ行く
                    var i = 60;
                    while (i < 100) {
                        t.Add(new Pos(i, i));
                        i++;
                    }
                    t.Add(new Pos(95, 95));
                    break;
                case 1:
                    t.Add(new Pos(95, 60));
                    t.Add(new Pos(95, 99));
                    t.Add(new Pos(95, 95));
                    t.Add(new Pos(60, 95));
                    break;
                case 2:
                    t.Add(new Pos(86, 69));
                    t.Add(new Pos(86, 95));
                    var l2 = this.currentTurn % 36;
                    t.Add(new Pos(95 - l2, 60 + l2));
                    break;
                case 3:
                    t.Add(new Pos(60, 95));
                    t.Add(new Pos(99, 95));
                    t.Add(new Pos(95, 95));
                    t.Add(new Pos(95, 60));
                    break;
                case 4:
                    t.Add(new Pos(69, 86));
                    t.Add(new Pos(95, 86));
                    var l4 = this.currentTurn % 36;
                    t.Add(new Pos(95 - l4, 60 + l4));
                    break;
            }

            

            return t;
        }

    }
}
