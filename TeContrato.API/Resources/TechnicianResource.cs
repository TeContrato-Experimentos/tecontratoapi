﻿using System.Collections.Generic;
using TeContrato.API.Domain.Models;

namespace TeContrato.API.Resources
{
    public class TechnicianResource : UserResource
    {
        public int CTechnician { get; set; }
        public string TBio { get; set; }
        public string NEducation { get; set; }
        public int Numphone { get; set; }
        public IList<Project> CProjects { get; set; }
    }
}