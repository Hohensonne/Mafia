using Mafia.Core.Models;
using System.ComponentModel.DataAnnotations;
namespace Mafia.API.Contracts;

public record class CreateOrderRequest
(
    [Required] string Address, 
    [Required] PaymenthMethod PaymentMethod
);