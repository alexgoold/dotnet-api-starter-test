using dotnet_api_test.Persistence.Repositories.Interfaces;

namespace dotnet_api_test.Persistence.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly AppDbContext _context;

        public DishRepository(AppDbContext context)
        {
            _context = context;
        }

        void IDishRepository.SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Dish> GetAllDishes()
        {
            return _context.Dishes.ToList();
        }

        public dynamic? GetAverageDishPrice()
        {
            return _context.Dishes.Average(x => x.Cost);
        }

        public Dish GetDishById(int Id)
        {
            return _context.Dishes.Find(Id);
        }

        public void DeleteDishById(int Id)
        {
            _context.Dishes.Remove(GetDishById(Id));
        }

        public Dish CreateDish(Dish dish)
        {
            _context.Dishes.Add(dish);
            return dish;
        }

        public Dish UpdateDish(Dish dish)
        {
            _context.Dishes.Update(dish);
            return dish;
        }
    }
}