using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using XmtSchool_TeachersApp.Models;
using XmtSchoolTypes.Tests;
using XmtSchoolTypes.Users;
using XmtSchoolWebApi.Types.ResultTypes;

namespace XmtSchool_TeachersApp.Cache
{
    internal class ApiCache
    {
        internal List<User> Users = new();
        internal List<TestWithAuthorName> Tests = new();
        internal List<Mark> Marks = new();
        
        internal async Task<List<User>> UpdateUsersAsync()
        {
            UsersResultTypes.GetUsersWithSensitiveDataResult? result;

            try
            {
                result = await Api.Api.GetAllUsersWithSensitiveDataAsync();

                if (result == null)
                    return new List<User>();

                Users = result.Data.Users;
            }
            catch (Exception ex)
            {
                MessageBoxBuilderModel messageBuilder = new MessageBoxBuilderModel()
                {
                    Message = $"{ex.Message}",
                    Icon = MessageBoxImage.Error,
                    Title = "Error while fetching users",
                    Button = MessageBoxButton.OK
                };

                messageBuilder.ShowMessage();
            }

            return Users;
        }

        internal async Task<List<TestWithAuthorName>> UpdateTestsAsync()
        {
            TestsResultTypes.GetTestsWithSensitiveDataResult? result;

            try
            {
                result = await Api.Api.GetDisclosedTestsAsync();

                if (result == null)
                    return new List<TestWithAuthorName>();

                Tests = result.Data.Tests ?? new List<TestWithAuthorName>();
            }
            catch (Exception ex)
            {
                MessageBoxBuilderModel messageBuilder = new MessageBoxBuilderModel()
                {
                    Message = $"{ex.Message}",
                    Icon = MessageBoxImage.Error,
                    Title = "Error while fetching tests",
                    Button = MessageBoxButton.OK
                };

                messageBuilder.ShowMessage();
            }

            return Tests;
        }

        internal async Task<List<Mark>> UpdateMarksAsync()
        {
            MarksResultTypes.GetAllMarksResult? result;

            try
            {
                result = await Api.Api.GetAllMarksAsync();

                if (result == null)
                    return new List<Mark>();

                Marks = result.Data ?? new List<Mark>();
            }
            catch (Exception ex)
            {
                MessageBoxBuilderModel messageBuilder = new MessageBoxBuilderModel()
                {
                    Message = $"{ex.Message}",
                    Icon = MessageBoxImage.Error,
                    Title = "Error while fetching marks",
                    Button = MessageBoxButton.OK
                };

                messageBuilder.ShowMessage();
            }

            return Marks;
        }
    }
}
