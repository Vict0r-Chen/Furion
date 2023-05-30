using Furion.Tests.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// ×¢²áÒÀÀµ×¢ÈëÄ£¿é
builder.Services.AddDependencyInjection(builder =>
{
});

builder.Services.AddNamedScoped<ITestService, TestService>("test");
builder.Services.AddNamedScoped<ITestService, TestService2>("test2");
builder.Services.AddNamedScoped<TestService>("test3");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var s = app.Services.CreateScope().ServiceProvider;
var c = s.GetNamedService<ITestService>("test");
var b = s.GetNamedService<ITestService>("test2");
var f = s.GetNamedService<ITestService>("test");
var g = s.GetNamedService<TestService>("test3");
var h = s.GetNamedService<ITestService>("none");
var e = c == f;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();