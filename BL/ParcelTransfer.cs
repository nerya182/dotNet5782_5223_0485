using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class ParcelTransfer
        {
            public int Id { get; set; }
            public Priorities Priority { get; set; }
            public CustomerInShipment Sender { get; set; }
            public CustomerInShipment Target { get; set; }

            public override string ToString()
            {
                return $"Id: {Id}, priority: {Priority}, sender: {Sender}, target: {Target}";
            }
        }
    }
    
}
