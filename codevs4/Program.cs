using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codevs4 {

    class Program {

        private King king;

        private bool input() {
            var remainingTime = int.Parse(Console.ReadLine());
            var currentStage = int.Parse(Console.ReadLine());
            var currentTurn = int.Parse(Console.ReadLine());
            if (currentTurn == 0) {
                Console.Error.WriteLine("stage: {0}", currentStage);
                this.king.initStage(currentStage);
            }
            var currentResource = int.Parse(Console.ReadLine());

            this.king.initTurn(currentTurn, currentResource);

            var myNum = int.Parse(Console.ReadLine());
            for (int i = 0; i < myNum; i++) {
                var u = new UnitCore(Console.ReadLine());
                this.king.inputMyUnit(u);
            }

            var enNum = int.Parse(Console.ReadLine());
            for (int i = 0; i < enNum; i++) {
                var u = new UnitCore(Console.ReadLine());
                this.king.inputEnUnit(u);
            }

            var resNum = int.Parse(Console.ReadLine());
            for (int i = 0; i < resNum; i++) {
                var r = new Resource(Console.ReadLine());
                this.king.inputResource(r);
            }

            string end = Console.ReadLine();
            return (end == "END");
        }

        private void output() {
            var orders = this.king.getOrders();
            Console.WriteLine(orders.Count);
            foreach (var order in orders) {
                Console.WriteLine(order);
            }
        }

        static void Main(string[] args) {
            var pg = new Program();
            pg.king = new King();

            Console.WriteLine("NalshQB");

            //var x = new List<UnitCore>();
            //x.Add(new UnitCore("0 1 2 2000 4"));
            //x.Add(new UnitCore("1 1 2 2000 0"));
            //x.Add(new UnitCore("2 1 2 2000 0"));

            //Console.WriteLine("test:{0}", x.Find(t => t.id == 1) == null);
            //Console.WriteLine("test:{0}", x.Find(t => t.id == 2) == null);
            //Console.WriteLine("test:{0}", x.Find(t => t.id == 3) == null);

            while (pg.input()) {
                pg.output();
            }
        }
    }
}
