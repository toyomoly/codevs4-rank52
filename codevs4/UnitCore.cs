using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codevs4 {

    class UnitCore {

        public int id;
        public int x;
        public int y;
        public int hp;
        public int type;

        public UnitStatus status;
        public string order;

        public UnitCore(string input) {
            string[] a = input.Split(' ');
            this.id = int.Parse(a[0]);
            this.y = int.Parse(a[1]);
            this.x = int.Parse(a[2]);
            this.hp = int.Parse(a[3]);
            this.type = int.Parse(a[4]);
            this.status = UnitStatus.UNKNOWN;
            this.order = "";
        }

        public UnitCore(int x, int y, int type) {
            this.x = x;
            this.y = y;
            this.type = type;

            this.id = -1;
            this.hp = -1;
            this.status = UnitStatus.UNKNOWN;
            this.order = "";
        }

        public void setId(int id) {
            this.id = id;
        }

        public void initTurn() {
            this.status = UnitStatus.UNKNOWN;
            this.order = "";
        }

        public void update(int x, int y, int hp) {
            this.x = x;
            this.y = y;
            this.hp = hp;
            this.status = UnitStatus.LIFE;
        }

        public string getOrderString() {
            if (this.order == "") {
                return "";
            } else {
                return string.Format("{0} {1}", this.id, this.order);
            }
        }
    }

    class UnitType {
        public const int WORKER = 0;
        public const int KNIGHT = 1;
        public const int FIGHTER = 2;
        public const int ASSASSIN = 3;
        public const int CASTLE = 4;
        public const int WORKER_FACTORY = 5;
        public const int WARRIOR_FACTORY = 6;
    }

    class UnitCost {
        public const int WORKER = 40;
        public const int KNIGHT = 20;
        public const int FIGHTER = 40;
        public const int ASSASSIN = 60;

        public const int WORKER_FACTORY = 100;
        public const int WARRIOR_FACTORY = 500;
    }

    enum UnitStatus {
        UNKNOWN,
        DEATH,
        LIFE
    }
}


//switch (u.type) {
//    case UnitType.WORKER:
//        break;
//    case UnitType.KNIGHT:
//    case UnitType.FIGHTER:
//    case UnitType.ASSASSIN:
//        break;
//    case UnitType.CASTLE:
//        break;
//    case UnitType.WORKER_FACTORY:
//        break;
//    case UnitType.WARRIOR_FACTORY:
//        break;
//}
