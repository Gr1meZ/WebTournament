﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebTournament.Domain.Objects.AgeGroup;
using WebTournament.Domain.Objects.Belt;
using WebTournament.Domain.Objects.Bracket;
using WebTournament.Domain.Objects.BracketWinner;
using WebTournament.Domain.Objects.Club;
using WebTournament.Domain.Objects.Fighter;
using WebTournament.Domain.Objects.Tournament;
using WebTournament.Domain.Objects.Trainer;
using WebTournament.Domain.Objects.WeightCategorie;
using WebTournament.Domain.SeedWork;
using WebTournament.Infrastructure.Data.Repository;
using WebTournament.Infrastructure.Data.UoW;

namespace WebTournament.Infrastructure.IoC
{
    public static class NativeInjector
    {
        public static  IServiceCollection AddCustomServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddScoped<IAgeGroupRepository, AgeGroupRepository>();
            services.AddScoped<IBeltRepository, BeltRepository>();
            services.AddScoped<IClubRepository, ClubRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();
            services.AddScoped<ITrainerRepository, TrainerRepository>();
            services.AddScoped<IWeightCategorieRepository, WeightCategorieRepository>();
            services.AddScoped<IFighterRepository, FighterRepository>();
            services.AddScoped<IBracketRepository, BracketRepository>();
            services.AddScoped<IBracketWinnerRepository, BracketWinnerRepository>();

            return services;
        }
    }
}