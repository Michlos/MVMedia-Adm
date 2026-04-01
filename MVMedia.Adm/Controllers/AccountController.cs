using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVMedia.Adm.Models;
using MVMedia.Adm.Services;

namespace MVMedia.Adm.Controllers;

public class AccountController : Controller
{
    //for autentication
    private readonly ApiAuthService _apiAuthService;


    public AccountController(ApiAuthService apiAuthService)
    {
        _apiAuthService = apiAuthService;
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        await HttpContext.SignOutAsync(); // Limpa qualquer sessão anterior
        Response.Cookies.Delete("AuthToken"); // Remove o cookie de autenticação anterior, se existir
        Response.Cookies.Delete("IsAdmin"); // Remove o cookie de autenticação anterior, se existir
        Response.Cookies.Delete("Username"); // Remove o cookie de autenticação anterior, se existir
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserViewModel model, UserToken? token)
    {
        

        if (!ModelState.IsValid)
            return View(model);

        // Chama o serviço que faz a requisição para a API
        var userData = await _apiAuthService.LoginAsync(model.Username, model.Password);

        if (userData == null)
        {
            ModelState.AddModelError("", "Usuário ou senha inválidos.");
            return View(model);
        }

        

        // Salva o token em cookie seguro
        Response.Cookies.Append("AuthToken", userData.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });
        Response.Cookies.Append("IsAdmin", userData.IsAdmin.ToString().ToLower());
        //Response.Cookies.Append("IsAdmin", model.IsAdmin.ToString().ToLower(), new CookieOptions
        //{
        //    HttpOnly = false, //Deixe false para que o layout conseiga ler se necessário
        //    Expires = DateTimeOffset.Now.AddHours(8)
        //});
        Response.Cookies.Append("Username", model.Username);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(); // Limpa qualquer sessão anterior
        Response.Cookies.Delete("AuthToken"); // Remove o cookie de autenticação anterior, se existir
        Response.Cookies.Delete("IsAdmin"); // Remove o cookie de autenticação anterior, se existir
        Response.Cookies.Delete("Username");
        // Redireciona para a página de login
        return RedirectToAction("Index", "Home");
    }
}
