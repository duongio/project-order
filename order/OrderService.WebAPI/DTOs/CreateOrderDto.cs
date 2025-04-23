﻿namespace OrderService.WebAPI.DTOs
{
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public List<CreateOrderItemDto> Items {get; set;} = new List<CreateOrderItemDto>();
        public int PIN { get; set; }
    }
}
