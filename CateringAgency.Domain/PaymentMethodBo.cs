using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    class PaymentMethodBo
    {
        private int paymentMethodId = 1;
        private string name = "Cash";

        public PaymentMethodBo()
        {

        }

        public PaymentMethodBo(int paymentMethodId, string name)
        {
            this.paymentMethodId = paymentMethodId;
            this.name = name;
        }

        public int PaymentMethodId { get => paymentMethodId; set => paymentMethodId = value; }
        public string Name { get => name; set => name = value; }
    }
}
