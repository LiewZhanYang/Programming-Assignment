using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    class Order
    {
        public int id { get; set; }
        public DateTime timeReceived { get; set; }
        public DateTime? timeFulfilled { get; set; }
        public List<IceCream> iceCreamList { get; set; }

        public Order()
        {
            
        }

        public Order(int id,DateTime timeReceived)
        {
            this.id = id;
            this.timeReceived = timeReceived;
        }

        public ModifyIceCream(int)
        {

        }
    }
}
