using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_get_discount_back._1_Domain.Dtos;
using project_get_discount_back._2_Infrastructure.Schemas.Responses;
using project_get_discount_back.Queries;
using project_get_discount_back.Results;
using project_get_discount_back.ViewModel;

namespace project_get_discount_back.V1.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de login de usuários.
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor do LoginController.
        /// </summary>
        /// <param name="mediator">Mediator para enviar a query de login.</param>
        /// <param name="mapper">mapeia o request do login.</param>
        public LoginController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém o login do usuário.
        /// </summary>
        /// <param name="loginDto">O email e password do usuário.</param>
        /// <param name="cancellationToken">O token de cancelamento de operação assíncrona.</param>
        /// <returns>O resultado da operação de login.</returns>
        [HttpPost("/login")]
        public async Task<ActionResult> Post([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<GetLoginQuery>(loginDto);
            var result = await _mediator.Send(query, cancellationToken);
            if (result is Fail<LoginViewModel> fail)
            {
                return BadRequest(new ErrosResponse(fail.Code, fail.Message));
            }
            return Ok(result);
        }
    }
}
