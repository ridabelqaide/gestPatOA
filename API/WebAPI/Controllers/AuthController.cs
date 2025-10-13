using Azure.Core;
using PATOA.CORE.Entities;
using PATOA.CORE.Interfaces;
using PATOA.WebAPI.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PATOA.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
     

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authentifie un utilisateur
        /// </summary>
        /// <param name="loginDto">Informations de connexion</param>
        /// <returns>Token JWT et informations de l'utilisateur</returns>
        /// <response code="200">Authentification réussie</response>
        /// <response code="400">Données invalides</response>
        /// <response code="401">Identifiants invalides</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginRequest), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto.Username,loginDto.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Enregistre un nouvel utilisateur
        /// </summary>
        /// <param name="registerDto">Informations d'inscription</param>
        /// <returns>Token JWT et informations de l'utilisateur</returns>
        /// <response code="201">Inscription réussie</response>
        /// <response code="400">Données invalides</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(Ok), 201)]
        [ProducesResponseType(400)]
         
        public async Task<ActionResult> Register([FromBody] RegisterRequest registerDto)
        {
            try
            {
                var account = new Account(registerDto.Username, registerDto.Email);
                account.RoleId = registerDto.RoleId;
                var result = await _authService.RegisterAsync(account, registerDto.Password);
                return CreatedAtAction(nameof(Login), result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

 
        [HttpPost("change-password")]
        [Authorize]

        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var accountId = (User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var result = await _authService.ChangePasswordAsync(new Guid(accountId), request.CurrentPassword, request.NewPassword);

            if (!result)
                return BadRequest(new { message = "Current password is incorrect" });

            return Ok(new { message = "Password changed successfully" });
        }

        [HttpPost("deactivate/{id}")]
        [Authorize]

        public async Task<ActionResult> DeactivateAccount(Guid id)
        {
            var result = await _authService.DeactivateAccountAsync(id);
            if (!result)
                return NotFound(new { message = "Account not found" });

            return Ok(new { message = "Account deactivated successfully" });
        }

        [HttpPost("reactivate/{id}")]
        [Authorize]

        public async Task<ActionResult> ReactivateAccount(Guid id)
        {
            var result = await _authService.ReactivateAccountAsync(id);
            if (!result)
                return NotFound(new { message = "Account not found" });

            return Ok(new { message = "Account reactivated successfully" });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}