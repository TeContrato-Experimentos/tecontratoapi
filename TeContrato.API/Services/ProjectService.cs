﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeContrato.API.Domain.Models;
using TeContrato.API.Domain.Persistence.Repositories;
using TeContrato.API.Domain.Services;
using TeContrato.API.Domain.Services.Communications;

namespace TeContrato.API.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ITechnicianRepository _TechnicianRepository;
        private readonly IBudgetRepository _budgetRepository;
        public readonly IUnitOfWork _unitOfWork;


        public ProjectService(IProjectRepository projectRepository, IClientRepository clientRepository, ITechnicianRepository TechnicianRepository, IBudgetRepository budgetRepository, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _clientRepository = clientRepository;
            _TechnicianRepository = TechnicianRepository;
            _budgetRepository = budgetRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectResponse> DeleteAsync(int id)
        {
            var existingTag = await _projectRepository.FindById(id);

            if (existingTag == null)
                return new ProjectResponse("Project not found");

            try
            {
                _projectRepository.Remove(existingTag);
                await _unitOfWork.CompleteAsync();
                return new ProjectResponse(existingTag);
            }
            catch (Exception ex)
            {
                return new ProjectResponse($"An error ocurred while deleting Project: {ex.Message}");
            }
        }

        public async Task<ProjectResponse> GetByIdAsync(int id)
        {
            var existingTag = await _projectRepository.FindById(id);

            if (existingTag == null)
                return new ProjectResponse("Project not found");
            return new ProjectResponse(existingTag);
        }
        
        public async Task<ProjectResponse> SaveAsync(Project city)
        {
            try
            {
                await _projectRepository.AddAsync(city);
                await _unitOfWork.CompleteAsync();

                return new ProjectResponse(city);
            }
            catch (Exception e)
            {
                return new ProjectResponse($"Ocurrió un Error: {e.Message}");
            }
        }
        
        public async Task<ProjectResponse> SaveAsync(int clientId, int TechnicianId, int budgetId,Project project)
        {
            var existingClient = await _clientRepository.FindById(clientId);
            var existingTechnician = await _TechnicianRepository.FindById(TechnicianId);
            var existingBudget = await _budgetRepository.FindById(budgetId);

            if (existingClient == null)
                return new ProjectResponse("Client not found");
                
            if (existingTechnician == null)
                return new ProjectResponse("Technician not found");
            
            if (existingBudget == null)
                return new ProjectResponse("Budget not found");
            
            try
            {
                project.ClientId = clientId;
                project.TechnicianId = TechnicianId;
                project.BudgetId = budgetId;
                
                await _projectRepository.AddAsync(project);
                await _unitOfWork.CompleteAsync();

                return new ProjectResponse(project);
            }
            catch (Exception e)
            {
                return new ProjectResponse($"Ocurrió un Error: {e.Message}");
            }
        }

        public async Task<IEnumerable<Project>> ListAsync()
        {
            return await _projectRepository.ListAsync();

        }

        public async Task<ProjectResponse> GetByTechnicianIdAndBudgetId(int technicianId, int budgetId)
        {
            var existingProject = await _projectRepository.FindByTechnicianIdAndBudgetId(technicianId, budgetId);
            if (existingProject == null)
            {
                return new ProjectResponse("Project not found");
            }

            return new ProjectResponse(existingProject);
        }
        
        public async Task<ProjectResponse> GetByClientIdAndProjectId(int clientId, int budgetId)
        {
            var existingProject = await _projectRepository.FindByClientIdAndProjectId(clientId, budgetId);
            if (existingProject == null)
            {
                return new ProjectResponse("Project not found");
            }

            return new ProjectResponse(existingProject);
        }
        
        public async Task<ProjectResponse> GetByTechnicianIdAndProjectId(int technicianId, int budgetId)
        {
            var existingProject = await _projectRepository.FindByClientIdAndProjectId(technicianId, budgetId);
            if (existingProject == null)
            {
                return new ProjectResponse("Project not found");
            }

            return new ProjectResponse(existingProject);
        }

        public async Task<ProjectResponse> UpdateAsync(int id, Project city)
        {
            var existingCity = await _projectRepository.FindById(id);

            if (existingCity == null)
                return new ProjectResponse("Project not found");

            existingCity.Nproject = city.Nproject;
            existingCity.Tdescription = city.Tdescription;

            try
            {
                _projectRepository.Update(existingCity);

                return new ProjectResponse(existingCity);
            }
            catch (Exception ex)
            {
                return new ProjectResponse($"An error ocurred while updating the Project: {ex.Message}");
            }

        }
    }
}