using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    class DeliveryMethodBo
    {
        private int deliveryMethodId;
        private string name;
        private float price;

        public DeliveryMethodBo()
        {

        }

        public DeliveryMethodBo(int deliveryMethodId, string name, float price)
        {
            this.deliveryMethodId = deliveryMethodId;
            this.name = name;
            Price = price;
        }

        public int DeliveryMethodId { get => deliveryMethodId; set => deliveryMethodId = value; }
        public string Name { get => name; set => name = value; }
        public float Price { get => price; set => price = value < 0 ? 0 : value; }
    }
}