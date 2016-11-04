using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codevs4 {

    class Resource {

        public int x;
        public int y;
        public bool myArea;
        public bool isNew;
        public List<Worker> workers;
        
        public Resource(string input) {
            string[] a = input.Split(' ');
            this.y = int.Parse(a[0]);
            this.x = int.Parse(a[1]);
            // (0,0) から100マス以内なら自領地の資源
            this.myArea = (this.x + this.y) < 100;
            this.isNew = true;

            this.workers = new List<Worker>();
        }

        public List<Pos> getTargetForWorker(){
            var t = new List<Pos>();
            t.Add(new Pos(this.x, this.y));
            return t;
        }

    }

    class ResourceManager {

        public bool isSearching;
        List<List<Pos>> resourceSearchTargets;
        List<Resource> resources;

        public bool isResourceSearchTarget {
            get {
                return (this.resourceSearchTargets.Count > 0);
            }
        }

        public ResourceManager() {
            this.isSearching = true;    // KingにfalseにされるまでSearch
            this.resources = new List<Resource>();
        }

        public void initTurn() {
            this.resources.ForEach(r => r.isNew = false);
        }

        public void initByTurn0(int castleX, int castleY) {
            this.resourceSearchTargets = ResourceManager.getResourceSearchTargets(castleX, castleY);
        }

        public List<Pos> getResourceSearchTarget(){
            if (this.resourceSearchTargets.Count > 0) {
                var res = this.resourceSearchTargets[0];
                this.resourceSearchTargets.RemoveAt(0);
                return res;
            } else {
                return null;
            }
        }

        private static List<List<Pos>> getResourceSearchTargets(int castleX, int castleY) {
            var res = new List<List<Pos>>();

            var max = castleX;
            if (castleX < castleY) {
                max = castleY;
            }

            // 真ん中へ行く
            var c0 = new List<Pos>();
            var c0i = max;
            while (c0i < 50) {
                c0.Add(new Pos(c0i, c0i));
                c0i++;
            }

            // →へ行く
            var r1 = new List<Pos>();
            r1.Add(new Pos(castleX, 4));
            r1.Add(new Pos(95, 4));

            var r2 = new List<Pos>();
            r2.Add(new Pos(castleX, 13));
            r2.Add(new Pos(95, 13));

            var r3 = new List<Pos>();
            r3.Add(new Pos(castleX, 22));
            r3.Add(new Pos(95, 22));

            // ↓へ行く
            var d1 = new List<Pos>();
            d1.Add(new Pos(4, castleY));
            d1.Add(new Pos(4, 95));

            var d2 = new List<Pos>();
            d2.Add(new Pos(13, castleY));
            d2.Add(new Pos(13, 95));

            var d3 = new List<Pos>();
            d3.Add(new Pos(22, castleY));
            d3.Add(new Pos(22, 95));

            // 先頭
            res.Add(c0);
            res.Add(r2);
            res.Add(r3);
            res.Add(d2);
            res.Add(d3);
            // 追加
            res.Add(r1);
            res.Add(d1);

            return res;
        }

        public void findResource(Resource newRes) {
            // 既存資源と座標をチェック
            if (this.resources.Find(res => (res.x == newRes.x) && (res.y == newRes.y)) == null) {
                // 新しい資源
                this.resources.Add(newRes);
            }
        }

        public void setWorkerForResource(Worker w, Resource r) {
            r.workers.Add(w);
        }

        public Resource getTargetByTurn40() {
            return this.resources.Find(r => r.workers.Count < 5);
        }

        public Resource getTargetByTurn60() {
            return this.resources.Find(r => r.isNew);
        }

        public Resource getTargetByFreeWorker(Worker w) {
            Resource temp = null;
            var tempAbs = 200;
            this.resources.ForEach(r => {
                var abs = Util.getPosDistance(r.x, r.y, w.x, w.y);
                if (tempAbs > abs) {
                    temp = r;
                    tempAbs = abs;
                }
            });
            return temp;
        }

        public bool checkSearchCompleteByTurn100() {
            return (this.resources.Count > 6);
        }
    }

}
