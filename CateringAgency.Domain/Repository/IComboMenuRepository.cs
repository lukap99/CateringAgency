using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain.Repository
{
    public interface IComboMenuRepository
    {
        IEnumerable<ComboMenuBo> GetAllComboMenus();
        IEnumerable<ComboMenuBo> GetAllActiveComboMenus();
        ComboMenuBo GetComboMenu(int comboMenuId);
        void CreateComboMenu(ComboMenuBo comboMenuBo);
        void CreateComboMenu(String name, float price, float discountAmount, bool isVisible);
        void EditComboMenu(ComboMenuBo comboMenuBo);
        void EditComboMenu(int comboMenuId, String name, float price, float discountAmount, bool isVisible);
        void DeleteComboMenu(ComboMenuBo comboMenuBo);
        void DeleteComboMenu(int comboMenuId);
        void AddComboMenuItem(FoodBo foodBo, int comboMenuId);
        void DeleteComboMenuItem(int comboItemId);
        void EditComboMenuItemAmount(int comboItemId, int amount);
        IEnumerable<FoodBo> GetComboMenuItems(int comboMenuId);
        FoodBo GetComboMenuItem(int comboMenuItemId);
    }
}
