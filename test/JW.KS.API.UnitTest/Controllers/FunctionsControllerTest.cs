using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JW.KS.API.Controllers;
using JW.KS.API.Data;
using JW.KS.API.Data.Entities;
using JW.KS.ViewModels;
using JW.KS.ViewModels.Systems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace JW.KS.API.UnitTest.Controllers
{
    public class FunctionsControllerTest
    {
        private ApplicationDbContext _context;
        private Mock<ILogger<FunctionsController>> _mockLogger;

        public FunctionsControllerTest()
        {
            _context = new InMemoryDbContextFactory().GetApplicationDbContext();
            _mockLogger = new Mock<ILogger<FunctionsController>>();
        }
        
        [Fact]
        public void Should_CreateInstance_NotNull_Success()
        {
            var controller = new FunctionsController(_context, _mockLogger.Object);
            
            Assert.NotNull(controller);
        }
        
        [Fact]
        public async Task PostFunction_ValidInput_Success()
        {
            var controller = new FunctionsController(_context, _mockLogger.Object);
            var result = await controller.PostFunction(new FunctionCreateRequest()
            {
                Id = "PostFunction_ValidInput_Success",
                ParentId = null,
                Name = "PostFunction_ValidInput_Success",
                SortOrder = 5,
                Url = "/PostFunction_ValidInput_Success"
            });
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task PostFunction_ValidInput_Failed()
        {
            _context.Functions.AddRange(new List<Function>()
            {
                new Function(){
                    Id = "PostFunction_ValidInput_Failed",
                    ParentId = null,
                    Name = "PostFunction_ValidInput_Failed",
                    SortOrder =1,
                    Url ="/PostFunction_ValidInput_Failed"
                }
            });
            await _context.SaveChangesAsync();
            var functionsController = new FunctionsController(_context, _mockLogger.Object);

            var result = await functionsController.PostFunction(new FunctionCreateRequest()
            {
                Id = "PostFunction_ValidInput_Failed",
                ParentId = null,
                Name = "PostFunction_ValidInput_Failed",
                SortOrder = 5,
                Url = "/PostFunction_ValidInput_Failed"
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetFunctions_HasData_ReturnSuccess()
        {
            _context.Functions.AddRange(new List<Function>()
            {
                new Function(){
                    Id = "GetFunction_HasData_ReturnSuccess",
                    ParentId = null,
                    Name = "GetFunction_HasData_ReturnSuccess",
                    SortOrder =1,
                    Url ="/GetFunction_HasData_ReturnSuccess"
                }
            });
            await _context.SaveChangesAsync();
            var functionsController = new FunctionsController(_context, _mockLogger.Object);
            var result = await functionsController.GetFunctions();
            var okResult = result as OkObjectResult;
            var functionVms = okResult.Value as IEnumerable<FunctionVm>;
            Assert.True(functionVms.Count() > 0);
        }

        [Fact]
        public async Task GetFunctionsPaging_NoFilter_ReturnSuccess()
        {
            _context.Functions.AddRange(new List<Function>()
            {
                new Function(){
                    Id = "GetFunctionsPaging_NoFilter_ReturnSuccess1",
                    ParentId = null,
                    Name = "GetFunctionsPaging_NoFilter_ReturnSuccess1",
                    SortOrder =1,
                    Url ="/test1"
                },
                new Function(){
                    Id = "GetFunctionsPaging_NoFilter_ReturnSuccess2",
                    ParentId = null,
                    Name = "GetFunctionsPaging_NoFilter_ReturnSuccess2",
                    SortOrder =2,
                    Url ="/test2"
                },
                new Function(){
                    Id = "GetFunctionsPaging_NoFilter_ReturnSuccess3",
                    ParentId = null,
                    Name = "GetFunctionsPaging_NoFilter_ReturnSuccess3",
                    SortOrder = 3,
                    Url ="/test3"
                },
                new Function(){
                    Id = "GetFunctionsPaging_NoFilter_ReturnSuccess4",
                    ParentId = null,
                    Name = "GetFunctionsPaging_NoFilter_ReturnSuccess4",
                    SortOrder =4,
                    Url ="/test4"
                }
            });
            await _context.SaveChangesAsync();
            var functionsController = new FunctionsController(_context, _mockLogger.Object);
            var result = await functionsController.GetFunctionsPaging(null, 1, 2);
            var okResult = result as OkObjectResult;
            var functionVm = okResult.Value as Pagination<FunctionVm>;
            Assert.Equal(4, functionVm.TotalRecords);
            Assert.Equal(2, functionVm.Items.Count);
        }
        
        [Fact]
        public async Task GetFunctionsPaging_HasFilter_ReturnSuccess()
        {
            _context.Functions.AddRange(new List<Function>()
            {
                new Function(){
                    Id = "GetFunctionsPaging_HasFilter_ReturnSuccess",
                    ParentId = null,
                    Name = "GetFunctionsPaging_HasFilter_ReturnSuccess",
                    SortOrder = 3,
                    Url ="/GetFunctionsPaging_HasFilter_ReturnSuccess"
                }
            });
            await _context.SaveChangesAsync();

            var functionsController = new FunctionsController(_context, _mockLogger.Object);
            var result = await functionsController.GetFunctionsPaging("GetFunctionsPaging_HasFilter_ReturnSuccess", 1, 2);
            var okResult = result as OkObjectResult;
            var functionVm = okResult.Value as Pagination<FunctionVm>;
            Assert.Equal(1, functionVm.TotalRecords);
            Assert.Single(functionVm.Items);
        }
        
        [Fact]
        public async Task GetById_HasData_ReturnSuccess()
        {
            _context.Functions.AddRange(new List<Function>()
            {
                new Function(){
                    Id = "GetById_HasData_ReturnSuccess",
                    ParentId = null,
                    Name = "GetById_HasData_ReturnSuccess",
                    SortOrder =1,
                    Url ="/GetById_HasData_ReturnSuccess"
                }
            });
            await _context.SaveChangesAsync();
            var functionsController = new FunctionsController(_context, _mockLogger.Object);
            var result = await functionsController.GetById("GetById_HasData_ReturnSuccess");
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            var functionVm = okResult.Value as FunctionVm;
            Assert.Equal("GetById_HasData_ReturnSuccess", functionVm.Id);
        }
        
        [Fact]
        public async Task PutFunction_ValidInput_Success()
        {
            var functionsController = new FunctionsController(_context, _mockLogger.Object);

            var result = await functionsController.PutFunction("PutFunction_ValidInput_Failed", new FunctionCreateRequest()
            {
                ParentId = null,
                Name = "PutFunction_ValidInput_Failed update",
                SortOrder = 6,
                Url = "/PutFunction_ValidInput_Failed"
            });
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteFunction_ValidInput_Success()
        {
            _context.Functions.AddRange(new List<Function>()
            {
                new Function(){
                    Id = "DeleteFunction_ValidInput_Success",
                    ParentId = null,
                    Name = "DeleteFunction_ValidInput_Success",
                    SortOrder =1,
                    Url ="/DeleteFunction_ValidInput_Success"
                }
            });
            await _context.SaveChangesAsync();
            var functionsController = new FunctionsController(_context, _mockLogger.Object);
            var result = await functionsController.DeleteFunction("DeleteFunction_ValidInput_Success");
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteFunction_ValidInput_Failed()
        {
            var functionsController = new FunctionsController(_context, _mockLogger.Object);
            var result = await functionsController.DeleteFunction("DeleteFunction_ValidInput_Failed");
            Assert.IsType<NotFoundResult>(result);
        }
    }
}