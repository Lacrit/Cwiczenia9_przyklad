using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using System.Text;
using Apbd_auth.DTO;
using Apbd_auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Apbd_auth.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            // Z body przyjmujemy login oraz hasło użytkownika
            // Sprawdzamy w bazie czy użytkownik istnieje w bazie
            // Jeżeli nie: 400 BadRequest
            // Jeżeli tak: zwracamy go do kontrolera

            // var userFromDb = ....

            
            return Ok(new
            {
                appToken = new JwtSecurityTokenHandler().WriteToken(GenerateToken()),
                refreshToken = Guid.NewGuid()
            });
        }


        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            // Przyjmij refresh token z ciała żądania
            // Sprawdź czy w bazie mamy zapisany ten token przy którymś z użytkowników
            // oraz jeżeli tak, czy nie wygasł

            // Jeżeli refreshToken'a nie ma lub wygasł to zwróć 400 BadRequest.

            // Jeżeli refreshToken jest i nie wygasł to pobierz użytkownika z bazy i 
            //wygeneruj mu nowy token i refreshToken (refreshToken powininien być znowu zapisany (nadpisany) do bazy)


            // var userFromDb = ....
            var newRefreshToken = Guid.NewGuid();


            return Ok(new
            {
                appToken = new JwtSecurityTokenHandler().WriteToken(GenerateToken()),
                refreshToken = newRefreshToken
            });
        }



        [HttpPost("register")]
        public IActionResult Register(RegisterDto register)
        {

            var newUser = new User();

            // AQAAAAEAACcQAAAAEI2fWHJKCdNyewnOKHCfIFgzu6n2KFtHtVyvQg94L+zyXYoHbk39dmWF5CSLcEDb+w==
            // 12345678


            var hashedPassword = new PasswordHasher<User>().HashPassword(newUser,register.Password);


            // Porównywanie hasha z dostarczonym hasłem: Wynik Failed lub Success

            // var verifyPassword = new PasswordHasher<User>().VerifyHashedPassword(newUser,
            //     "AQAAAAEAACcQAAAAEI2fWHJKCdNyewnOKHCfIFgzu6n2KFtHtVyvQg94L+zyXYoHbk39dmWF5CSLcEDb+w==",
            //     "123sss45678");



            return Ok(hashedPassword);
        }





        private JwtSecurityToken GenerateToken()
        {
            // Claimsy wypełniany operując na obiekcie reprezentującym użytkownika z BD
            Claim[] claims =
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "jd"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Lecturer"),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Secret"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds

            );

            return token;
        }
    }
}
