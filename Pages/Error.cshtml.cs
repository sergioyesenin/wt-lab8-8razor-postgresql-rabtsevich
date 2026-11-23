using Microsoft.AspNetCore.Mvc.RazorPages;

namespace wt_lab8_8razor_postgresql_rabtsevich.Pages;

public class ErrorModel : PageModel
{
    public int HttpStatus { get; set; }
    public string? Message { get; set; }

    public void OnGet(int? code)
    {
        HttpStatus = code ?? 500;

        Message = HttpStatus switch
        {
            400 => "Некорректный запрос. Проверьте параметры.",
            403 => "Доступ запрещён.",
            404 => "Страница или объект не найдены.",
            405 => "Метод не поддерживается.",
            _   => "Произошла непредвиденная ошибка."
        };
    }
}
