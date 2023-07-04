namespace dotnet_api_test.Models.Dtos
{
    public class DishesAndAveragePriceDto
    {
        public IEnumerable<ReadDishDto> Dishes { get; init; }
        public double? AveragePrice { get; init; }

        public DishesAndAveragePriceDto()
        {
            Dishes = new List<ReadDishDto>();
            AveragePrice = 0;
        }
    }
}