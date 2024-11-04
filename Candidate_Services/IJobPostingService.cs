﻿using Candidate_BussinessObjs;
using Candidate_DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_Services
{
    public interface IJobPostingService
    {
        public JobPosting GetJobPostingByID(string jobid);
        public List<JobPosting> GetJobPostings();

        public bool updateJobPosting(JobPosting jobPosting);
        public bool deleteJobPosting(string postingID);
        public bool addJobPosting(JobPosting jobPosting);
    }
}
