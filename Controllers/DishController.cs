using AutoMapper;
using dotnet_api_test.Exceptions.ExceptionResponses;
using dotnet_api_test.Models.Dtos;
using dotnet_api_test.Persistence.Repositories.Interfaces;
using dotnet_api_test.Validation;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_api_test.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly ILogger<DishController> _logger;
        private readonly IMapper _mapper;
        private readonly IDishRepository _dishRepository;

        public DishController(ILogger<DishController> logger, IMapper mapper, IDishRepository dishRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _dishRepository = dishRepository;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<DishesAndAveragePriceDto> GetDishesAndAverageDishPrice()
        {
			var dishes = _mapper.Map<ReadDishDto[]>(_dishRepository.GetAllDishes());

            if (dishes.Length == 0)
			{
				throw new NotFoundRequestExceptionResponse($"No Dishes found in database");
			}
			_logger.LogInformation($"{DateTime.Now}: Retrieved dishes and average price");

			return Ok(new DishesAndAveragePriceDto()
			{
				Dishes = _mapper.Map<ReadDishDto[]>(_dishRepository.GetAllDishes()),
				AveragePrice = _dishRepository.GetAverageDishPrice()
			});
		}

        [HttpGet]
        [Route("{id}")]
        public ActionResult<ReadDishDto> GetDishById(int id)
        {
	        _logger.LogInformation($"{DateTime.Now}: Retrieved dish with id {id}");
	        return Ok(_mapper.Map<ReadDishDto>(_dishRepository.GetDishById(id)));
        }

        [HttpPost]
        [Route("")]
        public ActionResult<ReadDishDto> CreateDish([FromBody] CreateDishDto createDishDto)
        {
            ModelValidation.ValidateCreateDishDto(createDishDto);
            if (_dishRepository.GetAllDishes().Where(d => d.Name == createDishDto.Name).ToList().Count > 0)
            {
				_logger.LogWarning($"{DateTime.Now}: Dish with name: {createDishDto.Name} already exists");
				throw new BadRequestExceptionResponse("Dish with this name already exists");
			}
            _dishRepository.CreateDish(_mapper.Map<Dish>(createDishDto));
            _logger.LogInformation($"{DateTime.Now}: Dish created");
            _dishRepository.SaveChanges();
            return Ok("Dish created successfully");
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<ReadDishDto> UpdateDishById(int id, UpdateDishDto updateDishDto)
        {
            ModelValidation.ValidateUpdateDishDto(updateDishDto);
	        var dishToUpdate = _dishRepository.GetDishById(id);
	        if (dishToUpdate == null)
	        {
		        throw new NotFoundRequestExceptionResponse($"Dish with id:{id} not found");
			}
	        if (updateDishDto.Cost > dishToUpdate.Cost * 1.2)
            {
                _logger.LogWarning($"{DateTime.Now}: Business Rule Violation: The cost of '{updateDishDto.Name}' cannot be raised by more than 20%.");
                throw new BadRequestExceptionResponse("New cost cannot be more than 20% higher than the original cost");
            }
            dishToUpdate.Cost = (double)updateDishDto.Cost;
            dishToUpdate.MadeBy = updateDishDto.MadeBy;
            dishToUpdate.Name = updateDishDto.Name;

            _dishRepository.UpdateDish(dishToUpdate);
            _dishRepository.SaveChanges();
            _logger.LogInformation($"{DateTime.Now}: Dish with id {id} updated");

			return Ok(_mapper.Map<ReadDishDto>(dishToUpdate));
        }
        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteDishById(int id)
        {
	        if (_dishRepository.GetDishById(id)==null)
	        {
		        throw new NotFoundRequestExceptionResponse($"Dish with id:{id} not found");
	        }
            _dishRepository.DeleteDishById(id);

            _dishRepository.SaveChanges();
            _logger.LogInformation($"{DateTime.Now}: Dish with {id} deleted");

            return Ok("Dish deleted successfully");
        }
    }
}