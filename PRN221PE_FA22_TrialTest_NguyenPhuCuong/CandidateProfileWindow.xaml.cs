using Candidate_BussinessObjs;
using Candidate_Repositories;
using Candidate_Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PRN221PE_FA22_TrialTest_NguyenPhuCuong
{
    public partial class CandidateProfileWindow : Window
    {
        private readonly ICandidateProfileService profileService;
        private readonly IJobPostingService jobPostingService;

        public CandidateProfileWindow()
        {
            InitializeComponent();
            profileService = new CandidateProfileService();
            jobPostingService = new JobPostingService();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGrdCandidateProfileManagement();
        }

        private void LoadGrdCandidateProfileManagement()
        {
            this.DataGrid_CandidateProfile.ItemsSource = profileService.GetCandidateProfiles();
            this.cbxJobPosting_CandidateProfile.ItemsSource = jobPostingService.GetJobPostings();
            this.cbxJobPosting_CandidateProfile.DisplayMemberPath = "JobPostingTitle";
            this.cbxJobPosting_CandidateProfile.SelectedValuePath = "PostingId";
        }

        private void btnAdd_CandidateProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var candidate = new CandidateProfile
                {
                    Fullname = txtFullName_CandidateProfile.Text,
                    CandidateId = txtCandidateID_CandidateProfile.Text,
                    ProfileShortDescription = txtProfileShortDescription_CandidateProfile.Text,
                    Birthday = txtBirthday_CandidateProfile.SelectedDate ?? DateTime.Now, // Thêm giá trị mặc định nếu không có
                    ProfileUrl = txtImageURL_CandidateProfile.Text,
                    PostingId = cbxJobPosting_CandidateProfile.SelectedValue?.ToString()
                };

                if (profileService.AddCandidateProfile(candidate))
                {
                    MessageBox.Show("Add Success");
                    LoadGrdCandidateProfileManagement();
                }
                else
                {
                    MessageBox.Show("Failed to add candidate profile.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDelete_CandidateProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGrid_CandidateProfile.SelectedItem == null)
                {
                    MessageBox.Show("Please select a candidate profile to delete.");
                    return;
                }

                var selectedCandidateProfile = DataGrid_CandidateProfile.SelectedItem as CandidateProfile;
                if (selectedCandidateProfile != null)
                {
                    bool isDeleted = profileService.DeleteCandidateProfile(selectedCandidateProfile.CandidateId);
                    MessageBox.Show(isDeleted ? "Deleted successfully." : "Failed to delete.");
                    LoadGrdCandidateProfileManagement();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void DataGrid_CandidateProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid_CandidateProfile.SelectedItem is CandidateProfile selectedProfile)
            {
                txtCandidateID_CandidateProfile.Text = selectedProfile.CandidateId;
                txtFullName_CandidateProfile.Text = selectedProfile.Fullname;
                txtProfileShortDescription_CandidateProfile.Text = selectedProfile.ProfileShortDescription;
                txtBirthday_CandidateProfile.SelectedDate = selectedProfile.Birthday; // Sử dụng SelectedDate
                txtImageURL_CandidateProfile.Text = selectedProfile.ProfileUrl;
                cbxJobPosting_CandidateProfile.SelectedValue = selectedProfile.PostingId;
            }
        }

        private void btnClose_CandidateProfile_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_CandidateProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCandidateID_CandidateProfile.Text))
                {
                    MessageBox.Show("You must select a candidate!");
                    return;
                }

                var existingProfile = profileService.GetCandidateProfile(txtCandidateID_CandidateProfile.Text);
                if (existingProfile != null)
                {
                    existingProfile.Fullname = txtFullName_CandidateProfile.Text;
                    existingProfile.ProfileShortDescription = txtProfileShortDescription_CandidateProfile.Text;
                    existingProfile.Birthday = txtBirthday_CandidateProfile.SelectedDate ?? DateTime.Now; // Thêm giá trị mặc định nếu không có
                    existingProfile.ProfileUrl = txtImageURL_CandidateProfile.Text;
                    existingProfile.PostingId = cbxJobPosting_CandidateProfile.SelectedValue?.ToString();

                    if (profileService.UpdateCandidateProfile(existingProfile))
                    {
                        MessageBox.Show("Candidate profile updated successfully.");
                        LoadGrdCandidateProfileManagement();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update candidate profile.");
                    }
                }
                else
                {
                    MessageBox.Show("Candidate not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void btnJobPosting_Click(object sender, RoutedEventArgs e)
        {
            JobPostingWindow jobPostingWindow = new JobPostingWindow();
            jobPostingWindow.Show();
            this.Close();
        }
    }
}
