namespace dotnet_api_test.Persistence.Repositories.Interfaces
{
    public interface IDishRepository
    {
        void SaveChanges();
        IEnumerable<Dish> GetAllDishes();
        dynamic? GetAverageDishPrice();
        Dish GetDishById(int Id);
        void DeleteDishById(int Id);
        Dish CreateDish(Dish dish);
        Dish UpdateDish(Dish dish);
    }
}