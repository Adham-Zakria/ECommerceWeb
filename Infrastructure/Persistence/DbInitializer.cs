﻿using Domain.Contracts;
using Domain.Models.Identity;
using Domain.Models.Orders;
using Domain.Models.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DbInitializer(StoreDbContext _storeDbContext ,
        UserManager<ApplicationUser> _userManager , RoleManager<IdentityRole> _roleManager) : IDbInitializer
    {
        public async Task InitializeAsync()
        {
            // in deployment
            //if ((await _storeDbContext.Database.GetPendingMigrationsAsync()).Any())
            //{
            //    await _storeDbContext.Database.MigrateAsync();  // if you find migrations applay it 
            //}

            //in development
            try
            {
                if (!_storeDbContext.Set<ProductBrand>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(data);

                    if (brands is not null && brands.Any())
                        _storeDbContext.Set<ProductBrand>().AddRange(brands);

                    await _storeDbContext.SaveChangesAsync();
                }

                if (!_storeDbContext.Set<ProductType>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(data);

                    if (types is not null && types.Any())
                        _storeDbContext.Set<ProductType>().AddRange(types);

                    await _storeDbContext.SaveChangesAsync();
                }

                if (!_storeDbContext.Set<Product>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(data);

                    if (products is not null && products.Any())
                        _storeDbContext.Set<Product>().AddRange(products);

                    await _storeDbContext.SaveChangesAsync();
                }
                if (!_storeDbContext.Set<DeliveryMethod>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\deliveryMethods.json");
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(data);

                    if (deliveryMethods is not null && deliveryMethods.Any())
                        _storeDbContext.Set<DeliveryMethod>().AddRange(deliveryMethods);

                    await _storeDbContext.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task InitializeIdentityAsync()
        {
            // in deployment
            //if ((await _storeDbContext.Database.GetPendingMigrationsAsync()).Any())
            //{
            //    await _storeDbContext.Database.MigrateAsync();  // if you find migrations applay it 
            //}

            //in development
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!_userManager.Users.Any())
                {
                    var superAdminUser = new ApplicationUser()
                    {
                        DisplayName = "Super Admin",
                        UserName = "superadmin",
                        Email = "superadmin@gmail.com"
                    };
                    var adminUser = new ApplicationUser()
                    {
                        DisplayName = "Admin",
                        UserName = "admin",
                        Email = "admin@gmail.com"
                    };
                    await _userManager.CreateAsync(superAdminUser,"P@ssw0rd");
                    await _userManager.CreateAsync(adminUser,"password"); // add the user and his password

                    await _userManager.AddToRoleAsync(superAdminUser,"SuperAdmin");
                    await _userManager.AddToRoleAsync(adminUser,"Admin");
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
