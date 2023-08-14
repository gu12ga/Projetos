using health;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using(var context = new Context())
{
        // Creates the database if not exists
        if (!context.Database.EnsureCreated())
        {
            /*    public int idLoan { get; set; }
    public string type { get; set; }
    public int rate { get; set; }*/
        // Banco de dados não existe, imprime "hello" no console
           var l1 = new Loan{
                type = "personal",
                rate = 4

           };

           var l2 = new Loan{
                type = "collateralized",
                rate = 3

           };

            var l3 = new Loan{
                type = "payroll",
                rate = 2

           };

           context.Loan.Add(l1);
           context.Loan.Add(l2);
           context.Loan.Add(l3);
           context.SaveChanges();
        }
}

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
