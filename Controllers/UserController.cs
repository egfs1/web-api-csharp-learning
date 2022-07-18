﻿using Microsoft.AspNetCore.Mvc;
using WebAPI.Errors;
using WebAPI.Services.Users;

namespace WebAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext context;

        public UserController(DataContext context)
        {
            this.context = context;
        }

        [Helpers.Authorize]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult<User>> Update([FromBody] UpdateRequest request, string id)
        {

            if (!id.Equals(HttpContext.Items["UserId"])) throw new AppException("Unauthorazed");

            UpdateUserService updateUserService = new(context);

            User user = await updateUserService.Execute(id, request.Login, request.Email, request.Password, request.OldPassword);

            return Ok(user);
        }

        [Helpers.Authorize]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            if (!id.Equals(HttpContext.Items["UserId"])) throw new AppException("Unauthorazed");

            DeleteUserService deleteUserService = new(context);

            await deleteUserService.Execute(id);

            return Ok();
        }
    }

    public class UpdateRequest
    {
        public string Login { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string OldPassword { get; set; } = string.Empty;

    }
}
