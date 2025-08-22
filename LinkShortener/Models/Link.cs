using System.ComponentModel.DataAnnotations;

namespace LinkShortener.Models
{
    public class Link
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "URL обязателен")]
        [Url(ErrorMessage = "Некорректный формат URL")]
        [Display(Name = "Оригинальный URL")]
        public string OriginalUrl { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "Короткий код должен быть до 10 символов")]
        [Display(Name = "Короткий код")]
        public string ShortCode { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}