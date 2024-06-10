using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using project_get_discount_back._1_Domain.Dtos;
using project_get_discount_back._1_Domain.Queries;
using project_get_discount_back._2_Infrastructure.Schemas.Responses;
using project_get_discount_back.Queries;
using project_get_discount_back.Results;

namespace project_get_discount_back._3_Presentation.V1.Controllers
{
    /// <summary>
    /// Controller para criar e atualizar a senha do usuário.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RegisterPasswordController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor do RegisterPasswordController.
        /// </summary>
        /// <param name="mediator">Mediator para enviar a query de registro de senha do usuario.</param>
        /// <param name="mapper">mapeia o request do registro da senha.</param>
        public RegisterPasswordController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Cadastra senha do usuário.
        /// </summary>
        /// <param name="registerPasswordDto">O email e senha do usuário.</param>
        /// <param name="cancellationToken">O token de cancelamento de operação assíncrona.</param>
        /// <returns>O resultado da operação de cadastro da senha do usuário.</returns>
        [HttpPost("/registerPassword")]
        public async Task<ActionResult> Post([FromBody] RegisterPasswordDto registerPasswordDto, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<RegisterPasswordQuery>(registerPasswordDto);
            var result = await _mediator.Send(query, cancellationToken);
            if (result is Fail fail)
            {
                return BadRequest(new ErrosResponse(fail.Code, fail.Message));
            }
            return Ok();
        }
    }
}
