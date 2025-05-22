using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.Baskets;
using Domain.Models.Orders;
using Domain.Models.Products;
using Services.Specifications;
using ServicesAbstraction;
using Shared.DataTransferObjects.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService
        (IBasketRepository _basketRepository , IUnitOfWork _unitOfWork , IMapper _mapper) : IOrderService
    {
        public async Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest, string email)
        {
            var basket = await _basketRepository.GetAsync(orderRequest.BasketId)
                              ?? throw new BasketNotFoundException(orderRequest.BasketId);

            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();

            var specs = new OrderWithPaymentIntentSpcefications(basket.PaymentIntentId);
            var existingOrder = await orderRepo.GetByIdAsync(specs);

            if (existingOrder is not null) 
                orderRepo.Delete(existingOrder);

            List<OrderItems> items = [];
            foreach (var item in basket.Items) 
            {
                var originalProduct = await _unitOfWork.GetRepository<Product, int>()
                                                       .GetByIdAsync(item.Id)
                                                       ?? throw new ProductNotFoundException(item.Id);
                items.Add(CreateOrderItem(originalProduct,item));
            }
            var method = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                                          .GetByIdAsync(orderRequest.DeliveryMethodId)
                                          ?? throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);

            var address = _mapper.Map<OrderAddress>(orderRequest.Address);

            var subTotal = items.Sum(i => (i.Quantity * i.Price));

            var order = new Order(items,address,subTotal,email,method,basket.PaymentIntentId);

            orderRepo.Add(order);
            await _unitOfWork.SaveChanges();

            await _basketRepository.DeleteAsync(orderRequest.BasketId);  // delete the basket after create the order
            return _mapper.Map<OrderResponse>(order);
        }

        private OrderItems CreateOrderItem(Product originalProduct, BasketItem item)
        {
            return new OrderItems()
            {
                PictureUrl = originalProduct.PictureUrl,
                Price = originalProduct.Price,
                ProductName = originalProduct.Name,
                Quantity = item.Quantity,
                ProductId = originalProduct.Id,
            };
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync(string email)
        {
            var orders = await _unitOfWork.GetRepository<Order,Guid>()
                                          .GetAllAsync(new OrderSpecifications(email));

            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<OrderResponse> GetByIdAsync(Guid id)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>()
                                          .GetByIdAsync(new OrderSpecifications(id));

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<DeliveryMethodResponse>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods= await _unitOfWork.GetRepository<DeliveryMethod,int>()
                                                  .GetAllAsync();

            return _mapper.Map<IEnumerable<DeliveryMethodResponse>>(deliveryMethods);
        }
    }
}
