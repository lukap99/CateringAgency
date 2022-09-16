using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain.Repository
{
    public interface IFoodRepository
    {
        IEnumerable<FoodBo> GetAll();
        IEnumerable<FoodBo> GetAllActive();
        IEnumerable<FoodBo> GetFoodCategoryItems(int categoryId);
        IEnumerable<FoodBo> GetFoodCategoryItemsActive(int categoryId);
        FoodBo GetFoodItem(int foodId);
        IEnumerable<FoodCategoryBo> GetAllFoodCategories();
        void Create(FoodBo foodBo);
        void Edit(FoodBo foodBo);
        void EditDiscount(int foodId, float discountPercent);
        void EditCategoryDiscount(int categoryId, float discountPercent);
        void Delete(FoodBo foodBo);
        void Delete(int foodId);
    }
}
