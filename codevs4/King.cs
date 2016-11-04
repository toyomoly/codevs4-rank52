using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codevs4 {

    class King {

        int currentStage;
        int currentTurn;
        int currentResource;

        // Unitの保持は、２種類
        // 全ユニットを保持
        List<UnitCore> myUnits;
        // 各種類別に保持
        List<Worker> myWorkers;
        List<Knight> myKnights;
        List<UnitCore> myFighters;
        List<UnitCore> myAssassins;
        UnitCore myCastle;
        List<WorkerFactory> myWorkerFactorys;
        List<UnitCore> myWarriorFactorys;

        ResourceManager resourceManager;
        AttackManager atackManager;

        public King() {
        }

        public void initStage(int currentStage) {
            this.currentStage = currentStage;

            this.myUnits = new List<UnitCore>();
            this.myWorkers = new List<Worker>();
            this.myKnights = new List<Knight>();
            this.myFighters = new List<UnitCore>();
            this.myAssassins = new List<UnitCore>();

            this.myCastle = new UnitCore(-1, -1, UnitType.CASTLE);
            this.myUnits.Add(this.myCastle);

            this.myWorkerFactorys = new List<WorkerFactory>();
            this.myWarriorFactorys = new List<UnitCore>();

            this.resourceManager = new ResourceManager();
            this.atackManager = new AttackManager();
        }

        public void initTurn(int currentTurn, int currentResource) {
            this.currentTurn = currentTurn;
            this.currentResource = currentResource;

            // order初期化
            foreach (var u in this.myUnits) {
                u.initTurn();
            }

            this.resourceManager.initTurn();
            this.atackManager.currentTurn = this.currentTurn;
        }

        public void inputMyUnit(UnitCore u) {
            if (this.currentTurn == 0) {
                if (u.type == UnitType.CASTLE) {
                    this.myCastle.setId(u.id);
                    this.myCastle.update(u.x, u.y, u.hp);
                } else {
                    var w = new Worker(u.x, u.y);
                    w.setId(u.id);
                    w.update(u.x, u.y, u.hp);
                    this.myWorkers.Add(w);
                    this.myUnits.Add(w);
                }
            } else {
                var t = this.myUnits.Find(x => x.id == u.id);
                if (t != null) {
                    // id一致（既存Unit）
                    t.update(u.x, u.y, u.hp);
                } else {
                    // id不一致（新規Unit）
                    var s = this.myUnits.Find(x => (x.id == -1) && (x.x == u.x) && (x.y == u.y) && (x.type == u.type));
                    if (s != null) {
                        s.setId(u.id);
                        s.update(u.x, u.y, u.hp);
                    } else {
                        Console.Error.WriteLine("unknown unit. id:{0}, type:{1}", u.id, u.type);
                    }
                }
            }
        }

        public void inputEnUnit(UnitCore u) {
            if ((u.type == UnitType.CASTLE) && (this.atackManager.enCastle == null)) {
                Console.Error.WriteLine("enemy castle found");
                this.atackManager.findEnemyCastle(u);
                foreach (var k in this.myKnights) {
                    k.notifyCastle(u);
                }
            }
        }

        public void inputResource(Resource r) {
            this.resourceManager.findResource(r);
        }

        /* 
         * Workerと資源の思考ロジック (turn 0 ~ 40) 
         */
        private void think_T40() {
            if (this.currentTurn == 0) {
                this.resourceManager.initByTurn0(this.myCastle.x, this.myCastle.y);
                this.myWorkers[0].setPattern(WorkerPattern.WarriorFactoryMaker, this.resourceManager.getResourceSearchTarget());
                this.myWorkers[1].setPattern(WorkerPattern.Search, this.resourceManager.getResourceSearchTarget());
                this.myWorkers[2].setPattern(WorkerPattern.Search, this.resourceManager.getResourceSearchTarget());
                this.myWorkers[3].setPattern(WorkerPattern.Search, this.resourceManager.getResourceSearchTarget());
                this.myWorkers[4].setPattern(WorkerPattern.Search, this.resourceManager.getResourceSearchTarget());
            }

            // 資源の確認その１
            var targetResource = this.resourceManager.getTargetByTurn40();
            if ((targetResource != null) && (this.currentResource >= UnitCost.WORKER)) {
                this.makeWorker(this.myCastle, targetResource);
                this.currentResource -= UnitCost.WORKER;
            }

            // 資源の追加探索
            this.addResourceSearchWorker();

            foreach (var w in this.myWorkers) {
                w.thinkOrder();
            }
        }

        /* 
         * Workerと資源の思考ロジック (turn 41 ~ 60) 
         */
        private void think_T60() {
            // 資源の追加探索
            this.addResourceSearchWorker();

            foreach (var w in this.myWorkers) {
                w.thinkOrder();
            }

            // 新しい資源を見つけたら村を作る
            if ((this.myWorkerFactorys.Count < 1) && (this.currentResource >= UnitCost.WORKER_FACTORY)) {
                var targetResource = this.resourceManager.getTargetByTurn60();
                if (targetResource != null) {
                    var maker = this.myWorkers.Find(w => Util.getPosDistance(w.x, w.y, targetResource.x, targetResource.y) <= 4);
                    // 村の生産を指示
                    maker.order = "5";
                    this.currentResource -= UnitCost.WORKER_FACTORY;
                    var newWrkFac = new WorkerFactory(maker.x, maker.y, targetResource);
                    this.myWorkerFactorys.Add(newWrkFac);
                    this.myUnits.Add(newWrkFac);
                }
            }

            // 村を作ったら5体までWorkerを作る
            var fac = this.myWorkerFactorys.Find(w => w.nearResource.workers.Count < 5);
            if ((fac != null) && (this.currentResource >= UnitCost.WORKER)) {
                // Workerの生産を指示
                this.makeWorker(fac, fac.nearResource);
                this.currentResource -= UnitCost.WORKER;
            }
        }

        /* 
         * Workerと資源の思考ロジック (turn 61 ~ 100) 
         */
        private void think_T100() {
            // 死亡Unitの確認
            this.checkDeath();

            // 村を作ったら5体までWorkerを作る
            var fac = this.myWorkerFactorys.Find(w => w.nearResource.workers.Count < 5);
            if ((fac != null) && (this.currentResource >= UnitCost.WORKER)) {
                // Workerの生産を指示
                this.makeWorker(fac, fac.nearResource);
                this.currentResource -= UnitCost.WORKER;
            }

            // 資源の確認その２
            if (this.resourceManager.isSearching) {
                if ((this.currentTurn == 100) || (this.resourceManager.checkSearchCompleteByTurn100())) {
                    this.resourceManager.isSearching = false;
                    // 探索を中断
                    this.myWorkers.FindAll(w => w.pattern == WorkerPattern.Search).ForEach(w => w.pattern = WorkerPattern.Free);
                }
            }

            foreach (var w in this.myWorkers) {
                w.thinkOrder();
                if (w.pattern == WorkerPattern.Free) {
                    // 最寄りの空き資源へ向かう
                    var targetResource = this.resourceManager.getTargetByFreeWorker(w);
                    if (targetResource != null) {
                        w.setPattern(WorkerPattern.StayResource, targetResource.getTargetForWorker());
                    }
                }
            }
        }

        /* 
         * Workerと資源の思考ロジック (turn 101 ~ 300) 
         */
        private void think_T300() {
            // 死亡Unitの確認
            this.checkDeath();

            foreach (var w in this.myWorkers) {
                w.thinkOrder();
            }
        }

        /* 
         * Workerと資源の思考ロジック (turn 301 ~ 1000) 
         */
        private void think_T1000() {
            // 死亡Unitの確認
            this.checkDeath();
            
            // 悪あがき
        }

        /* 
         * 思考ロジック (攻撃担当) 
         */
        private void think_A() {

            foreach (var k in this.myKnights) {
                k.thinkOrder();
            }

            // 拠点の管理
            if (this.myWarriorFactorys.Count < 1 && this.currentResource >= UnitCost.WARRIOR_FACTORY) {
                var maker = this.myWorkers.Find(x => x.pattern == WorkerPattern.WarriorFactoryMaker);
                if (maker == null) {
                    // ピンチ！拠点生産担当が死亡

                } else if (maker.order == "") {
                    // 拠点生産を指示
                    maker.order = "6";
                    maker.pattern = WorkerPattern.Free;
                    this.currentResource -= UnitCost.WARRIOR_FACTORY;
                    var newWarFac = new UnitCore(maker.x, maker.y, UnitType.WARRIOR_FACTORY);
                    this.myWarriorFactorys.Add(newWarFac);
                    this.myUnits.Add(newWarFac);
                }
            }

            // ナイトを生産してみる
            foreach (var warFac in this.myWarriorFactorys) {
                if (this.currentResource >= UnitCost.KNIGHT) {
                    warFac.order = "1";
                    this.currentResource -= UnitCost.KNIGHT;
                    var newKni = new Knight(warFac.x, warFac.y);
                    newKni.targets = this.atackManager.getTarget();
                    this.myKnights.Add(newKni);
                    this.myUnits.Add(newKni);
                }
            }
        }

        private void addResourceSearchWorker() {
            if (this.resourceManager.isResourceSearchTarget && (this.currentResource >= UnitCost.WORKER)) {
                myCastle.order = "0";
                var w = new Worker(this.myCastle.x, this.myCastle.y);
                w.setPattern(WorkerPattern.Search, this.resourceManager.getResourceSearchTarget());
                this.myWorkers.Add(w);
                this.myUnits.Add(w);
            }
        }

        private void makeWorker(UnitCore maker, Resource target) {
            maker.order = "0";
            var w = new Worker(maker.x, maker.y);
            w.setPattern(WorkerPattern.StayResource, target.getTargetForWorker());
            this.myWorkers.Add(w);
            this.myUnits.Add(w);
            this.resourceManager.setWorkerForResource(w, target);
        }

        private void checkDeath() {
            var deathUnits = this.myUnits.FindAll(u => u.status != UnitStatus.LIFE);
            deathUnits.ForEach(u => {
                u.status = UnitStatus.DEATH;
                switch (u.type) {
                    case UnitType.WORKER:
                        break;
                    case UnitType.KNIGHT:
                        break;
                    case UnitType.WARRIOR_FACTORY:
                        break;
                }
            });
            this.myUnits.RemoveAll(u => u.status == UnitStatus.DEATH);
            this.myWorkers.RemoveAll(u => u.status == UnitStatus.DEATH);
            this.myKnights.RemoveAll(u => u.status == UnitStatus.DEATH);
            this.myFighters.RemoveAll(u => u.status == UnitStatus.DEATH);
            this.myAssassins.RemoveAll(u => u.status == UnitStatus.DEATH);
            this.myWorkerFactorys.RemoveAll(u => u.status == UnitStatus.DEATH);
            this.myWarriorFactorys.RemoveAll(u => u.status == UnitStatus.DEATH);
        }

        public List<string> getOrders() {

            // Workerと資源の思考
            if (this.currentTurn > 300) {
                think_T1000();
            } else if (this.currentTurn > 100) {
                think_T300();
            } else if (this.currentTurn > 60) {
                think_T100();
            } else if (this.currentTurn > 40) {
                think_T60();
            } else {
                think_T40();
            }

            // 攻撃担当の思考
            if (this.currentTurn > 60) {
                think_A();
            }
            
            var orders = new List<string>();

            if (this.currentTurn > 400) {
                return orders;
            }

            foreach (var u in this.myUnits) {
                var o = u.getOrderString();
                if (o != "") {
                    orders.Add(o);
                }
            }

            return orders;
        }
    }
}
