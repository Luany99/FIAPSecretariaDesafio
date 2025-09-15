using Dapper;
using FIAPSecretariaDesafio.API.Helpers;
using FIAPSecretariaDesafio.Application.Interfaces;
using FIAPSecretariaDesafio.Application.Services;
using FIAPSecretariaDesafio.Application.Validators;
using FIAPSecretariaDesafio.Domain.Entities;
using FIAPSecretariaDesafio.Domain.Interfaces;
using FIAPSecretariaDesafio.Infrastructure.Data;
using FIAPSecretariaDesafio.Infrastructure.Repositories;
using FIAPSecretariaDesafio.Infrastructure.TypeHandlers;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FIAPSecretariaDesafio.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddSingleton<DapperContext>(sp =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                return new DapperContext(connectionString);
            });

            SqlMapper.AddTypeHandler(new CpfTypeHandler());
            SqlMapper.AddTypeHandler(new EmailTypeHandler());
            SqlMapper.AddTypeHandler(new SenhaTypeHandler());

            builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
            builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();
            builder.Services.AddScoped<ITurmaRepository, TurmaRepository>();
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            builder.Services.AddScoped<IAlunoService, AlunoService>();
            builder.Services.AddScoped<IMatriculaService, MatriculaService>();
            builder.Services.AddScoped<ITurmaService, TurmaService>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<IProblemDetailsFactory, ProblemDetailsFactory>();
            builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();

            builder.Services.AddScoped<IValidator<Aluno>, AlunoValidator>();
            builder.Services.AddScoped<IValidator<Turma>, TurmaValidator>();
            builder.Services.AddScoped<IValidator<Matricula>, MatriculaValidator>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            builder.Services.AddSwaggerConfiguration();


            var key = Encoding.UTF8.GetBytes("K0zLl1IM8Z8nECy5Zt3J+0/vXG4Q8qD5aZ2bL6XwVxA=");

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); 
            app.UseAuthorization(); 

            app.MapControllers();

            app.Run();
        }
    }
}