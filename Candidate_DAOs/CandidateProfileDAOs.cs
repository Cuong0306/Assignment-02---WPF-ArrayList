using Candidate_BussinessObjs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Candidates_DAOs
    {
        public class CandidateProfileDAOs
        {
            private List<JobPosting> jobPostings;
            private ArrayList candidateProfiles;
            private static CandidateProfileDAOs instance;
            private readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileTxt/candidateProfiles.txt");
            private readonly string postingFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileTxt/jobPostings.txt");
            public static CandidateProfileDAOs Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new CandidateProfileDAOs();
                    }
                    return instance;
                }
            }

            public CandidateProfileDAOs()
            {
                candidateProfiles = new ArrayList();
                jobPostings = LoadJobPostings();
                LoadData();
            }

            private void LoadData()
            {
                if (File.Exists(filePath))
                {
                    var lines = File.ReadAllLines(filePath);
                    foreach (var line in lines)
                    {
                        var data = line.Split('\t');
                        if (data.Length >= 6)
                        {
                            var candidateProfile = new CandidateProfile
                            {
                                CandidateId = data[0],
                                Fullname = data[1],
                                Birthday = DateTime.Parse(data[2]),
                                ProfileShortDescription = data[3],
                                ProfileUrl = data[4],
                                PostingId = data[5]
                            };
                            candidateProfiles.Add(candidateProfile);
                        }
                    }
                }
            }

            private List<JobPosting> LoadJobPostings()
            {
                var postings = new List<JobPosting>();
                if (File.Exists(postingFilePath))
                {
                    var lines = File.ReadAllLines(postingFilePath);
                    foreach (var line in lines.Skip(1))
                    {
                        var data = line.Split('\t');
                        if (data.Length >= 2)
                        {
                            var jobPosting = new JobPosting
                            {
                                PostingId = data[0],
                                JobPostingTitle = data[1]
                            };
                            postings.Add(jobPosting);
                        }
                    }
                }
                else
                {
                    using StreamReader sr = File.OpenText(filePath);
                }
                return postings;
            }

            public CandidateProfile GetCandidateProfile(string id)
            {
                return candidateProfiles.Cast<CandidateProfile>().SingleOrDefault(c => c.CandidateId.Equals(id));
            }

            public bool AddCandidateProfile(CandidateProfile candidateProfile)
            {
                bool isSuccess = false;
                CandidateProfile candidate = GetCandidateProfile(candidateProfile.CandidateId);
                if (candidate == null)
                {
                    candidateProfiles.Add(candidateProfile);
                    SaveDataToFile();
                    isSuccess = true;
                }
                return isSuccess;
            }

            public bool DeleteCandidateProfile(string candidateID)
            {
                bool isSuccess = false;
                CandidateProfile candidate = GetCandidateProfile(candidateID);
                if (candidate != null)
                {
                    candidateProfiles.Remove(candidate);
                    SaveDataToFile();
                    isSuccess = true;
                }
                return isSuccess;
            }

            public bool UpdateCandidateProfile(CandidateProfile candidateProfile)
            {
                bool isSuccess = false;
                CandidateProfile existingCandidate = GetCandidateProfile(candidateProfile.CandidateId);
                if (existingCandidate != null)
                {
                    candidateProfiles.Remove(existingCandidate);
                    candidateProfiles.Add(candidateProfile);
                    SaveDataToFile();
                    isSuccess = true;
                }
                return isSuccess;
            }

            private void SaveDataToFile()
            {
                var lines = candidateProfiles.Cast<CandidateProfile>()
                                             .Select(c=> $"{c.CandidateId}\t{c.Fullname}\t{c.Birthday:yyyy-MM-dd HH:mm:ss.fff}\t{c.ProfileShortDescription}\t{c.ProfileUrl}\t{c.PostingId}");
                File.WriteAllLines(filePath, lines);
            }

            public List<CandidateProfile> GetCandidateProfiles()
            {
                List<CandidateProfile> candidates = new List<CandidateProfile>();
                foreach (CandidateProfile candidate in candidateProfiles)
                {
                    var posting = jobPostings.SingleOrDefault(p => p.PostingId == candidate.PostingId);
                    candidate.Posting = posting;

                    if (posting == null)
                    {
                        Console.WriteLine($"No JobPosting found for CandidateId: {candidate.CandidateId} with PostingId: {candidate.PostingId}");
                    }

                    candidates.Add(candidate);
                }
                return candidates;
            }
        }
    }

