using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var employees = new List<Employee>(){
    new Employee { Id = 1, FirstName = "John", LastName = "Doe", SocialSecurityNumber = "123-45-4460" },
    new Employee { Id = 2, FirstName = "Jane", LastName = "Doe", SocialSecurityNumber = "223-54-4468" }
};


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var employeeRoute = app.MapGroup("employees");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

employeeRoute.MapGet(string.Empty, () => {
    return employees;
});

employeeRoute.MapGet("{id:int}", (int id) => {
    var employee = employees.SingleOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(employee);
});

employeeRoute.MapPost(string.Empty, (Employee employee) => {
    employee.Id = employees.Max(e => e.Id) + 1; // We're not using a database, so we need to manually assign an ID
    employees.Add(employee);
    return Results.Created($"/employees/{employee.Id}", employee);
});

employeeRoute.MapPut("{id}", ([FromBody] Employee employee, int id) => {
    var existingEmployee = employees.SingleOrDefault(e => e.Id == id);
    if (existingEmployee == null)
    {
        return Results.NotFound();
    }

    existingEmployee.FirstName = employee.FirstName;
    existingEmployee.LastName = employee.LastName;
    existingEmployee.Address1 = employee.Address1;
    existingEmployee.Address2 = employee.Address2;
    existingEmployee.City = employee.City;
    existingEmployee.State = employee.State;
    existingEmployee.ZipCode = employee.ZipCode;
    existingEmployee.PhoneNumber = employee.PhoneNumber;
    existingEmployee.Email = employee.Email;

    return Results.Ok(existingEmployee);
});

app.Run();

public partial class Program {}