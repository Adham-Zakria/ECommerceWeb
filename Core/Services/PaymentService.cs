using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.Orders;
using Microsoft.Extensions.Configuration;
using Services.Specifications;
using ServicesAbstraction;
using Shared.DataTransferObjects.Basket;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Domain.Models.Products.Product;

namespace Services
{
    public class PaymentService
        (IConfiguration _configuration , IBasketRepository _basketRepository,
        IUnitOfWork _unitOfWork , IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntent(string basketId)
        {
            //1) read key from the app settings => IConfiguration
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            //2) get basket by basket's id => basket repository
            var basket = await _basketRepository.GetAsync(basketId)
                ?? throw new BasketNotFoundException(basketId);

            //3) check prices of the basket's items => IUnitOfWork
            foreach (var item in basket.Items)
            {
                var originalProduct = await _unitOfWork.GetRepository<Product, int>()
                                                       .GetByIdAsync(item.Id)
                                                       ?? throw new ProductNotFoundException(item.Id);
                item.Price = originalProduct.Price;
            }

            //4) check if the delivery method exists or not
            ArgumentNullException.ThrowIfNull(basket.DeliveryMethodId);
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                                            .GetByIdAsync(basket.DeliveryMethodId)
                                            ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId);
            
            basket.ShippingPrice = deliveryMethod.Price;

            //5) calculate total amount
            var amount = ( basket.Items.Sum(i => i.Price * i.Quantity) + deliveryMethod.Price ) * 100; // 100 for 100 cent
           
            //6) create or update payment
            var service = new PaymentIntentService();

            if (string.IsNullOrWhiteSpace(basket.PaymentIntentId))// create
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)amount,
                    Currency = "USD",
                    PaymentMethodTypes = ["Card"]
                };

                var paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // update 
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)amount,
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.CreateOrUpdateAsync(basket);
            return _mapper.Map<BasketDto>(basket);
        }


        public async Task UpdateOrderPaymentStatus(string jsonRequest, string stripeHeader)
        {
            var stripeEvent = EventUtility.ConstructEvent(jsonRequest,
                                stripeHeader, _configuration["Stripe:EndPointSecret"]);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            if(stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                await UpdatePaymentFailedAsync(paymentIntent.Id);
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded) 
            {
                await UpdatePaymentRecivedAsync(paymentIntent.Id);
            }
            else // handle other event types
            {
                Console.WriteLine("Unhandled event type : {0}", stripeEvent.Type);
            }

        }

        private async Task UpdatePaymentRecivedAsync(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order,Guid>()
                                    .GetByIdAsync(new OrderWithPaymentIntentSpcefications(paymentIntentId));

            order.PaymentStatus = PaymentStatus.PaymentReceived;
            _unitOfWork.GetRepository<Order, Guid>().Update(order);

            await _unitOfWork.SaveChanges();
        }

        private async Task UpdatePaymentFailedAsync(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>()
                                    .GetByIdAsync(new OrderWithPaymentIntentSpcefications(paymentIntentId));

            order.PaymentStatus = PaymentStatus.PaymentFailed;
            _unitOfWork.GetRepository<Order, Guid>().Update(order);

            await _unitOfWork.SaveChanges();
        }
    }
}
