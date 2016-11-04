using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codevs4 {

    class Util {
        public static string getMoveOrder(Pos target, int nowX, int nowY) {
            if (nowX < target.x) {
                return "R";
            } else if (nowX > target.x) {
                return "L";
            } else if (nowY < target.y) {
                return "D";
            } else if (nowY > target.y) {
                return "U";
            } else {
                return "";
            }
        }

        public static int getPosDistance(int ax, int ay, int bx, int by) {
            return Math.Abs(ax - bx) + Math.Abs(ay - by);
        }
    }

    class Pos {
        public int x;
        public int y;

        public Pos(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

}
