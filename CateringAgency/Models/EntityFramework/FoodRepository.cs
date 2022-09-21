using CateringAgency.Domain;
using CateringAgency.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CateringAgency.Models.EntityFramework
{
    public class FoodRepository : IFoodRepository
    {
        private readonly CateringAgencyEntities cateringEntities;

        public FoodRepository()
        {
            cateringEntities = new CateringAgencyEntities();
        }

        public FoodBo FoodMap(food foodModel)
        {
            FoodBo foodBo = new FoodBo
            {
                FoodId = foodModel.id,
                Name = foodModel.name,
                FoodCategory = new FoodCategoryBo
                {
                    FoodCategoryId = foodModel.food_category.id,
                    Name = foodModel.food_category.name,
                    CategoryDiscount =  new FoodCategoryDiscountBo
                    {
                        DiscountAmount = (float)foodModel.food_category.discount_percent
                    }
                },
                Discount = new FoodItemDiscountBo
                {
                    DiscountAmount = (float)foodModel.discount_percent
                },
                UnitOfMeasurement = foodModel.unit_of_measurement,
                Ingredients = foodModel.ingredients,
                BasePrice = (float)foodModel.price,
                IsVisible = foodModel.is_visible
            };
            foodBo.CalculatePrice();
            return foodBo;
        }

        /*private FoodItemDiscountBo AddDiscountToFoodItem(FoodBo foodBo)
        {
            //return default discount with 0 if none exist, else put the discount with the latest date on it
            //potentially refactor DB and add food_discount_id column as foreign key to food_item table
            //FoodItemDiscountBo foodDiscount = cateringEntities.food_discount.FirstOrDefault
            FoodItemDiscountBo foodDiscountBo = new FoodItemDiscountBo();

            if (cateringEntities.food_discount.Any(t => t.food_id == foodBo.FoodId))
            {
                food_discount foodDiscountModel =
                    cateringEntities.food_discount.Where(t => t.food_id == foodBo.FoodId)
                    .OrderByDescending(t => t.date_created).First();

                FoodItemDiscountBo foo_foodDiscountBo = new FoodItemDiscountBo
                {
                    FoodItemDiscountId = foodDiscountModel.id,
                    DiscountAmount = (float)foodDiscountModel.discount_percent,
                    DateCreated = foodDiscountModel.date_created,
                    ValidFrom = foodDiscountModel.valid_from,
                    ValidUntil = foodDiscountModel.valid_until
                };

                return foo_foodDiscountBo;
            }
            else
                return foodDiscountBo;
        }*/

        public IEnumerable<FoodBo> GetAll()
        {
            List<FoodBo> FoodBoList = new List<FoodBo>();
            foreach (food foodItem in cateringEntities.foods)
            {
                FoodBoList.Add(FoodMap(foodItem));
            }
            return FoodBoList;
        }

        public IEnumerable<FoodBo> GetAllActive()
        {
            List<FoodBo> FoodBoList = new List<FoodBo>();
            foreach (food foodItem in cateringEntities.foods.Where(t=>t.is_visible == true))
            {
                FoodBoList.Add(FoodMap(foodItem));
            }
            return FoodBoList;
        }

        public IEnumerable<FoodBo> GetFoodCategoryItems(int categoryId)
        {
            List<FoodBo> FoodBoList = new List<FoodBo>();
            foreach (food foodItem in cateringEntities.foods.Where(t => t.food_category_id == categoryId))
            {
                FoodBoList.Add(FoodMap(foodItem));
            }
            return FoodBoList;
        }

        public IEnumerable<FoodBo> GetFoodCategoryItemsActive(int categoryId)
        {
            List<FoodBo> FoodBoList = new List<FoodBo>();
            foreach (food foodItem in cateringEntities.foods.Where(t => t.is_visible == true && t.food_category_id == categoryId))
            {
                FoodBoList.Add(FoodMap(foodItem));
            }
            return FoodBoList;
        }

        public FoodBo GetFoodItem(int foodId)
        {
            FoodBo foodBo = FoodMap(cateringEntities.foods.FirstOrDefault(t => t.id == foodId));

            return foodBo;
        }

        public IEnumerable<FoodCategoryBo> GetAllFoodCategories()
        {
            List<FoodCategoryBo> CategoryList = new List<FoodCategoryBo>();

            foreach (food_category foo in cateringEntities.food_category)
            {
                CategoryList.Add
                    (new FoodCategoryBo { 
                        FoodCategoryId = foo.id, 
                        Name = foo.name, 
                        CategoryDiscount = new FoodCategoryDiscountBo
                        {
                            DiscountAmount = (float)foo.discount_percent
                        }
                    });
            }
            return CategoryList;
        }

        public void Create(FoodBo foodBo)
        {
            food foodModel = new food
            {
                id = foodBo.FoodId,
                name = foodBo.Name,
                unit_of_measurement = foodBo.UnitOfMeasurement,
                food_category_id = foodBo.FoodCategory.FoodCategoryId,
                is_visible = foodBo.IsVisible,
                ingredients = foodBo.Ingredients,
                price = foodBo.BasePrice,
                discount_percent = 0
            };

            try
            {
                cateringEntities.foods.Add(foodModel);
                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void Edit(FoodBo foodBo)
        {
            food foodModel = cateringEntities.foods.First(t => t.id == foodBo.FoodId);

            foodModel.name = foodBo.Name;
            foodModel.unit_of_measurement = foodBo.UnitOfMeasurement;
            foodModel.food_category_id = foodBo.FoodCategory.FoodCategoryId;
            foodModel.is_visible = foodBo.IsVisible;
            foodModel.ingredients = foodBo.Ingredients;
            foodModel.price = foodBo.BasePrice;
            foodModel.discount_percent = foodBo.Discount.DiscountAmount;

            try
            {
                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in FoodRepository.Edit(FoodBo foodBo): " + ex.Message);
            }
        }

        public void EditDiscount(int foodId, float discountPercent)
        {
            food foodModel = cateringEntities.foods.FirstOrDefault(t => t.id == foodId);

            foodModel.discount_percent = discountPercent;

            try
            {
                cateringEntities.SaveChanges();
                Console.WriteLine("Succesfully changed " + foodModel.name + " discount to " + discountPercent + "%");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in FoodRepository.EditDiscount(int foodId, float discountPercent): " + ex.Message);
            }
        }

        public void EditCategoryDiscount(int categoryId, float discountPercent)
        {
            food_category foodCategoryModel = cateringEntities.food_category.FirstOrDefault(t => t.id == categoryId);

            foodCategoryModel.discount_percent = discountPercent;

            try
            {
                cateringEntities.SaveChanges();
                Console.WriteLine("Succesfully changed " + foodCategoryModel.name + " discount to " + discountPercent + "%");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in FoodRepository.EditCategoryDiscount(int categoryId = " + categoryId + ", float discountPercent = " + discountPercent + " ): " + ex.Message);
            }
        }

        public void Delete(FoodBo foodBo)
        {
            food foodModel = cateringEntities.foods.First(t => t.id == foodBo.FoodId);
            try
            {
                cateringEntities.foods.Remove(foodModel);
                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in FoodRepository.Delete(FoodBo foodBo): " + ex.Message);
            }
        }

        public void Delete(int foodId)
        {
            food foodModel = cateringEntities.foods.First(t => t.id == foodId);
            try
            {
                cateringEntities.foods.Remove(foodModel);
                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in FoodRepository.Delete(int foodId): " + ex.Message);
            }
        }
    }
}