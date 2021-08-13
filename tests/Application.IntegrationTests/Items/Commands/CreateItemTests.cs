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
using Newtonsoft.Json;
using Xunit;

namespace Application.IntegrationTests.Items.Commands
{
    public class CreateItemTests : IntegrationTest
    {
        [Fact]
        public async Task AdministratorShouldAddNewItem()
        {
            var userId = await AuthHelper.RunAsAdministratorAsync(_factory);
            var authResult = _identityService.LoginAsync(AdminUser.Email, AdminUser.Password);
            
            var command = new CreateItemCommand()
            {
                Brand = new BrandObject() {Id = Guid.NewGuid().ToString(), Name = "A brand new Brand"},
                Name = "Jordan 4 Retro University Red",
                ImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=700&h=500&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                SmallImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=300&h=214&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                ThumbUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=140&h=100&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                Make = "Jordan 4 Retro",
                Model = "University Blue",
                RetailPrice = 200,
                Description = "Jordan Brand paid homage to MJ’s alma mater with the Air Jordan 4 University Blue. The University Blue colorway draws a close resemblance to the extremely rare Air Jordan 4 UNC PE that was given to Tarheel student-athletes in 2019.\name\name\nThe Air Jordan 4 University Blue features a University Blue suede upper with mesh netting on the quarter panel and tongue. Similar to OG Jordan 4 colorways of the past, the eyelets, heel tab, and sections of the midsole are a speckled Cement Grey. Hits of black appear on the wing eyelets, sole, and Jumpman logo on the heel tab. Two woven labels are stitched to the tongue; one being the classic Jumpman woven label; the other being a Team Jordan jock tag. A black, white, and Tech Grey sole with a clear Air unit completes the design.\name\name\nThe Air Jordan 4 University Blue released in April of 2021 and retailed for $200."
            };
            
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{ApiBaseAddress}/{ItemsAddress}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            var response = await _client.SendAsync(request);
            
            response.StatusCode.Should().BeEquivalentTo(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task SupervisorShouldNotAddNewItem()
        {
            var userId = await AuthHelper.RunAsSupervisorAsync(_factory);
            var authResult = _identityService.LoginAsync(SupervisorUser.Email, SupervisorUser.Password);
            
            var command = new CreateItemCommand()
            {
                Brand = new BrandObject() {Id = Guid.NewGuid().ToString(), Name = "A brand new Brand"},
                Name = "Jordan 4 Retro University Red",
                ImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=700&h=500&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                SmallImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=300&h=214&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                ThumbUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=140&h=100&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                Make = "Jordan 4 Retro",
                Model = "University Blue",
                RetailPrice = 200,
                Description = "Jordan Brand paid homage to MJ’s alma mater with the Air Jordan 4 University Blue. The University Blue colorway draws a close resemblance to the extremely rare Air Jordan 4 UNC PE that was given to Tarheel student-athletes in 2019.\name\name\nThe Air Jordan 4 University Blue features a University Blue suede upper with mesh netting on the quarter panel and tongue. Similar to OG Jordan 4 colorways of the past, the eyelets, heel tab, and sections of the midsole are a speckled Cement Grey. Hits of black appear on the wing eyelets, sole, and Jumpman logo on the heel tab. Two woven labels are stitched to the tongue; one being the classic Jumpman woven label; the other being a Team Jordan jock tag. A black, white, and Tech Grey sole with a clear Air unit completes the design.\name\name\nThe Air Jordan 4 University Blue released in April of 2021 and retailed for $200."
            };
            
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{ApiBaseAddress}/{ItemsAddress}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            var response = await _client.SendAsync(request);
            
            response.StatusCode.Should().BeEquivalentTo(StatusCodes.Status403Forbidden);
        }
        
        [Fact]
        public async Task UserShouldNotAddNewItem()
        {
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);
            var authResult = _identityService.LoginAsync(DefaultUser.Email, DefaultUser.Password);
            
            var command = new CreateItemCommand()
            {
                Brand = new BrandObject() {Id = Guid.NewGuid().ToString(), Name = "A brand new Brand"},
                Name = "Jordan 4 Retro University Red",
                ImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=700&h=500&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                SmallImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=300&h=214&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                ThumbUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=140&h=100&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                Make = "Jordan 4 Retro",
                Model = "University Blue",
                RetailPrice = 200,
                Description = "Jordan Brand paid homage to MJ’s alma mater with the Air Jordan 4 University Blue. The University Blue colorway draws a close resemblance to the extremely rare Air Jordan 4 UNC PE that was given to Tarheel student-athletes in 2019.\name\name\nThe Air Jordan 4 University Blue features a University Blue suede upper with mesh netting on the quarter panel and tongue. Similar to OG Jordan 4 colorways of the past, the eyelets, heel tab, and sections of the midsole are a speckled Cement Grey. Hits of black appear on the wing eyelets, sole, and Jumpman logo on the heel tab. Two woven labels are stitched to the tongue; one being the classic Jumpman woven label; the other being a Team Jordan jock tag. A black, white, and Tech Grey sole with a clear Air unit completes the design.\name\name\nThe Air Jordan 4 University Blue released in April of 2021 and retailed for $200."
            };
            
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{ApiBaseAddress}/{ItemsAddress}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            var response = await _client.SendAsync(request);
            
            response.StatusCode.Should().BeEquivalentTo(StatusCodes.Status403Forbidden);
        }
    }
}