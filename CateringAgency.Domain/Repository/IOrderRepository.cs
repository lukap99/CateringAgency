using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<OrderBo> GetAllOrders();
        IEnumerable<OrderBo> GetUserOrders(int userId);
        OrderBo GetOrder(int orderId);
        bool IsOrderComplete(int orderId);
        IEnumerable<FoodBo> GetOrderItems(int orderId);
        void CompleteOrder(int orderId);
        void CancelOrder(int orderId);
        void CreateOrder(CartBo cartBo);
        IEnumerable<OrderDiscountBo> GetDiscounts();
        PaymentMethodBo GetPaymentMethod(int id);
        DeliveryMethodBo GetDeliveryMethod(int id);
        OrderDiscountBo GetOrderDiscount(int id);
    }
}
