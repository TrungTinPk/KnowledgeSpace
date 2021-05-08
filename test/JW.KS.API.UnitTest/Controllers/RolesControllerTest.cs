
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JW.KS.API.Controllers;
using JW.KS.ViewModels;
using JW.KS.ViewModels.Systems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace JW.KS.API.UnitTest.Controllers
{
    public class RolesControllerTest
    {
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager; 
        private List<IdentityRole> _roleSources = new List<IdentityRole>(){
            new IdentityRole("test1"),
            new IdentityRole("test2"),
            new IdentityRole("test3"),
            new IdentityRole("test4")
        };
        public RolesControllerTest()
        {
            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(mockRoleStore.Object,null,null,null,null);
        }
        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {
            var rolesController = new RolesController(_mockRoleManager.Object);
            Assert.NotNull(rolesController);
        }
        
        [Fact]
        public async Task PostRole_ValidInput_Success()
        {
            _mockRoleManager
                .Setup(x => x.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);
            
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.PostRole(new RoleVM()
            {
                Id = "Test",
                Name = "Test"
            });
            Assert.NotNull(rolesController);
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task PostRole_ValidInput_Failed()
        {
            _mockRoleManager
                .Setup(x => x.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError[] { }));
            
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.PostRole(new RoleVM()
            {
                Id = "test",
                Name = "test"
            }); 
            Assert.NotNull(rolesController);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetRoles_HasData_ReturnSuccess()
        {
            _mockRoleManager
                .Setup(x => x.Roles)
                .Returns(_roleSources.AsQueryable().BuildMock().Object);
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.GetRoles();
            var okResult = result as OkObjectResult;
            var roleVMs = okResult.Value as IEnumerable<RoleVM>;
            Assert.True(roleVMs.Count()  > 0);
        }

        [Fact]
        public async Task GetRoles_ThrowException_Failed()
        {
            _mockRoleManager.Setup(x => x.Roles).Throws<Exception>();
            var rolesController = new RolesController(_mockRoleManager.Object);
            await Assert.ThrowsAsync<Exception>(async () => await rolesController.GetRoles());
        }

        [Fact]
        public async Task GetRolesPaging_NoFilter_ReturnSuccess()
        {
            _mockRoleManager
                .Setup(x => x.Roles)
                .Returns(_roleSources.AsQueryable().BuildMock().Object);
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.GetAllRolesPaging(null, 1, 2);
            var okResult = result as OkObjectResult;
            var roleVMs = okResult.Value as Pagination<RoleVM>;
            Assert.Equal(4, roleVMs.TotalRecords);
            Assert.Equal(2, roleVMs.Items.Count);
        }
        
        [Fact]
        public async Task GetRolesPaging_HasFilter_ReturnSuccess()
        {
            _mockRoleManager
                .Setup(x => x.Roles)
                .Returns(_roleSources.AsQueryable().BuildMock().Object);
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.GetAllRolesPaging("test3", 1, 2);
            var okResult = result as OkObjectResult;
            var roleVMs = okResult.Value as Pagination<RoleVM>;
            Assert.Equal(1, roleVMs.TotalRecords);
            Assert.Single(roleVMs.Items);
        }
        
        [Fact]
        public async Task GetRolesPaging_ThrowException_Failed()
        {
            _mockRoleManager.Setup(x => x.Roles).Throws<Exception>();
            var rolesController = new RolesController(_mockRoleManager.Object);
            await Assert.ThrowsAsync<Exception>(async () => await rolesController.GetAllRolesPaging(null,1,1));
        }

        [Fact]
        public async Task GetById_HasData_ReturnSuccess()
        {
            _mockRoleManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityRole()
                {
                    Id = "test1",
                    Name = "test1"
                });
            
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.GetById("test1");
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            var roleVM = okResult.Value as RoleVM;
            
            Assert.Equal("test1",roleVM.Name);
        }

        [Fact]
        public async Task GetById_ThrowException_Failed()
        {
            _mockRoleManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .Throws<Exception>();
            
            var rolesController = new RolesController(_mockRoleManager.Object);
            await Assert.ThrowsAnyAsync<Exception>(async () => await rolesController.GetById("test1"));
        }

        [Fact]
        public async Task PutRole_ValidInput_Success()
        {
            _mockRoleManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityRole()
                {
                    Id = "test",
                    Name = "test"
                });
            _mockRoleManager
                .Setup(x => x.UpdateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.PutRole("test", new RoleVM()
            {
                Id = "test",
                Name = "test"
            });
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutRole_ValidInput_Failed()
        {
            _mockRoleManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityRole()
                {
                    Id = "test",
                    Name = "test"
                });
            _mockRoleManager
                .Setup(x => x.UpdateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Failed());
            var rolesController = new RolesController(_mockRoleManager.Object);
            var result = await rolesController.PutRole("test", new RoleVM()
            {
                Id = "test",
                Name = "test"
            });
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteRole_ValidInput_Success()
        {
            _mockRoleManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityRole()
                {
                    Id = "test",
                    Name = "test"
                });
            _mockRoleManager
                .Setup(x => x.DeleteAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);
            var rolesManager = new RolesController(_mockRoleManager.Object);
            var result = await rolesManager.DeleteRole("test");
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteRole_ValidInput_Failed()
        {
            _mockRoleManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityRole()
                {
                    Id = "test",
                    Name = "test"
                });
            _mockRoleManager.Setup(x => x.DeleteAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError[] { }));
            var rolesManager = new RolesController(_mockRoleManager.Object);
            var result = await rolesManager.DeleteRole("test");
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
