using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBankApi.DTOs;
using MyBankApi.Models;
using MyBankApi.Services.Interface;
using System;
using System.Collections.Generic;

namespace MyBankApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly IAccountService _account;
        private readonly Mapper _mapper;

        /// <summary>
        /// inject account service interface and mapper in the "Account Controller" constructor
        /// </summary>
        /// <param name="account"></param>
        /// <param name="mapper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AccountsController(IAccountService account, Mapper mapper)
        {
            _account = account ?? throw new ArgumentNullException(nameof(account));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [HttpPost("create-new-account")]
        public IActionResult CreateNewAccount([FromBody] NewAccountDto newAccountDto)
        {
            // map Account to newAccountDto
            var account = _mapper.Map<Account>(newAccountDto);
            var accountCreated = _account.Create(account, newAccountDto.Pin, newAccountDto.ConfirmPin);
            return Ok(accountCreated);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [HttpGet("get-all-accounts")]
        public IActionResult GetAllAccount()
        {
            var accounts = _account.GetAllAccounts();
            var acc = _mapper.Map <IList<AccountDto>>(accounts);
            return Ok(acc);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [HttpPost("authenticate")]
        public IActionResult Authentication([FromBody] AuthenticationDto authenticationDto)
        {
            var success = _account.Authenticate(authenticationDto.AccountNumber, authenticationDto.Pin);
            if (success == null)
            {
                return BadRequest();
            }
            return Ok(success);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [HttpGet("get-account-by-number")]
        public IActionResult GetAccountByNumber(string accountNumber)
        {
            
            var account = _account.GetByAccountNumber(accountNumber);
            var acc = _mapper.Map<AccountDto>(account);
            if (account != null)
            {
                return Ok(acc);
            }
            return NotFound(account);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [HttpGet("get-account-by-id")]
        public IActionResult GetAccountById(int id)
        {
            var account = _account.GetAccountById(id);
            var acc = _mapper.Map<AccountDto>(account);
            if (account != null)
            {
                return Ok(acc);
            }
            return NotFound(account);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [HttpPut("update-account")]
        public IActionResult UpdateAccount([FromBody] AccountUpdateDto accountUpdateDto)
        {
            var account = _mapper.Map<Account>(accountUpdateDto);
            if (account != null)
            {
                _account.Update(account, accountUpdateDto.Pin);
                return Ok(account);
            }
            return NotFound(account);
        }
    }
}
