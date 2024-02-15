using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;

namespace ImageAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly PredictionEnginePool<MachineLearningModel.ModelInput, MachineLearningModel.ModelOutput> _predictionEngine;

        public ImageController(PredictionEnginePool<MachineLearningModel.ModelInput, MachineLearningModel.ModelOutput> predictionEngine)
        {
            _predictionEngine = predictionEngine;
        }

        [HttpPost("predict")]
        public async Task<IActionResult> Index()
        {
            var formCollection = await Request.ReadFormAsync();

            var file = formCollection?.Files?.First();

            if (file == null)
                return BadRequest("Необходимо выбрать файл для анализа");

            // Считывание данных файла в массив байтов
            using MemoryStream ms = new MemoryStream();

            file.OpenReadStream().CopyTo(ms);

            byte[] imageBytes = ms.ToArray();

            // Создание входных данных для модели машинного обучения
            var input = new MachineLearningModel.ModelInput()
            {
                ImageSource = imageBytes
            };

            // Получение результатов прогнозирования от модели
            var predictResult = _predictionEngine.Predict(input);

            // Получение коллекции (точность - наименование)
            var scoresWithLabels = MachineLearningModel.GetSortedScoresWithLabels(predictResult);

            return Ok(scoresWithLabels);
        }
    }
}
