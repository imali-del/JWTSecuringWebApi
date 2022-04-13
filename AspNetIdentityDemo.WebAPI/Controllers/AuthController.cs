using AspNetIdentityDemo.Shared;
using AspNetIdentityDemo.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentityDemo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private IMailService _mailService;
        public AuthController(IUserService userService , IMailService mailService) 
        {
            _userService = userService;
            _mailService = mailService;
        }

        // /api/auth/register
        [HttpPost("Register")]
        public async Task<ActionResult> RegisterAsync([FromBody]RegisterViewModel model) 
        {
            if (ModelState.IsValid) 
            {
                var result = await _userService.RegisterUserAsync(model);
                if (result.IsSuccess) 
                {
                    return Ok(result); //Status Code 200
                }

                return BadRequest(result);
            }

            return BadRequest("Some properties are not Valid"); //Status Code 400
        }

        // /api/auth/login
        [HttpPost("Login")]
        public async Task<ActionResult> LoginAsync([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);
                if (result.IsSuccess)
                {
                    await _mailService.SendEmailAsync(model.Email, "New login", "<h1>Hey!, new login to your account noticed</h1><p>New login to your account at " + DateTime.Now + "</p>");
                    return Ok(result); //Status Code 200
                }

                return BadRequest(result);
            }

            return BadRequest("Some properties are not Valid"); //Status Code 400
        }
    }
}
