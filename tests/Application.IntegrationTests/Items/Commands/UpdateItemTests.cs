using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Models.ApiModels.Items.Commands;
using Application.Models.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace Application.IntegrationTests.Items.Commands
{
    public class UpdateItemTests : IntegrationTest
    {
        [Fact]
        public async Task AdministratorShouldUpdateItem()
        {
            var context = DbHelper.GetDbContext(_factory);
            var oldItem = await context.Items.Include(x => x.Brand).FirstOrDefaultAsync();
            var itemId = oldItem.Id.ToString();

            await AuthHelper.RunAsAdministratorAsync(_factory);
            var authResult = _identityService.LoginAsync(AdminUser.Email, AdminUser.Password);
            
            var newName = "new name";
            var newMake = "new make";
            var newDescription = "new description";
            var newModel = "new model";
            var newGender = "new gender";
            var newUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=700&h=500&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672";
            var newRetailPrice = oldItem.RetailPrice + 1;
            var newBrand = await context.Brands.FirstOrDefaultAsync(x => x.Name != oldItem.Brand.Id.ToString());
            var category = "Sneakers";
            
            var command = new UpdateItemCommand()
            {
                Id = itemId,
                Name = newName,
                Description = newDescription,
                Make = newMake,
                Model = newModel,
                Gender = newGender,
                ImageUrl = newUrl,
                SmallImageUrl = newUrl,
                ThumbUrl = newUrl,
                RetailPrice = newRetailPrice,
                Brand = new BrandObject() { Id = newBrand.Id.ToString(), Name = newBrand.Name },
                Category = category
            };

            var request = new HttpRequestMessage(HttpMethod.Put,
                new Uri($"{Address.ApiBase}/{Address.Items}/{itemId}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            
            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
        
        [Fact]
        public async Task SupervisorShouldNotUpdateItem()
        {
            var context = DbHelper.GetDbContext(_factory);
            var oldItem = await context.Items.Include(x => x.Brand).FirstOrDefaultAsync();
            var itemId = oldItem.Id.ToString();

            await AuthHelper.RunAsSupervisorAsync(_factory);
            var authResult = _identityService.LoginAsync(SupervisorUser.Email, SupervisorUser.Password);
            
            var newName = "new name";
            var newMake = "new make";
            var newDescription = "new description";
            var newModel = "new model";
            var newUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=700&h=500&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672";
            var newRetailPrice = oldItem.RetailPrice + 1;
            var newBrand = await context.Brands.FirstOrDefaultAsync(x => x.Name != oldItem.Brand.Id.ToString());
            var category = "Sneakers";
            
            var command = new UpdateItemCommand()
            {
                Id = itemId,
                Name = newName,
                Description = newDescription,
                Make = newMake,
                Model = newModel,
                ImageUrl = newUrl,
                SmallImageUrl = newUrl,
                ThumbUrl = newUrl,
                RetailPrice = newRetailPrice,
                Brand = new BrandObject() { Id = newBrand.Id.ToString(), Name = newBrand.Name },
                Category = category
            };

            var request = new HttpRequestMessage(HttpMethod.Put,
                new Uri($"{Address.ApiBase}/{Address.Items}/{itemId}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            
            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        } 
        
        [Fact]
        public async Task DefaultUserShouldNotUpdateItem()
        {
            var context = DbHelper.GetDbContext(_factory);
            var oldItem = await context.Items.Include(x => x.Brand).FirstOrDefaultAsync();
            var itemId = oldItem.Id.ToString();

            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            var newName = "new name";
            var newMake = "new make";
            var newDescription = "new description";
            var newModel = "new model";
            var newUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=700&h=500&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672";
            var newRetailPrice = oldItem.RetailPrice + 1;
            var newBrand = await context.Brands.FirstOrDefaultAsync(x => x.Name != oldItem.Brand.Id.ToString());
            var category = "Sneakers";
            
            var command = new UpdateItemCommand()
            {
                Id = itemId,
                Name = newName,
                Description = newDescription,
                Make = newMake,
                Model = newModel,
                ImageUrl = newUrl,
                SmallImageUrl = newUrl,
                ThumbUrl = newUrl,
                RetailPrice = newRetailPrice,
                Brand = new BrandObject() { Id = newBrand.Id.ToString(), Name = newBrand.Name },
                Category = category
            };

            var request = new HttpRequestMessage(HttpMethod.Put,
                new Uri($"{Address.ApiBase}/{Address.Items}/{itemId}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            
            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }        
        
        [Fact]
        public async Task NotLoggedInUserShouldNotUpdateItem()
        {
            var context = DbHelper.GetDbContext(_factory);
            var oldItem = await context.Items.Include(x => x.Brand).FirstOrDefaultAsync();
            var itemId = oldItem.Id.ToString();

            var newName = "new name";
            var newMake = "new make";
            var newDescription = "new description";
            var newModel = "new model";
            var newUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=700&h=500&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672";
            var newRetailPrice = oldItem.RetailPrice + 1;
            var newBrand = await context.Brands.FirstOrDefaultAsync(x => x.Name != oldItem.Brand.Id.ToString());
            var category = "Sneakers";
            
            var command = new UpdateItemCommand()
            {
                Id = itemId,
                Name = newName,
                Description = newDescription,
                Make = newMake,
                Model = newModel,
                ImageUrl = newUrl,
                SmallImageUrl = newUrl,
                ThumbUrl = newUrl,
                RetailPrice = newRetailPrice,
                Brand = new BrandObject() { Id = newBrand.Id.ToString(), Name = newBrand.Name },
                Category = category
            };

            var request = new HttpRequestMessage(HttpMethod.Put,
                new Uri($"{Address.ApiBase}/{Address.Items}/{itemId}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }
    }
}