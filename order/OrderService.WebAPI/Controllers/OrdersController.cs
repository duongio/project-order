using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using OrderService.WebAPI.Models;
using OrderService.WebAPI.DTOs;
using OrderService.WebAPI.Repository;
using OrderService.WebAPI.Services;

namespace OrderService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersRepository _orderRepository;
        private readonly OrderProcessor _orderProcessor;

        public OrdersController(IOrdersRepository ordersRepository, OrderProcessor orderProcessor) 
        {
            _orderRepository = ordersRepository;
            _orderProcessor = orderProcessor;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetAllOrders()
        {
            try
            {
                var orders = await _orderRepository.GetAllOrdersAsync();
                return Ok(orders);
            } catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto?>> GetOrderById(Guid id)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound("Order not found");
                }
                return Ok(order);
            } catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            try
            {
                var order = await _orderRepository.CreateOrderAsync(orderDto);
                if (order != null)
                {
                    _orderProcessor.CreateOrder(order);
                }
                return Ok(order);
            } catch 
            { 
                return BadRequest(); 
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            try
            {
                await _orderRepository.DeleteOrderAsync(id);
                return NoContent();
            } catch (KeyNotFoundException)
            {
                return NotFound("Order not found");
            } catch
            {
                return BadRequest("Failed to delete order");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            if (id != updateOrderDto.Id)
            {
                return BadRequest("Id mismatch");
            }
            try
            {
                await _orderRepository.UpdateOrderAsync(updateOrderDto);
                return NoContent();
            } catch (KeyNotFoundException)
            {
                return NotFound("Order not found");
            } catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            } catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
