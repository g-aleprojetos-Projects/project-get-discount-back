using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_get_discount_back._1_Domain.Dtos;
using project_get_discount_back._2_Infrastructure.Schemas.Responses;
using project_get_discount_back.Queries;
using project_get_discount_back.Results;
using static project_get_discount_back.Entities.User;

namespace project_get_discount_back._3_Presentation.V1.Controllers
{
    /// <summary>
    /// Controller para criar usuário.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
     public class RegisterUserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor do RegisterUserController.
        /// </summary>
        /// <param name="mediator">Mediator para enviar a query de registro de usuário.</param>
        /// <param name="mapper">mapeia o request do registro de usuário.</param>
        public RegisterUserController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Cadastra usuário.
        /// </summary>
        /// <param name="registerUserDto">O nome, email e role do usuário</param>
        /// <param name="cancellationToken">O token de cancelamento de operação assíncrona.</param>
        /// <returns>O resultado da operação de cadastro do usuário.</returns>
        [HttpPost("/registerUser")]
        [Authorize(Roles = nameof(AccessType.ADMIN))]
        public async Task<ActionResult> Post([FromBody] RegisterUserDto registerUserDto, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<RegisterUserQuery>(registerUserDto);
            var result = await _mediator.Send(query, cancellationToken);
            if (result is Fail fail)
            {
                return BadRequest(new ErrosResponse(fail.Code, fail.Message));
            }
            return Ok(result);
        }
    }
}
