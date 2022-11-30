using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Model
    {
        public int ID { get; set; }

        public string? Text { get; set; }

        public int? Number { get; set; }

        public bool? Bool { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
    }
}
