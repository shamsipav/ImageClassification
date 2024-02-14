namespace ImageAPI.Models
{
    public class PredictInfo
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public string Name { get; set; }

        public float Score { get; set; }

        public static List<PredictInfo> GetPredicts()
        {
            return new List<PredictInfo>()
                {
                    new PredictInfo { Id = 0, Label = "silk_spot", Name = "Шелковое пятно" },
                    new PredictInfo { Id = 1, Label = "water_spot", Name = "Водяное пятно" },
                };
        }
    }
}
