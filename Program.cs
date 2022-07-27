using MailTesting1.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MailTesting1.Services;
using MimeKit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<MailService>();
builder.Services.AddTransient<MailContents>();

builder.Services.AddOptions();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
// Add DbContext 
builder.Services.AddDbContext<MailContext>();
// Add Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
                .AddEntityFrameworkStores<MailContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(enpoints =>
{
    enpoints.MapGet("/SendMail", async context =>
    {
        var mailService = context.RequestServices.GetService<MailService>();
        var mailContents = context.RequestServices.GetService<MailContents>();
        var mailSettingsOpts = context.RequestServices.GetService<IOptions<MailSettings>>()!.Value;
        // (MailContents) IMPORTANT properties: To, ReceivedAddress, Cc, Bcc, Subject, Body
        //
        // 'ReplyTo' property is opitons
        mailContents!.To = "Tiến lập trình, đẹp trai, ngầu loi";
        mailContents.ReceivedAddress = "tienvn998@gmail.com";
        mailContents.Subject = "Mail Testing";
        mailContents.Body = "<i>Send mail success</i>\n testing <br>    ! xuống dòng ";
        try
        {
            mailContents.Bcc!.Append<MailboxAddress>(new MailboxAddress("Trẻ trâu", "rboy18304@gmail.com"));
            // mailContents.ReplyTo!.Append<MailboxAddress>(new MailboxAddress("Cty TNHH MTV", "fawals98@gmail.com"))
            //     .Append<MailboxAddress>(new MailboxAddress("Trẻ trâu", "rboy18304@gmail.com"));
        }
        catch (System.Exception e)
        {
            await context.Response.WriteAsync(e.Message);
        }
        string rs = await mailService!.SendMail(mailContents!);
        await context.Response.WriteAsync(rs);
        try
        {
            // var fakeMime = mailService._message!.ReplyTo.Count;
            mailSettingsOpts.ReplyTo!.Add("a","b");
            KeyValuePair<string,string> Mime = mailSettingsOpts.ReplyTo.First();
            // var name1 = mailSettingsOpts.ReplyTo[0];
            // await context.Response.WriteAsync(fakeMime.ToString());
            await context.Response.WriteAsync(Mime.Value);
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message + " error here!");
        }
    });
});


app.MapRazorPages();

app.Run();
