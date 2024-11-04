using Candidate_BussinessObjs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_DAOs
{
    public class JobPostingDAOs
    {
        private List<CandidateProfile> candidateProfilefiles;
        
        private static JobPostingDAOs instance;
        private ArrayList jobPostings;
        private readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileTxt/jobPostings.txt");
        private readonly string postingFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileTxt/jobPostings.txt");
        public static JobPostingDAOs Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new JobPostingDAOs();
                }
                return instance;
            }
        }

        public JobPostingDAOs()
        {
            jobPostings = new ArrayList();
            candidateProfilefiles = LoadCandidateProfiles();
            LoadJobPostingsFromFile();
        }

        private List<CandidateProfile> LoadCandidateProfiles()
        {
            var candidates = new List<CandidateProfile>();
            if (File.Exists(postingFilePath))
            {
                var lines = File.ReadAllLines(postingFilePath);
                foreach (var line in lines.Skip(1))
                {
                    var data = line.Split('\t');
                    if (data.Length >= 2)
                    {
                        var candidate = new CandidateProfile
                        {
                            CandidateId = data[0],
                            Fullname = data[1],
                            
                        };
                        candidates.Add(candidate);
                    }
                }
            }
            else
            {
                using StreamReader sr = File.OpenText(filePath);
            }
            return candidates;
        }

        public JobPosting GetJobPostingsByID(string jobid)
        {
            return jobPostings.Cast<JobPosting>().SingleOrDefault(j => j.PostingId.Equals(jobid));
        }

        public bool AddJobPosting(JobPosting jobPosting)
        {
            bool isSuccess = false;
            JobPosting jobPosting1 = GetJobPostingsByID(jobPosting.PostingId);
            if (jobPosting1 == null)
            {
                jobPostings.Add(jobPosting);
                SaveJobPostingsToFile();
                isSuccess = true;
            }
            return isSuccess;
        }

        public bool deleteJobPosting(string postingID)
        {
            bool isSuccess = false;
            JobPosting jobPosting = GetJobPostingsByID(postingID);

            if (jobPosting != null)
            {
                jobPostings.Remove(jobPosting);
                SaveJobPostingsToFile();
                isSuccess = true;
            }

            return isSuccess;
        }

        public bool updateJobPosting(JobPosting jobPosting)
        {
            bool isSuccess = false;
            JobPosting existPosting = GetJobPostingsByID(jobPosting.PostingId);
            if (existPosting != null)
            {
                jobPostings.Remove(jobPosting);
                jobPostings.Add(existPosting);
                
                SaveJobPostingsToFile();
                isSuccess = true;
            }
            return isSuccess;
        }

        private void LoadJobPostingsFromFile()
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var data = line.Split('\t');
                    if (data.Length >= 4)
                    {
                        var jobPosting = new JobPosting
                        {
                            PostingId = data[0],
                            JobPostingTitle = data[1],
                            Description = data[2],
                            PostedDate = DateTime.Parse(data[3]),
                        };
                        jobPostings.Add(jobPosting);
                    }
                }
                
            }
        }

        private void SaveJobPostingsToFile()
        {
            var lines = jobPostings.Cast<JobPosting>().Select(j => $"{j.PostingId}\t{j.JobPostingTitle}").ToList();
            File.WriteAllLines(filePath, lines);
        }

        public List<JobPosting> GetJobPostings()
        {
            List<JobPosting > jobs = new List<JobPosting>();
            foreach (JobPosting job in jobPostings)
            {
                var posting = candidateProfilefiles.SingleOrDefault(p => p.PostingId == job.PostingId);
                job.CandidateProfiles = candidateProfilefiles;
                if (candidateProfilefiles == null)
                {
                    Console.WriteLine("Not Found");
                }

                jobs.Add(job);
            }
            return jobs;
        }
    }
}
