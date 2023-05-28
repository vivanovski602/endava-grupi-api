using log4net.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace endavaRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IShopRepository _shopRepository;
        public ShopController(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
        }
        [HttpPost("user/Add")]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            var createdUser = await _shopRepository.AddUser(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);


        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {

            return await _shopRepository.Get(id);
        }

        [HttpPost("user/Reset-Password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequest resetRequest)
        {
            //Check if username exists
            var user = await _shopRepository.GetUserByName(resetRequest.Name);
            if (user == null)
            {
                return NotFound("Invalid username or e-mail!");
            }
            //Check if Email exists
            user = await _shopRepository.GetUserByEmail(resetRequest.Email);
            if (user == null)
            {
                return NotFound("Invalid username or e-mail!");
            }
            //Check if the old password is correct
            if (user.Password != resetRequest.Password)
            {
                return BadRequest("Old password is incorrect!");
            }
            //Check if the new password is the same as the old password
            if (user.Password == resetRequest.NewPassword)
            {
                return BadRequest("This password was previously used, add a new password!");
            }
            //Reset password
            user.Password = resetRequest.NewPassword;
            await _shopRepository.UpdateUser(user);
            return Ok("Password reset successfully!");
        }
    }
}

