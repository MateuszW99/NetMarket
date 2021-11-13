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
    public class CreateItemTests : IntegrationTest
    {
        [Fact]
        public async Task AdministratorShouldAddNewItem()
        {
            var context = DbHelper.GetDbContext(_factory);
            
            var userId = await AuthHelper.RunAsAdministratorAsync(_factory);
            var authResult = _identityService.LoginAsync(AdminUser.Email, AdminUser.Password);
            
            var command = new CreateItemCommand()
            {
                Brand = "A brand new Brand",
                Name = "Jordan 4 Retro University Red",
                ImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=700&h=500&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                SmallImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=300&h=214&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                ThumbUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=140&h=100&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                Make = "Jordan 4 Retro",
                Model = "University Blue",
                Gender = "Men",
                RetailPrice = 200,
                Description = "Jordan Brand paid homage to MJ’s alma mater with the Air Jordan 4 University Blue. The University Blue colorway draws a close resemblance to the extremely rare Air Jordan 4 UNC PE that was given to Tarheel student-athletes in 2019.\name\name\nThe Air Jordan 4 University Blue features a University Blue suede upper with mesh netting on the quarter panel and tongue. Similar to OG Jordan 4 colorways of the past, the eyelets, heel tab, and sections of the midsole are a speckled Cement Grey. Hits of black appear on the wing eyelets, sole, and Jumpman logo on the heel tab. Two woven labels are stitched to the tongue; one being the classic Jumpman woven label; the other being a Team Jordan jock tag. A black, white, and Tech Grey sole with a clear Air unit completes the design.\name\name\nThe Air Jordan 4 University Blue released in April of 2021 and retailed for $200.",
                Category = "Sneakers"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Items}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            var response = await _client.SendAsync(request);

            var newItem = await context.Items
                .FirstOrDefaultAsync(x => x.Name == command.Name);

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            newItem.Should().NotBeNull();
            newItem.ImageUrl.Should().Be(command.ImageUrl);
            newItem.SmallImageUrl.Should().Be(command.SmallImageUrl);
            newItem.ThumbUrl.Should().Be(command.ThumbUrl);
            newItem.Make.Should().Be(command.Make);
            newItem.Model.Should().Be(command.Model);
            newItem.Gender.Should().Be(command.Gender);
            newItem.RetailPrice.Should().Be(command.RetailPrice);
            newItem.Description.Should().Be(command.Description);
            newItem.Category.Should().Be(command.Category);
        }

        [Fact]
        public async Task SupervisorShouldNotAddNewItem()
        {
            var context = DbHelper.GetDbContext(_factory);
            var oldItemsCount = context.Items.CountAsync();
            
            var userId = await AuthHelper.RunAsSupervisorAsync(_factory);
            var authResult = _identityService.LoginAsync(SupervisorUser.Email, SupervisorUser.Password);
            
            var command = new CreateItemCommand()
            {
                Brand = "A brand new Brand",
                Name = "Jordan 4 Retro University Red",
                ImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=700&h=500&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                SmallImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=300&h=214&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                ThumbUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=140&h=100&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                Make = "Jordan 4 Retro",
                Model = "University Blue",
                Gender = "Men",
                RetailPrice = 200,
                Description = "Jordan Brand paid homage to MJ’s alma mater with the Air Jordan 4 University Blue. The University Blue colorway draws a close resemblance to the extremely rare Air Jordan 4 UNC PE that was given to Tarheel student-athletes in 2019.\name\name\nThe Air Jordan 4 University Blue features a University Blue suede upper with mesh netting on the quarter panel and tongue. Similar to OG Jordan 4 colorways of the past, the eyelets, heel tab, and sections of the midsole are a speckled Cement Grey. Hits of black appear on the wing eyelets, sole, and Jumpman logo on the heel tab. Two woven labels are stitched to the tongue; one being the classic Jumpman woven label; the other being a Team Jordan jock tag. A black, white, and Tech Grey sole with a clear Air unit completes the design.\name\name\nThe Air Jordan 4 University Blue released in April of 2021 and retailed for $200."
            };
            
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Items}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            var response = await _client.SendAsync(request);

            var newCount = await context.Items.CountAsync();
            
            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            newCount.Should().Be(await oldItemsCount);
        }
        
        [Fact]
        public async Task UserShouldNotAddNewItem()
        {
            var context = DbHelper.GetDbContext(_factory);
            var oldItemsCount = context.Items.CountAsync();
            
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            var command = new CreateItemCommand()
            {
                Brand = "A brand new Brand",
                Name = "Jordan 4 Retro University Red",
                ImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=700&h=500&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                SmallImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=300&h=214&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                ThumbUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=140&h=100&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                Make = "Jordan 4 Retro",
                Model = "University Blue",
                Gender = "Men",
                RetailPrice = 200,
                Description = "Jordan Brand paid homage to MJ’s alma mater with the Air Jordan 4 University Blue. The University Blue colorway draws a close resemblance to the extremely rare Air Jordan 4 UNC PE that was given to Tarheel student-athletes in 2019.\name\name\nThe Air Jordan 4 University Blue features a University Blue suede upper with mesh netting on the quarter panel and tongue. Similar to OG Jordan 4 colorways of the past, the eyelets, heel tab, and sections of the midsole are a speckled Cement Grey. Hits of black appear on the wing eyelets, sole, and Jumpman logo on the heel tab. Two woven labels are stitched to the tongue; one being the classic Jumpman woven label; the other being a Team Jordan jock tag. A black, white, and Tech Grey sole with a clear Air unit completes the design.\name\name\nThe Air Jordan 4 University Blue released in April of 2021 and retailed for $200."
            };
            
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Items}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            var response = await _client.SendAsync(request);
            
            var newCount = await context.Items.CountAsync();
            
            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            newCount.Should().Be(await oldItemsCount);
        }

        [Fact]
        public async Task NotLoggedInUserShouldNotAddNewItem()
        {
            var context = DbHelper.GetDbContext(_factory);
            var oldItemsCount = context.Items.CountAsync();
            
            var command = new CreateItemCommand()
            {
                Brand = "A brand new Brand",
                Name = "Jordan 4 Retro University Red",
                ImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=700&h=500&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                SmallImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=300&h=214&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                ThumbUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=140&h=100&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                Make = "Jordan 4 Retro",
                Model = "University Blue",
                Gender = "Men",
                RetailPrice = 200,
                Description = "Jordan Brand paid homage to MJ’s alma mater with the Air Jordan 4 University Blue. The University Blue colorway draws a close resemblance to the extremely rare Air Jordan 4 UNC PE that was given to Tarheel student-athletes in 2019.\name\name\nThe Air Jordan 4 University Blue features a University Blue suede upper with mesh netting on the quarter panel and tongue. Similar to OG Jordan 4 colorways of the past, the eyelets, heel tab, and sections of the midsole are a speckled Cement Grey. Hits of black appear on the wing eyelets, sole, and Jumpman logo on the heel tab. Two woven labels are stitched to the tongue; one being the classic Jumpman woven label; the other being a Team Jordan jock tag. A black, white, and Tech Grey sole with a clear Air unit completes the design.\name\name\nThe Air Jordan 4 University Blue released in April of 2021 and retailed for $200."
            };
            
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Items}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            
            var response = await _client.SendAsync(request);
            
            var newCount = await context.Items.CountAsync();
            
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            newCount.Should().Be(await oldItemsCount);
        }
    }
}