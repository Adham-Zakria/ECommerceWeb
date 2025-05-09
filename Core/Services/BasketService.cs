using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.Baskets;
using ServicesAbstraction;
using Shared.DataTransferObjects.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BasketService(IBasketRepository _basketRepository , IMapper _mapper) : IBasketService
    {
        public async Task DeleteAsync(string id)
           => await _basketRepository.DeleteAsync(id);
        
        public async Task<BasketDto> GetAsync(string id)
        {
            var basket = await _basketRepository.GetAsync(id) ?? throw new BasketNotFoundException(id);
            // mapping from CustomerBasket to BasketDto
            return _mapper.Map<BasketDto>(basket);
        }

        public async Task<BasketDto> UpdateAsync(BasketDto basketDto)
        {
            var basket = _mapper.Map<CustomerBasket>(basketDto);
           var updatedBasket = await _basketRepository.CreateOrUpdateAsync(basket) ??
                throw new Exception("Basket can't be created");
            return _mapper.Map<BasketDto>(updatedBasket);
        }
    }
}
