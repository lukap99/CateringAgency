using CateringAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CateringAgency.Models.EntityFramework
{
    public class OrderRepository
    {
        private readonly CateringAgencyEntities cateringEntities;
        private readonly FoodRepository foodRepo;
        private readonly ComboMenuRepository comboRepo;
        private readonly UserRepository userRepo;

        public OrderRepository()
        {
            cateringEntities = new CateringAgencyEntities();
            foodRepo = new FoodRepository();
            comboRepo = new ComboMenuRepository();
            userRepo = new UserRepository();
        }

        // Double check later
        public OrderBo OrderMap(order orderModel)
        {
            OrderBo orderBo = new OrderBo
            {
                OrderId = orderModel.id,

                User = new UserBo
                {
                    UserId = orderModel.user.id,
                    Email = orderModel.user.email,
                    Username = orderModel.user.username,
                    FirstName = orderModel.user.firstname,
                    LastName = orderModel.user.lastname,
                    Points = orderModel.user.points,
                    Role = new RoleBo
                    {
                        RoleId = orderModel.user.role.id,
                        RoleName = orderModel.user.role.role_name
                    }
                },

                DateCreated = orderModel.date_created,
                DateCompleted = orderModel.date_completed,
                DeliveryLocation = orderModel.delivery_location,

                BasePrice = (float)orderModel.price,
                BuyingPrice = (float)orderModel.price_with_discount,

                DeliveryMethod = new DeliveryMethodBo
                {
                    DeliveryMethodId = orderModel.delivery_method.id,
                    Name = orderModel.delivery_method.name,
                    Price = (float)orderModel.delivery_method.price
                },

                PaymentMethod = new PaymentMethodBo
                {
                    PaymentMethodId = orderModel.payment_method.id,
                    Name = orderModel.payment_method.type
                },

                OrderDiscount = new OrderDiscountBo
                {
                    OrderDiscountId = orderModel.order_discount.id,
                    Name = orderModel.order_discount.name,
                    DiscountAmount = (float)orderModel.order_discount.discount_percent,
                    PointCost = orderModel.order_discount.point_cost
                },

                IsComplete = orderModel.is_complete
            };

            // Goes through all food items in the order_food_item table and adds them to the OrderItems property
            List<FoodBo> orderItemsList = new List<FoodBo>();
            foreach (order_food_item orderFoodItem in orderModel.order_food_item)
            {
                FoodBo foo = foodRepo.FoodMap(orderFoodItem.food);
                foo.FoodId = orderFoodItem.id; // sets FOOD_ID to value of COMBO_MENU_ITEM_ID !! So you can delete
                foo.BasePrice = (float)orderFoodItem.price;
                foo.SellingPrice = (float)orderFoodItem.price;
                foo.Amount = orderFoodItem.amount;
                foo.IsVisible = true;
                orderItemsList.Add(foo);
            }
            orderBo.OrderItems = orderItemsList;

            // Goes through all the combo menus in the order_combo_item table and adds them to the OrderComboItems property
            // Notably, does not retrieve the food items from those combo menus, just the prices
            List<ComboMenuBo> orderComboItemsList = new List<ComboMenuBo>();
            foreach (order_combo_item comboMenuItem in orderModel.order_combo_item)
            {
                ComboMenuBo foo = new ComboMenuBo();
                foo.Name = comboMenuItem.combo_menu.name;
                foo.BasePrice = (float)comboMenuItem.price;
                foo.SellingPrice = (float)comboMenuItem.price;
                foo.IsVisible = true;
            }
            orderBo.OrderComboItems = orderComboItemsList;
            return orderBo;
        }

        public IEnumerable<OrderBo> GetAllOrders()
        {
            List<OrderBo> ordersList = new List<OrderBo>();

            foreach (order orderItem in cateringEntities.orders)
            {
                ordersList.Add(OrderMap(orderItem));
            }

            return ordersList;
        }

        public IEnumerable<OrderBo> GetUserOrders(int userId)
        {
            List<OrderBo> ordersList = new List<OrderBo>();

            foreach (order orderItem in cateringEntities.orders.Where(t=>t.user_id == userId))
            {
                ordersList.Add(OrderMap(orderItem));
            }

            return ordersList;
        }

        //Returns default order if none exists
        public OrderBo GetOrder(int orderId)
        {
            if (cateringEntities.orders.Any(t => t.id == orderId))
            {
                order orderModel = cateringEntities.orders.First(t => t.id == orderId);
                OrderBo orderBo = OrderMap(orderModel);
                return orderBo;
            }
            else
            {
                return new OrderBo();
            }
        }

        public bool IsOrderComplete(int orderId)
        {
            if (cateringEntities.orders.Any(t => t.id == orderId))
            {
                return cateringEntities.orders.FirstOrDefault(t => t.id == orderId).is_complete;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<FoodBo> GetOrderItems(int orderId)
        {
            List<FoodBo> foodList = new List<FoodBo>();

            foreach (order_food_item foodItem in cateringEntities.order_food_item)
            {
                FoodBo foo = new FoodBo();
                foo.Name = foodItem.food.name;
                foo.Amount = foodItem.amount;
                foo.BasePrice = (float)foodItem.price;
                foo.SellingPrice = (float)foodItem.price;

                foodList.Add(foo);
            }

            return foodList;
        }

        public void CompleteOrder(int orderId)
        {
            order orderModel = cateringEntities.orders.FirstOrDefault(t => t.id == orderId);

            if (orderModel.is_complete == false)
            {
                orderModel.is_complete = true;
                orderModel.date_completed = DateTime.Now;
            }

            try
            {
                cateringEntities.SaveChanges();
                Console.WriteLine("Order " + orderId + " completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in OrderRepository.CompleteOrder(int orderId): " + ex.Message);
            }
        }

        public void CancelOrder(int orderId)
        {
            order orderModel = cateringEntities.orders.FirstOrDefault(t => t.id == orderId);

            if (orderModel.is_complete == false)
            {
                try
                {
                    cateringEntities.orders.Remove(orderModel);
                    cateringEntities.SaveChanges();

                    Console.WriteLine("Order succesfully cancelled.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in OrderRepository.CompleteOrder(int orderId): " + ex.Message);
                }
            }
        }

        public void CreateOrder(CartBo cartBo)
        {
            cartBo.CalculatePrice();
            cartBo.CalculatePoints();
            //REMINDER: update users point balance!!

            order orderModel = new order
            {
                user_id = cartBo.User.UserId,
                date_created = DateTime.Now,
                delivery_location = cartBo.DeliveryLocation,
                price = cartBo.BasePrice,
                price_with_discount = cartBo.BuyingPrice,
                is_complete = false,
                payment_method_id = cartBo.DeliveryMethod.DeliveryMethodId,
                discount_id = cartBo.OrderDiscount.OrderDiscountId,
                delivery_id = cartBo.DeliveryMethod.DeliveryMethodId
            };

            try
            {
                // Adding the base field to the order
                cateringEntities.orders.Add(orderModel);
                cateringEntities.SaveChanges();

                int orderId = orderModel.id;

                // Adding to order_food_item
                foreach (FoodBo foodItem in cartBo.CartItems)
                {
                    order_food_item foo = new order_food_item();
                    foo.order_id = orderId;
                    foo.food_id = foodItem.FoodId;
                    foo.price = foodItem.SellingPrice;
                    foo.amount = foodItem.Amount;

                    cateringEntities.order_food_item.Add(foo);
                    cateringEntities.SaveChanges();
                    Console.WriteLine("Succesfully added food item: " + foodItem.Name + " x " + foodItem.SellingPrice + " to order");
                }

                // Adding to order_combo_item
                foreach (ComboMenuBo comboItem in cartBo.CartComboItems)
                {
                    order_combo_item foo = new order_combo_item();
                    foo.order_id = orderId;
                    foo.combo_menu_id = comboItem.ComboMenuId;
                    foo.price = comboItem.SellingPrice;
                    foo.amount = comboItem.Amount;

                    cateringEntities.order_combo_item.Add(foo);
                    cateringEntities.SaveChanges();
                    Console.WriteLine("Succesfully added combo item: " + comboItem.Name + " x " + comboItem.SellingPrice + " to order");
                }

                //Changing point balance
                userRepo.UpdatePointBalance(cartBo.User.UserId, cartBo.PointsDifference);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in OrderRepository.CreateOrder(CartBo cartBo): " + ex.Message);
            }
        }

        /*
        public IEnumerable<SelectListItem> GetDiscounts(int lesserThan)
        {
            List<SelectListItem> discountOptionList = new List<SelectListItem>();
            discountOptionList.Add(new SelectListItem { Value = "1", Text = "No discount (0%)", Selected = true });
            foreach (order_discount item in cateringEntities.order_discount.Where(t=>t.point_cost <= lesserThan && t.id > 1)
            {
                discountOptionList.Add(new SelectListItem { Value = item.id.ToString(), Text = item.name});
            }

            return discountOptionList;
        }*/

        public IEnumerable<OrderDiscountBo> GetDiscounts()
        {
            List<OrderDiscountBo> discountList = new List<OrderDiscountBo>();
            foreach (order_discount item in cateringEntities.order_discount)
            {
                discountList.Add(new OrderDiscountBo
                {
                    OrderDiscountId = item.id,
                    Name = item.name,
                    DiscountAmount = (float)item.discount_percent,
                    PointCost = item.point_cost
                });
            }
            return discountList;
        }

        public PaymentMethodBo GetPaymentMethod(int id)
        {
            payment_method paymentMethodModel = cateringEntities.payment_method.First(t => t.id == id);
            PaymentMethodBo paymentMethodBo = new PaymentMethodBo
            {
                PaymentMethodId = id,
                Name = paymentMethodModel.type
            };

            return paymentMethodBo;
        }

        public DeliveryMethodBo GetDeliveryMethod(int id)
        {
            delivery_method deliveryMethodModel = cateringEntities.delivery_method.First(t => t.id == id);
            DeliveryMethodBo deliveryMethodBo = new DeliveryMethodBo
            {
                DeliveryMethodId = id,
                Name = deliveryMethodModel.name,
                Price = (float)deliveryMethodModel.price
            };

            return deliveryMethodBo;
        }

        public OrderDiscountBo GetOrderDiscount(int id)
        {
            order_discount OrderDiscountModel = cateringEntities.order_discount.First(t => t.id == id);
            OrderDiscountBo orderDiscountBo = new OrderDiscountBo
            {
                OrderDiscountId = id,
                Name = OrderDiscountModel.name,
                DiscountAmount = (float)OrderDiscountModel.discount_percent,
                PointCost = OrderDiscountModel.point_cost
            };

            return orderDiscountBo;
        }
    }
}