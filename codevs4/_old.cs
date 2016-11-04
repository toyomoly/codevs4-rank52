using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codevs4
{
    class Unit {
        public int id;
        public int x;
        public int y;
        public int hp;
        public int type;
        public int status;

        public int job;

        public int targetX;
        public int targetY;

        public string order;

        public Unit(string input) {
            string[] a = input.Split(' ');
            this.id = int.Parse(a[0]);
            this.y = int.Parse(a[1]);
            this.x = int.Parse(a[2]);
            this.hp = int.Parse(a[3]);
            this.type = int.Parse(a[4]);

            this.targetX = -1;
            this.targetY = -1;
            this.order = "";
        }

        public void setTarget(int x, int y) {
            this.targetX = x;
            this.targetY = y;
        }

        public void setOrder(string order) {
            this.order = order;
        }

        private void move() {
            if (this.x < this.targetX) {
                this.order = "R";
            } else if (this.x > this.targetX) {
                this.order = "L";
            } else if (this.y < this.targetY) {
                this.order = "D";
            } else if (this.y > this.targetY) {
                this.order = "U";
            } else {
                this.order = "";
            }
        }

        public string getOrder() {

            //Console.Error.WriteLine("{0} : {1} : {2} : {3}", this.targetX, this.targetY, this.x, this.y);
            switch (this.type) {
                case UnitType.WORKER:
                    this.move();
                    if (this.order == "") {
                        return "";
                    } else {
                        return string.Format("{0} {1}", this.id, this.order);
                    }
                default:
                    return "";
            }

        }
    }

    class MyUnit {
        public int id;
        public int x;
        public int y;
        public int hp;
        public int type;

        public int parentId;
        public UnitStatus status;

        public int job;

        // targetの保持の仕方は変更したい
        public int targetX;
        public int targetY;

        public string order;

        public MyUnit(int parentId, int type) {
            // newする時は、まだidがわからない
            this.parentId = parentId;
            this.type = type;
        }

        public void setId(int id) {
            this.id = id;
        }

        public void update(int x, int y, int hp) {
            this.x = x;
            this.y = y;
            this.hp = hp;

            this.status = UnitStatus.LIFE;
        }

        public void initTurn() {
            if (this.status != UnitStatus.DEATH) {
                this.status = UnitStatus.UNKNOWN;
            }
        }

        public void checkDEATH() {
            if (this.status == UnitStatus.UNKNOWN) {
                this.status = UnitStatus.DEATH;
            }
        }

        public MyUnit(string input) {
            string[] a = input.Split(' ');
            this.id = int.Parse(a[0]);
            this.y = int.Parse(a[1]);
            this.x = int.Parse(a[2]);
            this.hp = int.Parse(a[3]);
            this.type = int.Parse(a[4]);

            this.targetX = -1;
            this.targetY = -1;
            this.order = "";
        }

        public void setTarget(int x, int y) {
            this.targetX = x;
            this.targetY = y;
        }

        public void setOrder(string order) {
            this.order = order;
        }

        private void move() {
            if (this.x < this.targetX) {
                this.order = "R";
            } else if (this.x > this.targetX) {
                this.order = "L";
            } else if (this.y < this.targetY) {
                this.order = "D";
            } else if (this.y > this.targetY) {
                this.order = "U";
            } else {
                this.order = "";
            }
        }

        public string getOrder() {

            //Console.Error.WriteLine("{0} : {1} : {2} : {3}", this.targetX, this.targetY, this.x, this.y);
            switch (this.type) {
                case UnitType.WORKER:
                    this.move();
                    if (this.order == "") {
                        return "";
                    } else {
                        return string.Format("{0} {1}", this.id, this.order);
                    }
                default:
                    return "";
            }

        }
    }

    class UnitJob {
        public const int WORKER_SEARCH_RESOURCE = 101;
        public const int WORKER_ON_RESOURCE = 102;
        public const int KNIGHT_SEARCH_CASTLE = 201;
    }



    class _old
    {
        List<MyUnit> myUnits;
        List<MyUnit> myUnitsTemp;

        List<Unit> enUnits;
        List<Resource> resources;

        Unit myCastle;
        Unit enCastle;

        Dictionary<int, Unit> prevStatus;

        List<Target> resourceSearchTargets;

        public _old()
        {

        }

        public void initStage()
        {
            this.myUnits = new List<MyUnit>();
            this.enUnits = new List<Unit>();
            this.resources = new List<Resource>();

            this.myCastle = null;
            this.enCastle = null;

            this.resourceSearchTargets = new List<Target>();

            this.myUnitsTemp.Add(new MyUnit(-1, UnitType.CASTLE));
        }

        public void initTurn()
        {
            //this.prevStatus = new Dictionary<int, Unit>();

            //foreach (var u in this.myUnits)
            //{
            //    this.prevStatus.Add(u.id, u);
            //}

            //this.myUnits = new List<Unit>();
            //this.enUnits = new List<Unit>();

        }

        public void inputMyUnit(Unit u)
        {
            //if (!this.myUnits.Exists(o => o.id == u.id))
            //{
            //    this.myUnits.Add(u);
            //}
            //this.myUnits.Add(u);
            if (this.prevStatus.ContainsKey(u.id))
            {
                var p = this.prevStatus[u.id];
                u.setTarget(p.targetX, p.targetY);
            }
        }

        public void inputEnUnit(Unit u)
        {
            //if (!this.myUnits.Exists(o => o.id == u.id))
            //{
            //    this.myUnits.Add(u);
            //}
        }

        public void inputResource(Resource r)
        {
            if (!this.resources.Exists(o => (o.x == r.x) && (o.y == r.y)))
            {
                this.resources.Add(r);
            }
        }

        private void think()
        {
            //foreach (var u in this.myUnits)
            //{
            //    if ((u.type == UnitType.WORKER) && (u.targetX < 0))
            //    {
            //        u.setTarget(95 - this.tmp * 9, 95);
            //        tmp++;
            //    }
            //}

            foreach (var r in this.resources)
            {
                Console.Error.WriteLine("{0} : {1}", r.x, r.y);
            }
        }

        // 最初
        private void think0()
        {
            // 自分の城の位置確認と、探索目標の決定
            foreach (var u in this.myUnits)
            {
                if (u.type == UnitType.CASTLE)
                {
                    //this.myCastle = u;
                }
            }

            this.resourceSearchTargets = new List<Target>();
            this.resourceSearchTargets.Add(new Target(50, 50));
            this.resourceSearchTargets.Add(new Target(50, 50));
            this.resourceSearchTargets.Add(new Target(50, 50));
            this.resourceSearchTargets.Add(new Target(50, 50));
            this.resourceSearchTargets.Add(new Target(50, 50));
        }

        // 序盤
        private void think1()
        {
            // とりあえず資源探索を進める
        }

        // 中盤
        private void think2()
        {
            // 拠点から侵攻を開始

        }

        private void thinkFinal()
        {
            // 敵の城に集中攻撃
        }

        public List<string> getOrders()
        {
            this.think();
            
            var orders = new List<string>();

            foreach (var u in this.myUnits)
            {
                var o = u.getOrder();
                if (o != "")
                {
                    orders.Add(o);
                }
            }

            return orders;
        }
    }

    class Target {
        public int x;
        public int y;

        public Target(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }
}
