using ImageAPI.Models;
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
        public IActionResult Index(IFormFile file)
        {
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

            // Получение списка возможных предсказаний
            var predictInfoList = PredictInfo.GetPredicts();

            // Обновление результатов предсказаний согласно полученным значениям
            for (int i = 0; i < predictResult.Score.Length; i++)
            {
                var label = predictInfoList.FirstOrDefault(x => x.Id == i);

                if (label != null) label.Score = predictResult.Score[i];
            }

            // Возврат отсортированного списка предсказаний с учетом Score
            return Ok(predictInfoList.OrderByDescending(x => x.Score).ToList());
        }
    }
}
