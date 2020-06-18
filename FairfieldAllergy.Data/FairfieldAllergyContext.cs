using FairfieldAllergy.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FairfieldAllergy.Data
{
    public class FairfieldAllergyDbContext : IdentityDbContext<FairfieldIdentityUser>
    {
        public DbSet<Patient> Patients { get; set; }

        public FairfieldAllergyDbContext(DbContextOptions<FairfieldAllergyDbContext> options)
     : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}

