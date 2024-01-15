using HotelBookingBlazor.Constants;
using HotelBookingBlazor.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingBlazor.Services;

public class SeedService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public SeedService(UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _userManager = userManager;
        _userStore = userStore;
        _roleManager = roleManager;
        _configuration = configuration;
        _contextFactory = contextFactory;
    }

    public async Task SeedDatabaseAsync()
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            if(context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        var adminUserEmail = _configuration.GetValue<string>("AdminUser:Email");

        var dbAdminUser = await _userManager.FindByEmailAsync(adminUserEmail);
        if (dbAdminUser is not null)
        {
            return; //Database already has admin user. No need to do anything
        }

        var applicationUser = new ApplicationUser()
        {
            FirstName = _configuration.GetValue<string>("AdminUser:FirstName")!,
            LastName = _configuration.GetValue<string>("AdminUser:LastName")!,
            RoleName = RoleType.Admin.ToString(),
            ContactNumber = _configuration.GetValue<string>("AdminUser:ContactNumber")!,
            Designation = "Adminstrator"
        };

        await _userStore.SetUserNameAsync(applicationUser, adminUserEmail, default);

        var emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
        await emailStore.SetEmailAsync(applicationUser, adminUserEmail, default);

        var result = await _userManager.CreateAsync(applicationUser, _configuration.GetValue<string>("AdminUser:Password")!);
        if(!result.Succeeded)
        {
            var errors = string.Join(Environment.NewLine, result.Errors.Select(e=> e.Description));
            throw new Exception($"Error in creating user. {errors}");
        }

        if(await _roleManager.FindByNameAsync(RoleType.Admin.ToString()) is null)
        {
            foreach (var roleName in Enum.GetNames<RoleType>())
            {
                var role = new IdentityRole
                {
                    Name= roleName
                };

                await _roleManager.CreateAsync(role);
            }
        }

        result = await _userManager.AddToRoleAsync(applicationUser, RoleType.Admin.ToString());
        if (!result.Succeeded)
        {
            var errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
            throw new Exception($"Error in adding user to Admin role. {errors}");
        }

    }

}
