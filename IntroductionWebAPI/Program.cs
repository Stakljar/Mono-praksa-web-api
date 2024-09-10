using Autofac;
using Autofac.Extensions.DependencyInjection;
using Introduction.Repository.Common;
using Introduction.Repository;
using Introduction.Service;
using Introduction.Service.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterType<CatRepository>().As<ICatRepository>().InstancePerDependency();
    containerBuilder.RegisterType<CatShelterRepository>().As<ICatShelterRepository>().InstancePerDependency();

    containerBuilder.RegisterType<CatService>().As<ICatService>().InstancePerDependency();
    containerBuilder.RegisterType<CatShelterService>().As<ICatShelterService>().InstancePerDependency();
});

var app = builder.Build();

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
